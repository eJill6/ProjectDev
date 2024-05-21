<template>
  <div class="history_row mb">
    <PlanNumbers :item="itemInfo"></PlanNumbers>
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import { BaccaratIssueModel, event as eventModel, IssueNo } from "@/models";
import AssetImage from "@/components/shared/AssetImage.vue";
import { Baccarat, JSBaccaratPoker } from "@/GameRules/JSBaccaratRule";
import { CardSuit, PokerCard } from "@/GameRules/BasePokerRule";
import { MqEvent } from "@/mixins";
import { MutationType } from "@/store";
import PlanNumbers from "../PlanNumbers";

export default defineComponent({
  mixins: [MqEvent],
  components: { AssetImage, PlanNumbers },
  data() {
    return {
      itemInfo: {} as BaccaratIssueModel,
    };
  },
  methods: {
    onLotteryDraw(arg: eventModel.LotteryDrawArg) {
      if (arg.LotteryID !== this.$store.state.lotteryInfo.lotteryId) return;
      if (arg.IssueNo !== this.$store.state.issueNo.lastIssueNo) return;

      this.$store.commit(MutationType.DrawIssueNo, arg.CurrentLotteryNum);
      this.gameResult();
    },
    getDiceClassName: (item: PokerCard) => {
      if (item.originalNumber === "0") {
        return "poker_back";
      }
      return `${CardSuit[item.suit]}_${item.type.toUpperCase()}`;
    },
    gameResult() {
      if (this.drawNumber.length > 0) {
        let baccarat = new Baccarat();
        let baccaratResult = baccarat.confirmResult(
          this.drawNumber as string[]
        );

        this.itemInfo.issueNo = this.issueNo.lastIssueNo;
        this.itemInfo.player = baccaratResult[0];
        this.itemInfo.banker = baccaratResult[1];
        // 處理bind data-text殘影問題
        let tmpPlayerIsWin = this.itemInfo.player.isWin;
        let tmpBankerIsWin = this.itemInfo.banker.isWin;
        this.itemInfo.player.isWin = false;
        this.itemInfo.banker.isWin = false;
        window.setTimeout(() => {
          this.itemInfo.player.isWin = tmpPlayerIsWin;
          this.itemInfo.banker.isWin = tmpBankerIsWin;
        }, 10);
        //--
      }
    },
  },
  watch: {
    issueNo: {
      handler(value) {
        this.gameResult();
      },
      deep: true,
    },
  },
  created() {
    this.itemInfo = {
      issueNo: "",
      player: { cards: [] } as unknown as JSBaccaratPoker,
      banker: { cards: [] } as unknown as JSBaccaratPoker,
    };
    for (let i = 0; i < 3; i++) {
      this.itemInfo.player.cards[i] = { originalNumber: "0" } as PokerCard;
      this.itemInfo.banker.cards[i] = { originalNumber: "0" } as PokerCard;
    }
    this.gameResult();
  },
  computed: {
    issueNo(): IssueNo {
      return this.$store.state.issueNo;
    },
    drawNumber() {
      return (
        (this.issueNo.lastDrawNumber &&
          this.issueNo.lastDrawNumber.split(",")) ||
        []
      );
    },
  },
});
</script>
