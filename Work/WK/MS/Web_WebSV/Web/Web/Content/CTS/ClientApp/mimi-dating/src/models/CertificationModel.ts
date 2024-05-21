import { IdentityApplyStatusType, IdentityType } from "@/enums";

export interface CertificationModel {  
  /// 剩餘發佈次數
  remainPublish: number;
  
  /// 申請身份
  applyIdentity: IdentityType;

  /// 申請狀態
  applyStatus: IdentityApplyStatusType;
}
