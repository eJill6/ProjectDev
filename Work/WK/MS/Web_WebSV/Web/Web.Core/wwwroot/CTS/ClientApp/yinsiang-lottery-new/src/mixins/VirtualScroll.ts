import { defineComponent } from "vue";

export default defineComponent({
  data() {
    return {
      list: [] as unknown[],
      virtualScroll: {
        list: [] as unknown[],
        paddingTop: 0,
        paddingBottom: 0,
      },
    };
  },
  methods: {
    calculateVirtualScroll() {
      let container = this.$refs.scrollContainer as HTMLDivElement;

      if (!container) return;

      let itemElemHeight = this.$_virtualScrollItemElemHeight;
      let itemCountPerView = Math.ceil(container.clientHeight / itemElemHeight);

      if (container.scrollTop < 0) return;
      let start = Math.floor(container.scrollTop / itemElemHeight);
      let end = Math.ceil(
        (itemElemHeight * itemCountPerView + container.scrollTop) /
          itemElemHeight
      );
      this.virtualScroll.list = this.list.slice(start, end);

      this.virtualScroll.paddingTop = start * itemElemHeight;
      this.virtualScroll.paddingBottom =
        (this.list.length - this.virtualScroll.list.length) * itemElemHeight -
        this.virtualScroll.paddingTop;
    },
    onScroll(e: Event) {
      let { scrollTop, offsetHeight, scrollHeight } =
        e.target as HTMLDivElement;
      if (scrollTop + offsetHeight >= scrollHeight) this.$_onScrollToBottom();
      else this.calculateVirtualScroll();
    },
    $_onScrollToBottom() {},
  },
  computed: {
    $_virtualScrollItemElemHeight() {
      throw new Error("Override");
    },
  },
});
