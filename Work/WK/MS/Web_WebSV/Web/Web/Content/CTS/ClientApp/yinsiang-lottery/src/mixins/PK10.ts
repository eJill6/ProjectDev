import { defineComponent } from "vue";

export default defineComponent({
    methods: {
        getSum(numbers: string[]) {
            return numbers.map(x => parseInt(x)).slice(0,2).reduce((x, y) => x + y, 0);
        },
        isDa(numbers: string[]) {
            let sum = this.getSum(numbers);
            return sum >= 12;
        },
        isShuang(numbers: string[]) {
            let sum = this.getSum(numbers);
            return sum % 2 === 0;
        },
        getDaXiaoText(numbers: string[]) {
            return this.isDa(numbers) ? '大' : '小';
        },
        getDanShuang(numbers: string[]) {
            return this.isShuang(numbers) ? '双' : '单';
        },
        getDaXiaoBackgroundClassName(numbers: string[]) {
            return this.isDa(numbers) ? 'bg-cyan' : 'bg-orange';
        },
        getDanShuangBackgroundClassName(numbers: string[]) {
            return this.isShuang(numbers) ? 'bg-orange' : 'bg-cyan';
        }
        
    }
});