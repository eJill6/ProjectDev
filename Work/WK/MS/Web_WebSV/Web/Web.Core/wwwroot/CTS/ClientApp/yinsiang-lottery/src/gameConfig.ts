import { LotteryMenuInfo } from "@/models";
const correspondContext: { [key: string]: string } = {
  豹子: "BaoZi",
  单骰: "DanTou",
  对子: "DuiZi",
  两面: "LiangMian",
  总和: "ZongHe",
  冠亚和两面: "KuaiXuanGuanJunYuLiangMian",
  冠亚和: "KuaiXuanGuanYaHe",
  冠军特殊: "KuaiXuanGuanJunTeShu",
  冠军: "KuaiXuanGuanJun",
  龙虎球1VS球5: "KuaiXuanLongHuQiu1VsQiu5",
  特码两面: "KuaiXuanTaMaLiangMian",
  特码: "KuaiXuanTaMa",
  特码生肖: "KuaiXuanTaMaShengXiao",
  特码色波: "KuaiXuanTaMaSeBo",
  胜负: "ShengFu",
  龙虎牌1VS牌5: "LongHuPai1VSPai5",
  蓝方牛: "LanFangNiu",
  红方牛: "HongFangNiu",
};

const basePlayTypeId: number[] = [17, 19, 34, 42, 1];

const basePlayTypeRadioId: number[] = [26, 62, 75, 173, 1];

const shengXiaoContext: string = "特码生肖";

const bigWinNumberList = ["NuiNui"];

const followOrderTypeList: {
  [key: string]: string[];
} = {
  K3: ["总和大小2期", "总和单双2期"],
  PK10: ["冠军大小2期", "冠军单双2期"],
  SSC: ["总和大小2期", "总和单双2期", "龙虎2期"],
  LHC: ["特码大小2期", "特码单双2期", "特码生肖2期", "特码色波2期"],
  NuiNui: ["胜负蓝红2期", "龙虎2期"],
};

const defaultLotteryInfo: LotteryMenuInfo = {
  gameTypeID: 0,
  groupPriority: 0,
  hotNew: 0,
  lotteryID: 0,
  lotteryType: "",
  maxBonusMoney: 0,
  notice: "",
  numberTrendUrl: "",
  officialLotteryUrl: "",
  priority: 0,
  typeURL: "",
  userType: 0,
};

export {
  correspondContext,
  basePlayTypeId,
  basePlayTypeRadioId,
  shengXiaoContext,
  followOrderTypeList,
  defaultLotteryInfo,
  bigWinNumberList,
};
