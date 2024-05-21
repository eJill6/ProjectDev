import { defineComponent } from "vue";

type PlayItemAttribute = {
    name: string;
    cname: string;
};

const YXXMappingInfo: { [id: number]: PlayItemAttribute } = {
    1: { name: "fish", cname: "鱼" },
    2: { name: "shrimp", cname: "虾" },
    3: { name: "gourd", cname: "葫芦" },
    4: { name: "coppercoins", cname: "铜钱" },
    5: { name: "crab", cname: "蟹" },
    6: { name: "chicken", cname: "鸡" },
};

export default defineComponent({
    methods: {
        getAliasName(num: number) {
            return YXXMappingInfo[num].name;
        },
        convertCNameToNumber(cname: string) {
            return Object.keys(YXXMappingInfo).find(
                (key: any) => YXXMappingInfo[key].cname === cname
            );
        },
        getSum(numbers: string[]) {
            return numbers.map(x => parseInt(x)).reduce((x, y) => x + y, 0);
        },
        isDa(numbers: string[]) {
            let sum = this.getSum(numbers);
            return sum >= 11 && sum <= 17;
        },
        isShuang(numbers: string[]) {
            let sum = this.getSum(numbers);
            return sum % 2 === 0;
        },
        isBaozi(numbers: string[]) {
            if (numbers.length > 0) {
                return !numbers.some((val) => {
                    return val !== numbers[0];
                });
            }
            return false;
        },
        getDaXiaoText(numbers: string[]) {
            return this.isDa(numbers) ? '大' : '小';
        },
        getDanShuang(numbers: string[]) {
            return this.isShuang(numbers) ? '双' : '单';
        },
        getDaXiaoBackgroundClassName(numbers: string[]) {
            return this.isDa(numbers) ? 'orange' : 'blue';
        },
        getDanShuangBackgroundClassName(numbers: string[]) {
            return this.isShuang(numbers) ? 'blue' : 'orange';
        }
    }
});