import { defineComponent } from "vue";

export default defineComponent({
  methods: {
    getSum(numbers: string[]) {
      return numbers.map((x) => parseInt(x)).reduce((x, y) => x + y, 0);
    },
    isDa(numbers: string[]) {
      let sum = this.getSum(numbers);
      return sum >= 23;
    },
    isShuang(numbers: string[]) {
      let sum = this.getSum(numbers);
      return sum % 2 === 0;
    },
    isLongHuHuo(numbers: string[]):number {
      const oneNumber = parseInt(numbers[0]);
      const fiveNumber = parseInt(numbers[4]);      
      if (oneNumber === fiveNumber) return 3;
      return oneNumber < fiveNumber ? 2 : 1;
    },
    getDaXiaoText(numbers: string[]) {
      return this.isDa(numbers) ? "大" : "小";
    },
    getDanShuang(numbers: string[]) {
      return this.isShuang(numbers) ? "双" : "单";
    },
    getLongHuHuo(numbers: string[]):string {
      const result = this.isLongHuHuo(numbers);
      let mappings = {
        1: '龙',
        2: '虎',
        3: '和'
      } as { [key: number]: string };

      return mappings[result] || '';
    },
    getDaXiaoBackgroundClassName(numbers: string[]) {
      return this.isDa(numbers) ? "bg-cyan" : "bg-orange";
    },
    getDanShuangBackgroundClassName(numbers: string[]) {
      return this.isShuang(numbers) ? "bg-orange" : "bg-cyan";
    }
  },
});
