import { OfficialShopModel } from "./OfficialShopModel";
import { OfficialPostModel } from "./OfficialPostModel";

export interface OfficialShopListModel extends OfficialShopModel {
  /// 基础浏览量
  viewBaseCount: number;

  /// 浏览量
  views: number;

  /// 店龄
  shopYears: number;

  /// 成交订单数
  dealOrder: 0;

  /// 评分
  selfPopularity: number;

  /// 帖子列表
  postList: OfficialPostModel[];
}
