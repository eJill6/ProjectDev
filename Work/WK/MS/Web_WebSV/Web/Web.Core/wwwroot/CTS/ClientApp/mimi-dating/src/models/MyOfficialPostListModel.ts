import { ReviewStatusType } from "@/enums";

export interface MyOfficialPostListModel {
  /// 帖子 Id
  postId: string;

  /// 封面Url
  coverUrl: string;

  /// 发帖人ID
  userId:string;

  /// 发帖人当下昵称
  nickname:string;

  /// 審核狀態
  status: ReviewStatusType;

  /// 预约次数
  appointmentCount:number;

  /// 標題
  title: string;

  /// 更新时间
  updateTimeText:string;

  /// 审核时间
  examineTimeText:string;

  ///创建时间
  createTimeText:string;

  /// 是否上架
  isDelete:boolean;
}
