import { defineComponent } from "vue";
import { GameType } from "@/enums";

const LongHu = "龙虎";

export default defineComponent({
  methods: {
    getNumberOdds(number: string) {
      const playName = this.currentPlayGame.playName as string;
      return (
        (this.currentRebatePro && this.currentRebatePro[playName][number]) || ""
      );
    },
    getNumberOddsByPlayName(playName: string, number: string) {
      return (
        (this.currentRebatePro && this.currentRebatePro[playName][number]) || ""
      );
    },
    getChangLongOdds(
      gameTypeId: number,
      lotteryId: number,
      betType: string,
      betContent: string
    ) {
      let radioOdds = this.currentAllRebatePro;
      if (!radioOdds || !Object.keys(radioOdds).length) return 0;
      
      let betItemOdds = radioOdds[lotteryId].find((x) => {
        return x.gameTypeId === gameTypeId && x.lotteryId === lotteryId;
      });

      if (!betItemOdds) return 0;

      let prefixPlayName = this.getChangLongPrefixName(gameTypeId, betType);
      return betItemOdds.numberOdds[`${prefixPlayName} ${betContent}`];
    },
    getChangLongPrefixName(gameTypeId: number, betType: string) {
      let prefixPlayName = betType;
      switch (gameTypeId) {
        case GameType.SSC:
          prefixPlayName = betType === LongHu ? "龙虎球1VS球5" : "总和";
          break;
        case GameType.K3:
          prefixPlayName = "两面";
          break;
        case GameType.PK10:
          prefixPlayName = "冠亚和两面";
          break;
        case GameType.LHC:
          prefixPlayName = "特码两面";
          break;
        case GameType.NuiNui:
          prefixPlayName = betType === LongHu ? "龙虎牌1VS牌5" : "胜负";
          break;
        case GameType.LP:
          prefixPlayName = "红黑";
          break;
        case GameType.YXX:
          prefixPlayName = "大小";
          break;
      }
      return prefixPlayName;
    },
    getBetOdds(lotteryId: number, betType: string, betContent: string) {
      let radioOdds = this.currentAllRebatePro;
      if (!radioOdds) return 0;

      let betItemOdds = radioOdds[lotteryId].find((x) => {
        return x.lotteryId === lotteryId;
      });

      if (!betItemOdds) return 0;

      return betItemOdds.numberOdds[`${betType} ${betContent}`] ?? 0;
    },
  },
  computed: {
    currentRebatePro() {
      const info = this.$store.getters.currentRebatePro;
      if (!info) return null;

      let numberOddType: { [key: string]: { [key: string]: string } } = {};
      for (const numberOdd in info?.numberOdds) {
        const splitNumberOdd = numberOdd.split(" ");
        const gameKind = splitNumberOdd[0];
        const gameType = splitNumberOdd[1];
        const value = info?.numberOdds[numberOdd];
        if (!numberOddType[gameKind]) {
          numberOddType[gameKind] = {} as { [key: string]: string };
        }
        numberOddType[gameKind][gameType] = value.toString();
      }
      return numberOddType;
    },
    currentPlayGame() {
      return this.$store.getters.playTypeSelected;
    },
    currentAllRebatePro() {
      return this.$store.getters.allRebatePros;
    },
  },
});
