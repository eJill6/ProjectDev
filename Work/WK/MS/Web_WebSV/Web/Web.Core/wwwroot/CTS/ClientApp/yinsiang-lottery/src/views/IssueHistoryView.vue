<template>
  <div class="h-100 second-dark-content rounded-main">
    <div class="position-relative">
      <div class="bg-darkbg text-white fw-bold rounded-main history_page_title">
        开奖记录
      </div>
      <div class="position-absolute backbtn" @click="navigateToBet"></div>
    </div>
    <div
      class="h-with-title overflow-scroll-y no-scrollbar"
      @scroll="onScroll"
      ref="scrollContainer"
    >
      <div
        :style="{
          'padding-top': virtualScroll.paddingTop + 'px',
          'padding-bottom': virtualScroll.paddingBottom + 'px',
        }"
      >
        <component
          :is="issueHistoryComponentName"
          :list="virtualScroll.list"
        ></component>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import { IssueHistorys } from "@/components";
import api from "@/api";
import { MutationType } from "@/store";
import { VirtualScroll, MqEvent } from "@/mixins";
import AssetImage from "@/components/shared/AssetImage.vue";
import { event as eventModel } from "@/models";
import { bigWinNumberList } from "@/gameConfig";

export default defineComponent({
  components: { ...IssueHistorys, AssetImage },
  mixins: [VirtualScroll, MqEvent],
  data() {
    return {
      nextCursor: "",
      hasNextPage: true,
    };
  },
  methods: {
    navigateToBet() {
      this.$router.push({ name: "Bet" });
    },
    onLotteryDraw(arg: eventModel.LotteryDrawArg) {
      if (arg.LotteryID !== this.$store.state.lotteryInfo.lotteryId) return;
      if (arg.IssueNo !== this.$store.state.issueNo.lastIssueNo) return;

      this.$store.commit(MutationType.DrawIssueNo, arg.CurrentLotteryNum);
      this.loadAsync();
    },
    async loadAsync() {
      try {
        if (this.isLoading) return;
        if (!this.hasNextPage) return;

        this.$store.commit(MutationType.SetIsLoading, true);

        let count = 30;
        let result = await api.getIssueHistoryAsync(
          this.lotteryInfo.lotteryId,
          count,
          this.nextCursor
        );
        this.list = this.list.concat(result.list);
        this.nextCursor = result.nextCursor;
        this.hasNextPage = !!result.nextCursor;
      } catch (error) {
        console.error(error);
      } finally {
        this.$store.commit(MutationType.SetIsLoading, false);
      }

      this.calculateVirtualScroll();
    },
    $_onScrollToBottom() {
      this.loadAsync();
    },
  },
  mounted() {
    this.loadAsync();
  },
  computed: {
    isLoading() {
      return this.$store.state.isLoading;
    },
    lotteryInfo() {
      return this.$store.state.lotteryInfo;
    },
    issueHistoryComponentName(): string {
      return this.lotteryInfo.gameTypeName;
    },
    $_virtualScrollItemElemHeight() {
      const isBigWinNumber =
        bigWinNumberList.indexOf(this.$store.state.lotteryInfo.gameTypeName) >
        -1;
      return isBigWinNumber ? 89 : 35;
    },
  },
});
</script>
