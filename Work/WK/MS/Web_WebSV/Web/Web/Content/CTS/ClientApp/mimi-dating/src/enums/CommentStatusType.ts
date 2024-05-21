export enum CommentStatusType {
  /// 审核中
  UnderReview = 0,

  /// 核准
  Approval = 1,

  /// 未通过
  NotApproved = 2,

  /// 帖子尚未解鎖

  PostLock = 3,

  /// 尚未評論
  NotYetComment = 4,
}
