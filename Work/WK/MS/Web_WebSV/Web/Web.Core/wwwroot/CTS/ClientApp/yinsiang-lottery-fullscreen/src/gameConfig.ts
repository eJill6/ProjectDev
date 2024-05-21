import { OrderStatus } from "@/enums";
import { LotteryMenuInfo, OptionItemModel } from "@/models";

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
  长龙: "ChangLong",
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
  三公: "SanGong"
};

const basePlayTypeId: number[] = [17, 19, 34, 42, 1];

const basePlayTypeRadioId: number[] = [26, 62, 75, 173, 1];

const shengXiaoContext: string = "特码生肖";

const changLongContext: string = "长龙";

const followOrderTypeList: {
  [key: string]: string[];
} = {
  K3: ["总和大小2期", "总和单双2期"],
  PK10: ["冠军大小2期", "冠军单双2期"],
  SSC: ["总和大小2期", "总和单双2期", "龙虎2期"],
  LHC: ["特码大小2期", "特码单双2期", "特码生肖2期", "特码色波2期"],
  NuiNui: ["胜负蓝红2期", "龙虎2期"],
  Baccarat: ["庄闲2期"],
  LP: ["大小2期", "单双2期", "红黑2期"],
  YXX: ["总合大小2期", "总合单双2期"], 
  SG: ["庄闲2期"], 
};

const orderStatusTypeList: OptionItemModel[] = [
  { text: "全部状态", value: "" },
  { text: "待开奖", value: OrderStatus.Unawarded },
  { text: "已中奖", value: OrderStatus.Won },
  { text: "未中奖", value: OrderStatus.Lost },
  // { text: "和局", value: OrderStatus.SystemCancel },
  { text: "系统撤单", value: OrderStatus.SystemRefund },
];

const ua = navigator.userAgent;
const isAndroid = ua.indexOf("Android") > -1 || ua.indexOf("Adr") > -1; // android
const isIOS = !!ua.match(/\(i[^;]+;( U;)? CPU.+Mac OS X/); // ios

const thirtySeccondsInLotteryID = [70, 71, 72];

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
  isMaintaining: false,
};

export {
  correspondContext,
  basePlayTypeId,
  basePlayTypeRadioId,
  shengXiaoContext,
  changLongContext,
  followOrderTypeList,
  orderStatusTypeList,
  defaultLotteryInfo,
  isAndroid,
  isIOS,
  thirtySeccondsInLotteryID,
};
