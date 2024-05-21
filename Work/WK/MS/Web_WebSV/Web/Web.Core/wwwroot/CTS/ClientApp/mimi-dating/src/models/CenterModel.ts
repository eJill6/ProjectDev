import { IdentityType } from "@/enums/IdentityType";
import { EfficientVipModel } from "./EfficientVipModel";
import { UserQuantityModel } from "./UserQuantityModel";
import { ContactType } from "@/enums/ContactType";
import {UserUnreadMessageModel} from "./UserUnreadMessageModel"
import{UserFavoriteStatisticsModel} from "@/models";

export interface CenterModel {
  //用户Id
  userId: number;
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

  contactType: ContactType;

  contact: string;

  avatar: string;
  ///用户未读消息
  unreadMessage:UserUnreadMessageModel;
  ///广场收藏
  collectSquareCount:number;
  ///寻芳阁
  collectXfgCount:number;
  ///店铺
  collectShopCount:number;
  ///用户的收藏数
  userFavorites:UserFavoriteStatisticsModel[];
  //用户是否申请过老板
  isApplyBoss:boolean;

}
