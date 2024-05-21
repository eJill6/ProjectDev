export interface OfficialShopListParamModel {
  /// 搜索关键字
  keyword: string;

  /// 使用者编号
  userId?: number;

  // 排序方式，0：（默认排序，成交订单数倒序）， 1：（评分倒序，若相同则按店龄倒序）
  sortType: number;

  pageNo: number;

  pageSize: number;
}
