import { ContactType } from "@/enums";

export interface ContactInfoModel {
  /// 聯絡方式。1：微信、2：QQ、3：手機號

  contactType: ContactType;

  /// 聯繫方式
  contact: string;
}
