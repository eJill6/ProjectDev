<template>
  <!-- header second start -->
  <header class="header_second_height header_second_bg">
    <div class="header_middle">
      <div class="header_second_title">
        <p>开奖记录</p>
      </div>
    </div>
  </header>
  <!-- header second  end -->
  <!-- filter start-->
  <div class="record_filter">
    <div class="type_filter">
      <AssetImage :src="`@/assets/images/record/ic_gametype.png`" />
      <p>{{ lotteryInfo.lotteryTypeName }}</p>
    </div>
  </div>
  <!-- filter end-->

  <!-- 滑動區塊 start-->
  <div class="flex_second_height">
    <div class="overflow no-scrollbar" @scroll="onScroll" ref="scrollContainer">
      <div
        class="second_adding_basic"
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
import { AssetImage } from "@/components/shared";
import { event as eventModel } from "@/models";

export default defineComponent({
  components: { ...IssueHistorys, AssetImage },
  mixins: [VirtualScroll, MqEvent],
  data() {
    return {
      nextCursor: "",
      hasNextPage: true,
      // hasNewIssue: false
    };
  },
  methods: {
    navigateToBet() {
      this.$router.push({ name: "Bet" });
    },
    onLoadData(arg: eventModel.LotteryDrawArg) {
      this.loadAsync(arg);
    },
    async loadAsync(arg?: eventModel.LotteryDrawArg) {
      const hasNewIssue = !!arg;
      try {
        if (this.isLoading) return;
        if (!this.hasNextPage && !hasNewIssue) return;

        this.$store.commit(MutationType.SetIsLoading, true);

        let count = hasNewIssue ? 5 : 30;
        let result = await api.getIssueHistoryAsync(
          this.lotteryInfo.lotteryId,
          count,
          hasNewIssue ? "" : this.nextCursor
        );
        if (hasNewIssue) {
          const newIssueNo = {
            issueNo: arg.IssueNo,
            drawNumbers: (arg.CurrentLotteryNum || "").split(","),
          };
          let oldData = this.list.some(
            (item: any) => item.issueNo === newIssueNo.issueNo
          );
          if (!oldData) {
            this.list.unshift(newIssueNo);
          }
        } else {
          this.list = this.list.concat(result.list);
          this.nextCursor = result.nextCursor;
          this.hasNextPage = !!result.nextCursor;
        }
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
      return 53;
    },
  },
});
</script>
