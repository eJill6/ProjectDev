import { HomeStatusInfoModel, LabelFilterModel, OfficialShopModel, ProductListModel, VirtualScrollModel } from "@/models";
import { MutationType } from "@/store";
import { defineComponent } from "vue";

export default defineComponent({
  data() {
    return {};
  },
  methods: {
    setContainerTop() {
      let container = this.$refs.scrollContainer as HTMLDivElement;

      if (!container) return;
      container.scrollTop = this.scrollStatus.scrollTop;
    },
    resetScroll() {
      this.scrollStatus.list = [] as unknown[];
      this.scrollStatus.virtualScroll.list = [] as unknown[];
      this.scrollStatus.virtualScroll.paddingTop = 0;
      this.scrollStatus.virtualScroll.paddingBottom = 0;
      this.scrollStatus.scrollTop = 0;
      this.scrollStatus.totalPage = 1;
      this.saveScrollStatus();
    },
    resetPageInfo() {
      this.pageInfo.pageNo = 0;
      this.pageInfo.pageSize = 30;
      this.pageInfo.ts = "";
      this.savePageInfoStatus();
    },
    savePageInfoStatus() {
      this.$store.commit(MutationType.SetPageInfo, this.pageInfo);
    },
    saveScrollStatus() {
      this.$store.commit(MutationType.SetScrollStatus, this.scrollStatus);
    },
    initParameter() {
      this.resetScroll();
      this.resetPageInfo();
      this.$store.commit(MutationType.SetHomeStatus, {
        //officialIndex: 0,
        //agencyIndex: 0,
        scrollTop: 0,
        homeAgencyList: [] as ProductListModel[],
        homeOfficialList: [] as OfficialShopModel[],
      } as HomeStatusInfoModel);
      this.$store.commit(MutationType.SetFilter, {} as LabelFilterModel);
    },
    initPageInfo() {
      if (!Object.keys(this.pageInfo).length) {
        this.pageInfo = {
          pageNo: 0,
          pageSize: 30,
          ts: "",
        };
        this.$store.commit(MutationType.SetPageInfo, this.pageInfo);
      }
    },
  },
  created() {
    this.initPageInfo();
  },
  mounted() {
    this.setContainerTop();
  },
  computed: {
    scrollStatus() {
      let result = this.$store.state.scrollStatus;
      if (!result.virtualScroll)
        result.virtualScroll = {} as VirtualScrollModel;
      return result;
    },
    pageInfo() {
      return this.$store.state.pageInfo;
    },
    filter() {
      return this.$store.state.filter;
    },
  },
});
