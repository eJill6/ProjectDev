<template>
  <component :is="issueHistoryComponentName" :list="list"></component>
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
    onLoadData(arg: eventModel.LotteryDrawArg) {
      this.loadData(arg);
    },
    async loadData(arg?: eventModel.LotteryDrawArg) {
      const hasNewIssue = !!arg;
      if (this.isLoading) return;

      try {
        this.$store.commit(MutationType.SetIsLoading, true);
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
            this.list.pop();
          }
        } else {
          const result = await api.getIssueHistoryAsync(
            this.lotteryInfo.lotteryId,
            8,
            ""
          );
          this.list = result.list;
        }
      } catch (error) {
        console.error(error);
      } finally {
        this.$store.commit(MutationType.SetIsLoading, false);
      }
    },
  },
  async created() {
    await this.loadData();
  },
  computed: {
    isLoading() {
      return this.$store.state.isLoading;
    },
    lotteryInfo() {
      return this.$store.state.lotteryInfo;
    },
    issueHistoryComponentName(): string {
      return `${this.lotteryInfo.gameTypeName}_SM`;
    },
  },
});
</script>
