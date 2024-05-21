import { CommentStatusType, IdentityType, PostType } from "@/enums";
import { UserUnlockGetInfoModel } from "./UserUnlockGetInfoModel";
import { ProductBaseModel } from "./ProductBaseModel";

export interface ProductDetailModel extends ProductBaseModel {
  /// 視頻連結
  videoUrl: string;
  /// 標題
  title: string;
  /// 區域代碼
  areaCode: string;
  /// 訊息標題 (等同列表的職業)
  job: string;
  /// 頭像連結
  avatarUrl: string;
  /// 發帖人當下暱稱
  nickname: string;
  /// 卡的類型
  cardType: number[];
  /// 註冊時間
  registerTime: string;
  /// 更新時間
  updateTime: string;
  /// 收藏數
  favorites: string;
  /// 評論數
  comments: string;
  /// 觀看數
  views: string;
  /// 定價
  unlockAmount: number;
  /// 特價
  discount: number;
  /// 免費解鎖次數
  freeUnlockCount: number;
  /// 是否解鎖
  isUnlock: boolean;
  /// 用戶解鎖可以得到的訊息
  unlockInfo: UserUnlockGetInfoModel;
  /// 身高
  height: string;
  /// 年齡
  age: string;
  /// 罩杯
  cup: string;
  /// 數量
  quantity: string;
  /// 最低價格
  lowPrice: string;
  /// 最高價格
  highPrice: string;
  /// 營業時間
  businessHours: string;
  /// 服務項目
  serviceItem: string[];
  /// 服務描述
  serviceDescribe: string;

  /// 是否為精選
  isFeatured: boolean;

  /// 是否已評論
  postCommentStatus: CommentStatusType;

  commentId: string;

  commentMemo: string;

  ///已回報/已投訴
  hasReported: boolean;

  postType: PostType;

  userIdentity: IdentityType;

  earnestMoney: string;
  ///是否能投訴
  canReported: boolean;
  //判斷當前是否
  hasFreeUnlockAuth: false;
  //自定義
  localPhotos: [];
}
