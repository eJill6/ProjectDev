import { OfficialPostSortType } from "@/enums";

export interface OfficialShopSearchModel {
  /// 查詢頁數
  pageNo?: number | undefined;

  /// 查詢頁數
  pageSize?: number | undefined;

  /// 搜寻关键字，店铺名称或帖子名称
  keyWords?: string | undefined;

  /// 帖子排序類型。0：最新、1：紅榜、2：顏值、 3：销量
  sortType?: OfficialPostSortType | undefined;

  /// 查詢時的郵戳
  ts: string;
}
