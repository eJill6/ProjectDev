import { OfficialComboModel } from "./OfficialComboModel";

export interface OfficialProductModel {
  /// 信息標題
  title: string;

  /// 地區代碼
  areaCode: string;

  /// 年齡(歲)
  age: number;

  /// 身高(cm)
  height: number;

  /// 罩杯
  cup: number;

  /// 營業時間
  businessHours: string;

  /// 服務種類 Id
  serviceIds: number[];

  /// 詳細地址
  address: string;

  /// 服務描述
  serviceDescribe: string;

  /// 套餐設定
  combo: OfficialComboModel[];

  /// 後台傳進來的 UserId

  userId?: number | undefined;

  /// 照片 id list
  photoIds: string[];

  /// 視頻 id list
  videoIds: string[];
  /// 照片來源
  photoSource: { [name: string]: string };
  /// 視頻來源
  videoSource: { [name: string]: string };
}
