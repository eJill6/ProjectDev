import { ContactInfoModel } from "./ContactInfoModel";

export interface UserUnlockGetInfoModel {
  /// 聯擊方式
  contactInfos: ContactInfoModel[];
  /// 聯繫方式 - 地址
  address: string;
}
