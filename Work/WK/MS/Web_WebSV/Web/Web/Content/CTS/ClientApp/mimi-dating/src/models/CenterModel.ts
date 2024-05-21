import { IdentityType } from "@/enums/IdentityType";
import { EfficientVipModel } from "./EfficientVipModel";
import { UserQuantityModel } from "./UserQuantityModel";

export interface CenterModel {
  /// 預約次數
  bookingCount: 0;
  /// 解鎖數量
  unlockCount: 0;
  /// 上架數量
  putOnShelvesCount: 0;
  /// 收益
  income: 0;
  /// 鑽石數
  point: 0;
  /// 有效的會員卡
  vips: EfficientVipModel[];
  /// 會員各種數量
  quantity: UserQuantityModel;
  /// 積分
  rewardsPoint: 0;
  /// 餘額
  amount: string;
  /// 註冊時間
  registerTime: string;

  identity: IdentityType;
  
  hasPhone: boolean;
}
