import { LotteryMenuInfo, OptionItemModel } from "@/models";
import { OrderStatus } from "./enums";
const correspondContext: { [key: string]: string } = {
  豹子: "BaoZi",
  单骰: "DanTou",
  对子: "DuiZi",
  两面: "LiangMian",
  总和: "ZongHe",
  冠军与两面: "KuaiXuanGuanJunYuLiangMian",
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
  庄闲: "ZhuangXian",
  红黑: "HongHei",
  直注: "ZhiZhu",
  组合: "ZuHe",
  大小: "DaXiao",
  围骰: "WeiTou",
  指定三色: "ZhiDingSanSe",
};

const basePlayTypeId: number[] = [17, 19, 34, 42, 1];

const basePlayTypeRadioId: number[] = [26, 62, 75, 173, 1];

const shengXiaoContext: string = "特码生肖";

const orderStatusTypeList: OptionItemModel[] = [
  { text: "全部状态", value: "" },
  { text: "待开奖", value: OrderStatus.Unawarded },
  { text: "已中奖", value: OrderStatus.Won },
  { text: "未中奖", value: OrderStatus.Lost },
  // { text: "和局", value: OrderStatus.SystemCancel },
  { text: "系统撤单", value: OrderStatus.SystemRefund },
];

const defaultLotteryInfo: LotteryMenuInfo = {
  gameTypeID: 0,
  groupPriority: 0,
  hotNew: 0,
  lotteryID: 0,
  lotteryType: "全部彩种",
  maxBonusMoney: 0,
  notice: "",
  numberTrendUrl: "",
  officialLotteryUrl: "",
  priority: 0,
  typeURL: "",
  userType: 0,
};

const followOrderTypeList: {
  [key: string]: string[];
} = {
  Baccarat: ["庄闲2期"],
  LP: ["大小2期", "单双2期", "红黑2期"],
  YXX: ["总合大小2期", "总合单双2期"]
};

export {
  correspondContext,
  basePlayTypeId,
  basePlayTypeRadioId,
  shengXiaoContext,
  followOrderTypeList,
  defaultLotteryInfo,
  orderStatusTypeList,
};
