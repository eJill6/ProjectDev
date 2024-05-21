import { ReviewStatusType } from "@/enums";

export interface MyPostListModel {
  /// 帖子 Id
  postId: string;

  /// 封面Url
  coverUrl: string;

  /// 審核狀態
  status: ReviewStatusType;

  /// 標題
  title: string;

  /// 解鎖次數
  unlockCount: number;

  /// 建立時間
  createTime: string;
  memo:string
}
