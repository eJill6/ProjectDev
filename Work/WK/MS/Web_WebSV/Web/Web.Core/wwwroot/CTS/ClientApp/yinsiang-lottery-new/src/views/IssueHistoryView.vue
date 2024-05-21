<template>
  <div class="confirm_main">
    <!-- 開獎紀錄 start -->
    <div class="confirm_wrapper">
      <div class="confirm_outter">
        <div class="confirm_close" @click="navigateToBet">
          <AssetImage src="@/assets/images/modal/ic_popup_close.png" />
        </div>
        <div class="confirm_header">
          <div class="confirm_header_title" data-text="开奖记录">开奖记录</div>
        </div>
        <div class="flex_second_height w_100">
          <div
            class="overflow no-scrollbar"
            @scroll="onScroll"
            ref="scrollContainer"
          >
            <div
              class="second_adding_basic pd_0"
              :style="{
                'padding-top': virtualScroll.paddingTop + 'px',
                'padding-bottom': virtualScroll.paddingBottom + 'px',
              }"
            >
              <div
                :class="
                  isRoulette ? `roulette_content` : `record_content spacing`
                "
              >
                <component
                  :is="issueHistoryComponentName"
                  :list="virtualScroll.list"
                  :isFull="true"
                ></component>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
    <!-- 開獎紀錄 end -->
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
      this.rouletteHandle(arg);
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
      return this.isRoulette ? 29 : 54;
    },
    isRoulette() {
      const typeName = this.$store.state.lotteryInfo.gameTypeName as string;
      return (
        typeName.toLocaleLowerCase() === "lp" ||
        typeName.toLocaleLowerCase() === "yxx"
      );
    },
  },
});
</script>
