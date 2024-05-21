import { defineComponent } from "vue";
import RebatePro from "./RebatePro";
import {shengXiaoContext} from "@/gameConfig";

const sumColorClassNameMappings = [
  {
    index: 1,
    className: "red",
    text: "红",
    numbers: [
      "01",
      "1",
      "02",
      "2",
      "07",
      "7",
      "08",
      "8",
      "12",
      "13",
      "18",
      "19",
      "23",
      "24",
      "29",
      "30",
      "34",
      "35",
      "40",
      "45",
      "46",
    ],
  },
  {
    index: 2,
    className: "blue",
    text: "蓝",
    numbers: [
      "03",
      "3",
      "04",
      "4",
      "09",
      "9",
      "10",
      "14",
      "15",
      "20",
      "25",
      "26",
      "31",
      "36",
      "37",
      "41",
      "42",
      "47",
      "48",
    ],
  },
  {
    index: 3,
    className: "green",
    text: "绿",
    numbers: [
      "05",
      "5",
      "06",
      "6",
      "11",
      "16",
      "17",
      "21",
      "22",
      "27",
      "28",
      "32",
      "33",
      "38",
      "39",
      "43",
      "44",
      "49",
    ],
  },
];

const shengXiaoIndexMappings = [
  { index: 0, numbers: ["1", "01", "13", "25", "37", "49"] },
  { index: 11, numbers: ["2", "02", "14", "26", "38"] },
  { index: 10, numbers: ["3", "03", "15", "27", "39"] },
  { index: 9, numbers: ["4", "04", "16", "28", "40"] },
  { index: 8, numbers: ["5", "05", "17", "29", "41"] },
  { index: 7, numbers: ["6", "06", "18", "30", "42"] },
  { index: 6, numbers: ["7", "07", "19", "31", "43"] },
  { index: 5, numbers: ["8", "08", "20", "32", "44"] },
  { index: 4, numbers: ["9", "09", "21", "33", "45"] },
  { index: 3, numbers: ["10", "22", "34", "46"] },
  { index: 2, numbers: ["11", "23", "35", "47"] },
  { index: 1, numbers: ["12", "24", "36", "48"] },
];

export default defineComponent({
  mixins: [RebatePro],
  methods: {
    isDa(number: string) {
      return parseInt(number) >= 25;
    },
    isShuang(number: string) {
      return parseInt(number) % 2 === 0;
    },
    getDaXiaoText(number: string) {
      return this.isDa(number) ? "大" : "小";
    },
    getDanShuang(number: string) {
      return this.isShuang(number) ? "双" : "单";
    },
    getDaXiaoBackgroundClassName(number: string) {
      return this.isDa(number) ? "bg-cyan" : "bg-orange";
    },
    getDanShuangBackgroundClassName(number: string) {
      return this.isShuang(number) ? "bg-orange" : "bg-cyan";
    },
    getSeBoBackgroundClassName(number: string) {
      let mapping = sumColorClassNameMappings.filter(
        (x) => x.numbers.indexOf(number) > -1
      )[0];
      
      const className = (mapping && mapping.className) || "";
      return className ? `lottery_ball_marksix_${className}` : "";
    },
    getColorBackgroundClassName(number: string) {
      let mapping = sumColorClassNameMappings.filter(
        (x) => x.numbers.indexOf(number) > -1
      )[0];
      
      const className = (mapping && mapping.className) || "";
      return className ? `bg-${className}` : "";
    },
    getSeBoText(number: string) {
      let mapping = sumColorClassNameMappings.filter(
        (x) => x.numbers.indexOf(number) > -1
      )[0];
      
      return (mapping && mapping.text) || "";
    },
    getShengXiao(number: string) {      
      const mapping = shengXiaoIndexMappings.filter(x => x.numbers.indexOf(number) > -1)[0];    
      const newArray = this.playConfig[shengXiaoContext].reduce((a, b) => {
        return a.concat(b);
      });
      return (mapping && newArray[mapping.index]) || "";
    },
    getShengXiaoBackgroundClassName(number: string) {
      const mapping = shengXiaoIndexMappings.filter(x => x.numbers.indexOf(number) > -1)[0];
      return (mapping && mapping.index) || "";
    },
  },
  computed:{
    playConfig() { 
      return this.$store.getters.playConfig;
    },
  }
});
