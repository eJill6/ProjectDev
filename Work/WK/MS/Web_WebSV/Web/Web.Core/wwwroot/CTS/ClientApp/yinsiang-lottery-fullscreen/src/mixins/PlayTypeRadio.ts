import { defineComponent } from "vue";
import { BetInfo, BetsLogDetail, PlayTypeRadioInfo, SelectedModel } from "@/models";
import { MutationType } from "@/store";
import RebatePro from "./RebatePro";

export default defineComponent({
  mixins: [RebatePro],
  data() {
    return {};
  },
  watch: {
    selectedNumbers: {
      handler() {
        this.convertSelectedNumbers();
      },
      deep: true,
    },
  },
  methods: {
    isNumberSelected(fieldIndex: number, numberIndex: number) {
      const playName = this.gameSelected.playName as string;
      return !!this.selectedNumbers[playName][fieldIndex][numberIndex];
    },
    showTotalAmount(value: string): number {
      const playName = this.gameSelected.playName as string;
      const betsLog = this.currentNumberOdds.find(
        (x) => x.category === playName
      );
      if (betsLog) {
        const detail = betsLog.values.find((x) => x.value === value);
        return detail ? Number(detail.totalAmount) : 0;
      }
      return 0;
    },
    addSelectedNumber(fieldIndex: number, numberIndex: number) {
      const playName = this.gameSelected.playName as string;
      let number = this.currentPlayConfig[playName][fieldIndex][numberIndex];
      this.selectedNumbers[playName][fieldIndex].splice(numberIndex, 1, number);
    },
    removeSelectedNumber(fieldIndex: number, numberIndex: number) {
      const playName = this.gameSelected.playName as string;
      this.selectedNumbers[playName][fieldIndex].splice(numberIndex, 1, "");
    },
    toggleSelectNumber(fieldIndex: number, numberIndex: number) {
      //與長龍互斥，因此投注非長龍時，就清掉長龍選擇
      this.$store.commit(MutationType.SetChangLongNumbers, null);

      if (this.isNumberSelected(fieldIndex, numberIndex))
        this.removeSelectedNumber(fieldIndex, numberIndex);
      else this.addSelectedNumber(fieldIndex, numberIndex);
    },
    convertSelectedNumbers() {
      const results = [] as BetInfo[];
      let numberId: number = 0;

      if (
        JSON.stringify(this.selectedChangLongNumbers) !== JSON.stringify({})
      ) {
        let changlong = this.selectedChangLongNumbers;

        let currentIssueInfo = this.allLotteryIssueNo?.find((x) => {
          return x.lotteryId === changlong.lotteryId;
        });

        let changlongBet: BetInfo[] = changlong.content.map((item) => ({
          id: (numberId++).toString(),
          lotteryId: changlong.lotteryId,
          lotteryTypeName: changlong.lotteryTypeName,
          playTypeRadioName: this.getChangLongPrefixName(
            changlong.gameTypeId,
            changlong.type
          ),
          selectedBetNumber: item,
          currentIssueNo: currentIssueInfo?.currentIssueNo,
          lotteryTime: currentIssueInfo?.currentTime,
          odds: this.getChangLongOdds(
            changlong.gameTypeId,
            changlong.lotteryId,
            changlong.type,
            item
          ).toString(),
          gameTypeName: changlong.gameTypeName,
          gameTypeId: changlong.gameTypeId,
        }));

        results.push(...changlongBet);
      } else {
        //續投以lotteryId存在為依據
        let keepBet = this.$store.state.currnetBetInfo?.filter(
          (x) => !!x.lotteryId
        );
        let hasCurrentBet = Object.values(this.selectedNumbers)
          .flatMap((x) => x)
          .flatMap((x) => x)
          .some((x) => x.trim().length > 0);

        if (keepBet?.length > 0 && !hasCurrentBet) {
          results.push(...keepBet);
        } else {
          for (let gameType in this.currentPlayConfig) {
            let selectedNumbers = this.selectedNumbers[gameType]?.map(
              (numbers) => numbers.filter((n) => !!n)
            ); //filter:去掉null empty '' undefined;

            if (selectedNumbers) {
              let betInfos: BetInfo[] = selectedNumbers
                .flatMap((x) => x)
                .map((x, i) => ({
                  id: (numberId++).toString(),
                  playTypeRadioName: gameType,
                  selectedBetNumber: x,
                  odds: this.getNumberOddsByPlayName(gameType, x),
                }));
              results.push(...betInfos);
            }
          }
        }
      }

      this.$store.commit(MutationType.SetCurrentBetInfo, results);
    },
    isChangLongNumberSelected(
      lotteryId: number,
      betType: string,
      betContent: string
    ) {
      return (
        this.selectedChangLongNumbers?.lotteryId === lotteryId &&
        this.selectedChangLongNumbers.type === betType &&
        this.selectedChangLongNumbers?.content.indexOf(betContent) > -1
      );
    },
  },
  computed: {
    currentPlayTypeRadio(): PlayTypeRadioInfo {
      return this.$store.getters.currentPlayTypeRadio as PlayTypeRadioInfo;
    },
    playTypeRadio(): Array<Array<string>> {
      const playConfig = this.currentPlayConfig;
      const playTypeSelected = this.gameSelected;
      const gameType = playTypeSelected.playName as string;
      return playConfig[gameType];
    },
    currentPlayConfig() {
      return this.$store.getters.playConfig;
    },
    gameSelected() {
      return this.$store.getters.playTypeSelected;
    },
    selectedNumbers() {
      return this.$store.getters.selectedNumbers;
    },
    selectedChangLongNumbers() {
      return this.$store.getters.selectedChangLongNumbers;
    },
    allLotteryIssueNo() {
      return this.$store.getters.allLotteryIssueNo;
    },
    currentNumberOdds() {
      return this.$store.state.betsLog.length
        ? this.$store.state.betsLog[0].numberOdds
        : ([] as BetsLogDetail[]);
    },
  },
});
