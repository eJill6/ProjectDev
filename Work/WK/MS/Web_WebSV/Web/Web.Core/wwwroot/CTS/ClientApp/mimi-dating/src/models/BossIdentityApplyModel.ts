export interface BossIdentityApplyModel {
  /// 店鋪資料
  shopName: string;

  // 店铺介绍

  // 店龄

  // 自评人气
  rating: number;

  /// 妹子數量
  girls: string;

  // 服務價格 - 最低價格
  // lowPrice: number;

  // 服務價格 - 最高價格
  // highPrice: number;

  // 联系软件
  contactApp: string;

  /// 聯繫方式
  contact: string;

  /// 照片 id list
  photoIds: string[];
  /// 店铺介绍
  shopIntroduce:string;
  /// 联系软件
  contactType:string;
  /// 成交订单数
  orderTurnover:number;
  /// 自评人气
  popularity:number;
  /// 电龄
  shopAge:number;

  // 商家照片
  ShopPhotoIds: string[];

  // 成交订单数
  dealOrder: number;
}
