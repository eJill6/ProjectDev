import { Roulette, RouletteColorType } from "@/GameRules/RouletteRule";
import { defineComponent } from "vue";

export default defineComponent({
  data() {
    return {
      roulette: new Roulette(),
    };
  },
  methods: {
    getSum(numbers: string[]) {
      return numbers.map((x) => parseInt(x)).reduce((x, y) => x + y, 0);
    },
    isDa(numbers: string[]) {
      let sum = this.getSum(numbers);
      return sum >= 19 && sum <= 36;
    },
    isShuang(numbers: string[]) {
      let sum = this.getSum(numbers);
      return sum % 2 === 0;
    },
    isBlack(number: Number) {
      return this.roulette.blackNumbers.indexOf(number as number);
    },
    getNumberColorName(numbers: string[]) {
      let sum = this.getSum(numbers);
      if (!sum) {
        return "0";
      }
      return this.isBlack(sum) > -1 ? "黑" : "红";
    },
    getDaXiaoText(numbers: string[]) {
      return this.isDa(numbers) ? "大" : "小";
    },
    getDanShuang(numbers: string[]) {
      return this.isShuang(numbers) ? "双" : "单";
    },
    getDaXiaoBackgroundClassName(numbers: string[]) {
      return this.isDa(numbers) ? "orange" : "blue";
    },
    getDanShuangBackgroundClassName(numbers: string[]) {
      return this.isShuang(numbers) ? "blue" : "orange";
    },
    getNumberBackgroundClassName(numbers: string[]): string {
      const value = this.getSum(numbers);
      const colorEnum = this.roulette.getColor(value);

      if (colorEnum === RouletteColorType.green) {
        return "zero";
      }
      return colorEnum;
    },
  },
});
