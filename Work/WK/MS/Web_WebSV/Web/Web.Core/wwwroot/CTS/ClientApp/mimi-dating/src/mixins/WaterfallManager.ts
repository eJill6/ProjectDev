import { defineComponent } from "vue";
import { MutationType } from "@/store";
import {
  PaginationModel,
  ProductListModel,
  WaterfallListModel,
  WaterfallModel,
} from "@/models";
import api from "@/api";
import Tools from "./Tools";
import DialogControl from "./DialogControl";
import PlayGame from "./PlayGame";
import NavigateRule from "./NavigateRule";
import ScrollManager from "./ScrollManager";
import ImageCacheManager from "./ImageCacheManager";

export default defineComponent({
  data() {
    return {
      magicNum: 120, //由上往下滑要超過的距離
      showWaterfallBottomLoading: false,
      moveStart: 0,
      moveEnd: 0,
      isEdit: false,
    };
  },
  mixins: [
    DialogControl,
    PlayGame,
    NavigateRule,
    ScrollManager,
    Tools,
    ImageCacheManager,
  ],
  watch: {
    isEdit(value: Boolean) {
      this.isEdit = false;
      if (value) {
        this.$nextTick(() => {
          this.checkWaterfallView();
        });
      }
    },
    showWaterfallBottomLoadingBar(value: Boolean) {
      if (!value) {
        // this.$store.commit(MutationType.SetIsLoading, true);
        // this.$nextTick(() => {
        setTimeout(() => {
          this.resetWaterfallView();
          // this.$store.commit(MutationType.SetIsLoading, false);
        }, 200);
        // });
      }
    },
  },
  methods: {
    resetWaterfallModel() {
      this.cancelFetchAction();
      this.resetPageInfo();
      this.resetScroll();
      const waterfallStatus = {} as WaterfallModel;
      this.$store.commit(MutationType.SetWaterfallStatus, waterfallStatus);
    },
    async reloadWaterfall() {
      this.resetWaterfallModel();
      await this.loadWaterfallAsync();
    },
    // 加载
    async loadWaterfallAsync() {
      if (
        this.isLoading ||
        this.showWaterfallBottomLoading ||
        this.isPromising ||
        this.scrollStatus.totalPage === this.pageInfo.pageNo
      )
        return;

      try {
        this.showWaterfallBottomLoading = this.scrollStatus.list.length > 0;
        if (!this.showWaterfallBottomLoading) {
          this.$store.commit(MutationType.SetIsLoading, true);
        }

        const nextPage = this.pageInfo.pageNo + 1;
        const pageSize = 20;

        const search = this.searchConditionModel(
          nextPage,
          this.filter,
          this.pageInfo.ts,
          pageSize
        );
        const result = await api.getProductList(search);

        let favoritePosts = this.$store.state.favoritePosts;

        for (let item of result.data) {
          item.coverUrl = item.coverUrl.replace("225-225-", "350-350-");

          if (item.serviceItem.length > 3) {
            item.serviceItem = item.serviceItem.slice(0, 3);
          }
          if (item.title.length > 12) {
            item.title = item.title.slice(0, 12) + "...";
          }
          if (item.isFavorite && favoritePosts.indexOf(item.postId) < 0) {
            favoritePosts.push(item.postId);
          }
        }
        
        if (result.nextPagePost) {
          for (let i = 0; i < result.nextPagePost.length; i++) {
            let item = result.nextPagePost[i];
            item.coverUrl = item.coverUrl.replace("225-225-", "350-350-");
          }
        }

        this.$store.commit(MutationType.SetFavoritePosts, favoritePosts);
        await this.baseImageInfoDownload(result.data);
        this.nextPagePostCoverDownload(result.nextPagePost);

        this.scrollStatus.totalPage = result.totalPage;
        this.pageInfo.pageNo = result.pageNo;
        this.pageInfo.ts = result.ts;
        this.scrollStatus.list = this.scrollStatus.list.concat(result.data);
        let waterfallStatus = this.$store.state.waterfallStatus;
        if (
          !Object.keys(waterfallStatus).length ||
          !this.scrollStatus.list ||
          !this.scrollStatus.list.length
        ) {
          waterfallStatus.leftList = [] as WaterfallListModel[];
          waterfallStatus.rightList = [] as WaterfallListModel[];
        }

        const leftList = result.data.filter((item, index) => {
          return !(index % 2);
        }) as WaterfallListModel[];

        const rightList = result.data.filter((item, index) => {
          return index % 2;
        }) as WaterfallListModel[];

        waterfallStatus.leftList = waterfallStatus.leftList.concat(leftList);
        waterfallStatus.rightList = waterfallStatus.rightList.concat(rightList);
        this.$store.commit(MutationType.SetWaterfallStatus, waterfallStatus);
      } catch (error) {
        console.error(error);
      } finally {
        if (!this.showWaterfallBottomLoading) {
          this.$store.commit(MutationType.SetIsLoading, false);
        } else {
          this.showWaterfallBottomLoading = false;
        }
      }
    },
    checkWaterfallView() {
      let leftContainer = this.$refs.leftWaterfall as HTMLDivElement;
      let rightContainer = this.$refs.rightWaterfall as HTMLDivElement;

      if (!leftContainer || !rightContainer) {
        return;
      }

      this.$store.state.waterfallStatus.leftList.forEach(
        (element) => (element.watterfallId = this.getUUID())
      );

      this.$store.state.waterfallStatus.rightList.forEach(
        (element) => (element.watterfallId = this.getUUID())
      );
      let leftDivsHeight = leftContainer.clientHeight;
      let rightDivsHeight = rightContainer.clientHeight;
      const leftDivs = leftContainer.children;
      const rightDivs = rightContainer.children;
      const isLeft = leftDivsHeight > rightDivsHeight;
      const expectRemoveDivElement = isLeft
        ? leftDivs[leftDivs.length - 1]
        : rightDivs[rightDivs.length - 1];
      const expectHeight = expectRemoveDivElement.clientHeight;

      const recheck =
        (isLeft && leftDivsHeight - expectHeight > rightDivsHeight) ||
        (!isLeft && rightDivsHeight - expectHeight > leftDivsHeight);
      if (recheck) {
        this.resetWaterfallView(recheck);
      }
    },
    resetWaterfallView(recheck: boolean = false) {
      let waterfallStatus = this.$store.state.waterfallStatus;
      let leftContainer = this.$refs.leftWaterfall as HTMLDivElement;
      let rightContainer = this.$refs.rightWaterfall as HTMLDivElement;

      if (
        !Object.keys(waterfallStatus).length ||
        !leftContainer ||
        !rightContainer ||
        !waterfallStatus.rightList.length
      ) {
        return;
      }
      if (leftContainer.clientHeight > rightContainer.clientHeight) {
        this.adjustWaterfallDiv(true, waterfallStatus, recheck);
      } else if (leftContainer.clientHeight < rightContainer.clientHeight) {
        this.adjustWaterfallDiv(false, waterfallStatus, recheck);
      }
    },
    adjustWaterfallDiv(
      isLeft: boolean,
      waterfallStatus: WaterfallModel,
      recheck: boolean
    ) {
      let leftContainer = this.$refs.leftWaterfall as HTMLDivElement;
      let rightContainer = this.$refs.rightWaterfall as HTMLDivElement;
      const leftDivs = leftContainer.children;
      const rightDivs = rightContainer.children;
      let leftDivsHeight = leftContainer.clientHeight;
      let rightDivsHeight = rightContainer.clientHeight;
      let moveItems: WaterfallListModel[] = [];
      let isCalculate = true;
      while (isCalculate) {
        ///取得要移動物件index
        const expectRemoveIndex =
          (isLeft ? leftDivs.length : rightDivs.length) -
          (moveItems.length + 1);
        ///取DIV
        const expectRemoveDivElement = isLeft
          ? leftDivs[expectRemoveIndex]
          : rightDivs[expectRemoveIndex];

        const expectHeight = expectRemoveDivElement.clientHeight;
        const leftExpectHeight = isLeft
          ? leftDivsHeight - expectHeight
          : leftDivsHeight;
        const rightExpectHeight = isLeft
          ? rightDivsHeight
          : rightDivsHeight - expectHeight;

        const stopCalculate = isLeft
          ? leftExpectHeight < rightExpectHeight
          : leftExpectHeight > rightExpectHeight;

        if (stopCalculate) {
          isCalculate = false;
        } else {
          leftDivsHeight = isLeft
            ? leftDivsHeight - expectHeight
            : leftDivsHeight + expectHeight;
          rightDivsHeight = isLeft
            ? rightDivsHeight + expectHeight
            : rightDivsHeight - expectHeight;
          const copyItem = isLeft
            ? waterfallStatus.leftList[expectRemoveIndex]
            : waterfallStatus.rightList[expectRemoveIndex];
          moveItems.push(copyItem);
        }
      }

      if (moveItems.length) {
        if (isLeft) {
          waterfallStatus.leftList.splice(
            waterfallStatus.leftList.length - moveItems.length,
            moveItems.length
          );
          waterfallStatus.rightList =
            waterfallStatus.rightList.concat(moveItems);
        } else {
          waterfallStatus.rightList.splice(
            waterfallStatus.rightList.length - moveItems.length,
            moveItems.length
          );
          waterfallStatus.leftList = waterfallStatus.leftList.concat(moveItems);
        }
        this.isEdit = !recheck;
      }
    },
    onWaterfallScroll(e: Event) {
      let { scrollTop, offsetHeight, scrollHeight } =
        e.target as HTMLDivElement;
      const scrollTopValue = Math.ceil(scrollTop); //解決android取出小數，導致位置算錯的問題

      this.$store.commit(MutationType.SetWaterfallTop, scrollTopValue);
      this.checkContainerIsTop();
      if (scrollTopValue + offsetHeight >= scrollHeight) {
        this.$_onWaterfallScrollToBottom();
      }
    },
    onWaterfallTouchStart(e: TouchEvent) {
      this.checkContainerIsTop();
      this.moveStart = e.touches[0].clientY;
      this.moveEnd = 0;
    },
    onWaterfallTouchMove(e: TouchEvent) {
      this.moveEnd = e.touches[0].clientY;
    },
    onWaterfallMouseDown(e: MouseEvent) {
      this.checkContainerIsTop();
      this.moveStart = e.clientY;
      this.moveEnd = 0;
    },
    onWaterfallMouseUp(e: MouseEvent) {
      this.moveEnd = e.clientY;
      this.endWaterfallEvent();
    },
    endWaterfallEvent() {
      let container = this.$refs.scrollContainer as HTMLDivElement;
      if (!container) return;

      const gap = this.moveEnd - this.moveStart;

      if (this.scrollStatus.isTop && gap > this.magicNum) {
        this.moveStart = 0;
        this.moveEnd = 0;
        this.reloadWaterfall();
      }
    },
    checkContainerIsTop() {
      let container = this.$refs.scrollContainer as HTMLDivElement;
      if (!container) return;
      this.scrollStatus.isTop = container.scrollTop <= 0;
    },
    async $_onWaterfallScrollToBottom() {
      await this.loadWaterfallAsync();
    },
    async checkTopToReturn() {
      if (!this.waterfallTopValue) {
        await this.loadWaterfallAsync();
      } else {
        let container = this.$refs.scrollContainer as HTMLDivElement;
        if (!container) return;

        container.scrollTop = this.waterfallTopValue;
      }
    },
  },
  onBeforeUnmount() {
    this.cancelFetchAction();
  },
  computed: {
    leftWaterfallSection() {
      return this.$store.state.waterfallStatus.leftList;
    },
    rightWaterfallSection() {
      return this.$store.state.waterfallStatus.rightList;
    },
    showWaterfallBottomLoadingBar() {
      return this.showWaterfallBottomLoading || this.isPromising;
    },
    waterfallTopValue() {
      return this.$store.state.waterfallTop;
    },
  },
});
