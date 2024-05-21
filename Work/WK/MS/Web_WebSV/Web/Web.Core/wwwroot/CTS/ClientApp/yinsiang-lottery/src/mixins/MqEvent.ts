import { defineComponent } from "vue";
import { MutationType } from "@/store";
import { event as eventModel } from "@/models";
import event from "@/event";

export default defineComponent({
  methods: {
    onLotteryDraw(arg: eventModel.LotteryDrawArg) {
      if (arg.LotteryID !== this.$store.state.lotteryInfo.lotteryId) return;
      if (arg.IssueNo !== this.$store.state.issueNo.lastIssueNo) return;
          
      this.$store.commit(MutationType.DrawIssueNo, arg.CurrentLotteryNum);
    },
  },
  created() {
    event.on("lotteryDraw", this.onLotteryDraw);
  },
  beforeUnmount() {
    event.off("lotteryDraw", this.onLotteryDraw);
  },
});
