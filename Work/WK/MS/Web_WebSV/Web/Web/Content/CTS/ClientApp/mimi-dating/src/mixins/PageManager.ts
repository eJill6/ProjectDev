import { defineComponent } from "vue";
import { MutationType } from "@/store";
import { PostType } from "@/enums";
import { LabelFilterModel, ProductListModel } from "@/models";
import api from "@/api";
import Tools from "./Tools";
import DialogControl from "./DialogControl";
import VirtualScroll from "./VirtualScroll";
import PlayGame from "./PlayGame";
import NavigateRule from "./NavigateRule";
import ScrollManager from "./ScrollManager";

export default defineComponent({
  mixins: [
    DialogControl,
    VirtualScroll,
    PlayGame,
    NavigateRule,
    ScrollManager,
    Tools,
  ],
  data() {
    return {
      totalPage: 1,
    };
  },
  watch: {
    async searchStatus(value: Boolean) {
      if (value) {
        this.$store.commit(MutationType.SetSearchStatus, false);
        await this.reload();
      }
    },
  },
  methods: {
    async filterCondition(condition: LabelFilterModel) {
      this.$store.commit(MutationType.SetFilter, condition);
      await this.reload();
    },
    setPostType() {
      this.$store.commit(MutationType.SetPostType, PostType.Square);
    },
    async reload() {
      this.totalPage = 1;
      this.resetPageInfo();
      this.resetScroll();
      await this.loadAsync();
    },
    async loadAsync() {
      if (this.isLoading || this.totalPage === this.pageInfo.pageNo) return;
      try {
        this.$store.commit(MutationType.SetIsLoading, true);

        const nextPage = this.pageInfo.pageNo + 1;
        const search = this.searchConditionModel(
          nextPage,
          this.filter,
          this.pageInfo.ts
        );
        const result = await api.getProductList(search);

        this.totalPage = result.totalPage;
        this.pageInfo.pageNo = result.pageNo;
        this.pageInfo.ts = result.ts;
        this.scrollStatus.list = this.scrollStatus.list.concat(result.data);
      } catch (error) {
        console.error(error);
      } finally {
        this.$store.commit(MutationType.SetIsLoading, false);
      }

      this.calculateVirtualScroll();
    },
    $_onScrollReload() {
      this.reload();
    },
    async $_onScrollToBottom() {
      await this.loadAsync();
    },
    async checkDetailToReturn() {
      if (
        !Object.keys(this.scrollStatus.virtualScroll).length ||
        this.scrollStatus.list.length === 0
      ) {
        this.resetScroll();
        await this.loadAsync();
      }
    },
  },
  async created() {
    await this.checkDetailToReturn();
  },
  computed: {
    $_virtualScrollItemElemHeight() {
      return 144;
    },
    currentPage() {
      return this.$store.state.currentPage;
    },
    searchStatus() {
      return this.$store.state.searchStatus;
    },
    orderList() {
      return (this.scrollStatus.virtualScroll.list || []) as ProductListModel[];
    },
  },
});
