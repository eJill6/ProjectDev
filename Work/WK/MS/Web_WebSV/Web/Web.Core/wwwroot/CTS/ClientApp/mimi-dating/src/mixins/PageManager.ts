import { defineComponent } from "vue";
import { MutationType } from "@/store";
import { PaginationModel } from "@/models";
import api from "@/api";
import Tools from "./Tools";
import DialogControl from "./DialogControl";
import VirtualScroll from "./VirtualScroll";
import PlayGame from "./PlayGame";
import NavigateRule from "./NavigateRule";
import ScrollManager from "./ScrollManager";
import ImageCacheManager from "./ImageCacheManager";
import { PostType } from "@/enums";

export default defineComponent({
  data() {
    return {
      showBottomLoading: false,
    };
  },
  mixins: [
    DialogControl,
    VirtualScroll,
    PlayGame,
    NavigateRule,
    ScrollManager,
    Tools,
    ImageCacheManager,
  ],
  methods: {
    async reload() {
      this.cancelFetchAction();
      this.resetPageInfo();
      this.resetScroll();
      await this.loadAsync();
    },
    // 加载
    async loadAsync() {
      if (
        this.isLoading ||
        this.showBottomLoading ||
        this.isPromising ||
        this.scrollStatus.totalPage === this.pageInfo.pageNo
      )
        return;

      let loadImageNextPage = 0;
      try {
        this.showBottomLoading = this.scrollStatus.list.length > 0;
        if (!this.showBottomLoading) {
          this.$store.commit(MutationType.SetIsLoading, true);
        }

        const nextPage = this.pageInfo.pageNo + 1;
        const pageSize = 30;
        let result = {} as PaginationModel<any>;

        if (this.$_PostType === PostType.Official) {
          const search = this.officialSearchConditionModel(
            nextPage,
            this.filter,
            this.pageInfo.ts,
            pageSize
          );
          const productModel = await api.getOfficialProductList(search);
          result = productModel;
        } else {
          const search = this.searchConditionModel(
            nextPage,
            this.filter,
            this.pageInfo.ts
          );

          const productModel = await api.getProductList(search);
          result = productModel;
          let favoritePosts = this.$store.state.favoritePosts;
          let posts = productModel.data.filter((x) => x.isFavorite);
          if (posts.length) {
            posts.forEach((x) => {
              if (favoritePosts.indexOf(x.postId) < 0) {
                favoritePosts.push(x.postId);
              }
            });
          }
          this.$store.commit(MutationType.SetFavoritePosts, favoritePosts);
          await this.baseImageInfoDownload(result.data);
          this.nextPagePostCoverDownload(productModel.nextPagePost);
        }

        result.data.forEach((x) => {
          if (x.serviceItem.length > 3) {
            x.serviceItem = x.serviceItem.slice(0, 3);
          }
          if (x.title.length > 12) {
            x.title = x.title.slice(0, 12) + "...";
          }
        });

        this.scrollStatus.totalPage = result.totalPage;
        this.pageInfo.pageNo = result.pageNo;
        this.pageInfo.ts = result.ts;
        this.scrollStatus.list = this.scrollStatus.list.concat(result.data);
      } catch (error) {
        console.error(error);
      } finally {
        if (!this.showBottomLoading) {
          this.$store.commit(MutationType.SetIsLoading, false);
        } else {
          this.showBottomLoading = false;
        }
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
        await this.reload();
      }
    },
  },
  onBeforeUnmount() {
    this.cancelFetchAction();
  },
  computed: {
    currentPage() {
      return this.$store.state.currentPage;
    },
    showBottomLoadingBar() {
      return this.showBottomLoading || this.isPromising;
    },
  },
});
