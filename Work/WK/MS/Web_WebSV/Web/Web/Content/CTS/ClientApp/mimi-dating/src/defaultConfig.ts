import { ChinaCityInfo, OptionItemModel } from "@/models";
import { PostLockStatus, PostSortType } from "./enums";

const decryptoKey = "94a4b778g01ca4ab";

const defaultCityArea: ChinaCityInfo[] = [
  {
    code: "00",
    name: "全国",
    province: "00",
  },
  {
    code: "310000",
    name: "上海市",
    province: "31",
  },
  {
    code: "440100",
    name: "广州市",
    province: "44",
  },
  {
    code: "440300",
    name: "深圳市",
    province: "44",
  },
  {
    code: "510100",
    name: "成都市",
    province: "51",
  },
  {
    code: "420100",
    name: "武汉市",
    province: "42",
  },
];

const defaultSort: OptionItemModel[] = [
  {
    key: PostSortType.New,
    value: "最新上传",
  },
  {
    key: PostSortType.Hot,
    value: "热门排行",
  },
];

const defaultStatus: OptionItemModel[] = [
  {
    key: -1,
    value: "全部",
  },
  {
    key: PostLockStatus.Unlock,
    value: "已解锁",
  },
  {
    key: PostLockStatus.Lock,
    value: "未解锁",
  },
];

export {
  decryptoKey,
  defaultCityArea,
  defaultSort,
  defaultStatus,
};
