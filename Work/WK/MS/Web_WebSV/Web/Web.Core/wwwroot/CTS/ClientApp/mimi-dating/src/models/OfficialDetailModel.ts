import { IdentityType, PostType, ReviewStatusType, ViewOfficialReportStatus } from "@/enums";
import { ProductBaseModel } from "./ProductBaseModel";

export interface OfficialDetailModel extends ProductBaseModel {
  // 发帖人用户id
  postUserId: string;

  /// 發帖類型
  postType: PostType;
  
  /// 帖子审核状态
  postStatus: ReviewStatusType;

  //编辑开启
  lockStatus: boolean;

  /// 該帖用戶身份
  userIdentity: IdentityType;

  /// 視頻連結
  videoUrl: string;

  /// 標題
  title: string;

  /// 區域代碼
  areaCode: string;

  /// 頭像連結
  avatarUrl: string;

  /// 暱稱
  nickname: string;

  /// 顏值
  facialScore: string;

  /// 地址
  address: string;

  /// 卡的類型
  cardType: number[];

  /// 身高
  height: string;

  /// 年齡
  age: string;

  /// 罩杯
  cup: string;

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

  /// 官方帖投訴狀態
  reportStatus: ViewOfficialReportStatus;

  /// 总投诉次数
  reportedCount: number;

  /// 評論人數
  comments: string;

  /// 平均顏值
  avgFacialScore: string;

  /// 平均服務質量
  avgServiceQuality: string;
  /// 成交量
  appointmentCount: string;

  /// 是否有未完成預約
  haveUnfinishedBooking: boolean;

  // 店铺名称
  shopName: string;
}
