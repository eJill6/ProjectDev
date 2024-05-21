import { ChinaCityInfo, OptionItemModel } from "@/models";
import {
  DefaultBookingStatusType,
  IdentityType,
  OfficialPostSortType,
  PostLockStatus,
  PostModeType,
  PostSortType,
} from "@/enums";

const decryptoKey = "94a4b778g01ca4ab";

const hlsKey =
  "c6e3c761d279440fb8004e9e92b88b3f01d635619e4a4a64abbd0e98af1337d5";

const defaultCityArea: ChinaCityInfo[] = [
  {
    code: "00",
    name: "全国",
    province: "00",
  },
  {
    code: "310000",
    name: "上海",
    province: "31",
  },
  {
    code: "440100",
    name: "广州",
    province: "44",
  },
  {
    code: "440300",
    name: "深圳",
    province: "44",
  },
  {
    code: "510100",
    name: "成都",
    province: "51",
  },
  {
    code: "420100",
    name: "武汉",
    province: "42",
  },
];

const defaultSort: OptionItemModel[] = [
  {
    key: PostSortType.New,
    value: "最新更新",
  },
  {
    key: PostSortType.View,
    value: "热门排行",
  },
  {
    key: PostSortType.PriceAsc,
    value: "价格最低",
  },
];

const defaultStatus: OptionItemModel[] = [
  {
    key: PostLockStatus.Lock,
    value: "未解锁",
  },
  {
    key: PostLockStatus.Unlock,
    value: "已解锁",
  },
  {
    key: PostLockStatus.VideoCertified,
    value: "视频认证",
  },
];

const officialDefaultMessage: OptionItemModel[] = [
  {
    key: IdentityType.General,
    value: "全部",
  },
  {
    key: IdentityType.Boss,
    value: "觅老板",
  },
  // {
  //   key: IdentityType.Girl,
  //   value: "觅女郎",
  // },
];

const officialComboKeyValue: OptionItemModel[] = [
  {
    key: 0,
    value: "一",
  },
  {
    key: 1,
    value: "二",
  },
  {
    key: 2,
    value: "三",
  },
];

const postMode: OptionItemModel[] = [
  // {
  //   key: PostModeType.swiper,
  //   value: "滑动模式",
  // },
  {
    key: PostModeType.doubleRow,
    value: "双列模式",
  },
  {
    key: PostModeType.little,
    value: "小图模式",
  }
];

const officialDefaultSort: OptionItemModel[] = [
  {
    key: OfficialPostSortType.New,
    value: "最新",
  },
  {
    key: OfficialPostSortType.Hot,
    value: "红榜",
  },
  {
    key: OfficialPostSortType.OrderQuantity,
    value: "销量",
  },
];

const officialDefaultStatus: OptionItemModel[] = [
  {
    key: DefaultBookingStatusType.All,
    value: "全部",
  },
  {
    key: DefaultBookingStatusType.MadeAnAppointment,
    value: "预约过",
  },
  {
    key: DefaultBookingStatusType.NoAppointment,
    value: "未预约",
  },
];

export {
  decryptoKey,
  hlsKey,
  defaultCityArea,
  postMode,
  defaultSort,
  defaultStatus,
  officialDefaultMessage,
  officialDefaultSort,
  officialDefaultStatus,
  officialComboKeyValue,
};
