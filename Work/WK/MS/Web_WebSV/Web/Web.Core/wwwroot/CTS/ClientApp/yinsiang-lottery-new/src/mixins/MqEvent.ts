import { defineComponent } from "vue";
import { MutationType } from "@/store";
import { event as eventModel } from "@/models";
import event from "@/event";
import { Roulette } from "@/GameRules/RouletteRule";

export default defineComponent({
  methods: {
    onLotteryDraw(arg: eventModel.LotteryDrawArg) {
      if (arg.LotteryID !== this.$store.state.lotteryInfo.lotteryId) return;
      if (arg.IssueNo !== this.$store.state.issueNo.lastIssueNo) return;

      this.$store.commit(MutationType.DrawIssueNo, arg.CurrentLotteryNum);
    },
    rouletteHandle(arg: eventModel.LotteryDrawArg) {
      if (arg.LotteryID !== 71) return;
      if (arg.IssueNo !== this.$store.state.issueNo.lastIssueNo) return;

      const roulette = new Roulette();
      const lotteryNumber = Number(arg.CurrentLotteryNum) | 0;
      const newRouletteIndex =
        roulette.rouletteNumberArray.indexOf(lotteryNumber);
      this.$store.commit(MutationType.SetRouletteIndex, newRouletteIndex);
    },
  },
  created() {
    event.on("lotteryDraw", this.onLotteryDraw);
  },
  beforeUnmount() {
    event.off("lotteryDraw", this.onLotteryDraw);
  },
});
