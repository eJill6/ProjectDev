import { ContactInfoModel } from "./ContactInfoModel";
import {PostType} from "@/enums";

export interface ProductModel {
  /// 帖子類型
  postType: PostType;
  /// 信息類型
  messageId: number;
  /// 解鎖價格，有傳來代表要申請調價
  applyAmount?: Number | undefined;
  /// 信息標題
  title: string;
  /// 地區代碼
  areaCode: string;
  /// 數量
  quantity: string;
  /// 年齡(歲)
  age: number;
  /// 身高(cm)
  height: number;
  /// 罩杯
  cup: number;
  /// 标签
  // label: number[];
  /// 營業時間
  businessHours: string;
  /// 服務種類 Id
  serviceIds: number[];
  /// 最低價格
  lowPrice: number;
  /// HighPrice
  highPrice: number;
  /// 詳細地址
  address: string;
  /// 聯繫資訊
  contactInfos: ContactInfoModel[];
  /// 服務描述
  serviceDescribe: string;
  /// 照片 id list
  photoIds: string[];
  /// 視頻 id list
  videoIds: string[];
   /// 照片來源
  photoSource: { [name: string]: string };
  /// 視頻來源
  videoSource: { [name: string]: string };
}
