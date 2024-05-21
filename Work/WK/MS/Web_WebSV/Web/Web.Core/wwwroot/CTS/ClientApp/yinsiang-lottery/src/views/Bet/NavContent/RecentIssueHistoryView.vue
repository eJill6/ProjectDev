<template>
  <div class="h-history-list overflow-scroll-y no-scrollbar">
    <component :is="issueHistoryComponentName" :list="list"></component>
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import { IssueHistorys } from "@/components";
import { MutationType } from "@/store";
import { IssueHistory, event as eventModel } from "@/models";
import api from "@/api";
import { MqEvent } from "@/mixins";

export default defineComponent({
  components: IssueHistorys,
  mixins: [MqEvent],
  data() {
    return {
      list: [] as IssueHistory[],
    };
  },
  methods: {
    onLotteryDraw(arg: eventModel.LotteryDrawArg) {
      if (arg.LotteryID !== this.$store.state.lotteryInfo.lotteryId) return;
      if (arg.IssueNo !== this.$store.state.issueNo.lastIssueNo) return;

      this.$store.commit(MutationType.DrawIssueNo, arg.CurrentLotteryNum);
      this.getListAsync();
    },
    async getListAsync() {
      if (this.isLoading) return;

      try {
        this.$store.commit(MutationType.SetIsLoading, true);
        var result = await api.getIssueHistoryAsync(
          this.lotteryInfo.lotteryId,
          8,
          ""
        );
        this.list = result.list;
      } catch (error) {
        console.error(error);
      } finally {
        this.$store.commit(MutationType.SetIsLoading, false);
      }
    },
  },
  async created() {
    await this.getListAsync();
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
  },
});
</script>
