import { DefaultBookingStatusType, IdentityType, OfficialPostSortType,OfficialPostStatus } from "@/enums";
import { PriceLowAndHighModel } from "./PriceLowAndHighModel";

export interface OfficialSearchModel {
  /// 查詢頁數
  pageNo?: Number | undefined;

  /// 查詢頁數
  pageSize?: Number | undefined;

  /// 是否為推薦
  isRecommend?: Boolean | undefined;

  /// 帖子排序類型。0：最新、1：紅榜、2：顏值
  sortType?: OfficialPostSortType | undefined;

  /// 解鎖狀態。0：全部、1：預約過、2：未約過
  bookingStatus?: DefaultBookingStatusType | undefined;

  /// 用戶身份。0：一般、1：覓經紀、2：覓女郎、3、覓老闆、4：星覓官  -- (官方帖專用)
  userIdentity?: IdentityType | undefined;

  /// 地區代碼
  areaCode: string;

  /// 篩選的年齡
  age: number[];

  /// 篩選的年齡
  height: number[];

  /// 篩選的年齡
  cup: number[];

  /// 價格設定
  price: PriceLowAndHighModel[];

  /// 篩選的服務項目
  serviceIds: Number[];

  /// 查詢時的郵戳
  ts: string;
  /// 状态
  status:OfficialPostStatus;
  /// 是否上架 如果为-1 条件为空
  isDelete:number;

}
