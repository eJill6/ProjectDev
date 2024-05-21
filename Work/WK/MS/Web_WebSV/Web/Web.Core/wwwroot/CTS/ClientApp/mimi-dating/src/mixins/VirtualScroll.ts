import { PostType } from "@/enums";
import { ScrollStatusModel, VirtualScrollModel } from "@/models";
import { defineComponent } from "vue";

export default defineComponent({
  data() {
    return {
      magicNum: 120, //由上往下滑要超過的距離
      moveStart: 0,
      moveEnd: 0,
      result: {
        totalPage: 1,
        list: [],
        isTop: false,
        scrollTop: 0,
        virtualScroll: {
          list: [],
          paddingTop: 0,
          paddingBottom: 0,
        } as VirtualScrollModel,
      } as ScrollStatusModel,
    };
  },
  methods: {
    calculateVirtualScroll() {
      
      let container = this.$refs.scrollContainer as HTMLDivElement;
      if (!container) return;


      const columnCount = this.postType === PostType.Official ? 2 : 1;
      let itemElemHeight = this.$_virtualScrollItemElemHeight;
      let itemCountPerView = Math.ceil(container.clientHeight / itemElemHeight);
      this.scrollStatus.scrollTop = container.scrollTop;

      if (this.scrollStatus.scrollTop < 0 && this.moveStart > 0) return;

      let start = Math.floor(this.scrollStatus.scrollTop / itemElemHeight);

      let end = Math.ceil(
        (itemElemHeight * itemCountPerView + this.scrollStatus.scrollTop) /
          itemElemHeight
      );

      start = start > 0 ? start - 1 : start;
      end =
        this.scrollStatus.list.length > 0 && !end
          ? this.scrollStatus.list.length
          : end;

      let spliceStart = start * columnCount;
      const spliceEnd = end * columnCount;
      spliceStart = spliceStart < 0 ? 0 : spliceStart;
      this.scrollStatus.virtualScroll.list = this.scrollStatus.list.slice(
        spliceStart,
        spliceEnd
      );
      this.scrollStatus.virtualScroll.paddingTop = start * itemElemHeight;

      this.scrollStatus.virtualScroll.paddingBottom =
        (this.scrollStatus.list.length -
          this.scrollStatus.virtualScroll.list.length) *
          (itemElemHeight / columnCount) -
        this.scrollStatus.virtualScroll.paddingTop;
    },
    onScroll(e: Event) {
      let { scrollTop, offsetHeight, scrollHeight } =
        e.target as HTMLDivElement;
      const scrollTopValue = Math.ceil(scrollTop); //解決android取出小數，導致位置算錯的問題
      this.checkContainerIsTop();
      if (scrollTopValue + offsetHeight >= scrollHeight)
        this.$_onScrollToBottom();
      else this.calculateVirtualScroll();
    },
    onTouchStart(e: TouchEvent) {
      this.checkContainerIsTop();
      this.moveStart = e.touches[0].clientY;
      this.moveEnd = 0;
    },
    onTouchMove(e: TouchEvent) {
      this.moveEnd = e.touches[0].clientY;
    },
    onMouseDown(e: MouseEvent) {
      this.checkContainerIsTop();
      this.moveStart = e.clientY;
      this.moveEnd = 0;
    },
    onMouseUp(e: MouseEvent) {
      this.moveEnd = e.clientY;
      this.endEvent();
    },
    endEvent() {
      let container = this.$refs.scrollContainer as HTMLDivElement;
      if (!container) return;

      const gap = this.moveEnd - this.moveStart;

      if (this.scrollStatus.isTop && gap > this.magicNum) {
        this.moveStart = 0;
        this.moveEnd = 0;
        this.$_onScrollReload();
      }
    },
    checkContainerIsTop() {
      let container = this.$refs.scrollContainer as HTMLDivElement;
      if (!container) return;
      this.scrollStatus.isTop = container.scrollTop <= 0;
    },
    $_onScrollReload() {},
    $_onScrollToBottom() {},
  },
  computed: {
    $_virtualScrollItemElemHeight() {
      throw new Error("Override");
    },
    $_scrollDataUseStore() {
      return true;
    },
    scrollStatus() {
      return (
        this.$_scrollDataUseStore ? this.$store.state.scrollStatus : this.result
      ) as ScrollStatusModel;
    },
    postType() {
      return this.$store.state.postType;
    },
  },
});