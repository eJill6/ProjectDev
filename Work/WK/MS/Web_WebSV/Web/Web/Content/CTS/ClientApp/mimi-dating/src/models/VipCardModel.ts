export interface VipCardModel {
  /// VIP Id
  id: number;
  /// 名稱
  name: string;
  /// 售價
  price: number;
  /// 備註
  memo: string;
  /// 類型(廣場、官方...)
  type: number;
  days: number;
}
