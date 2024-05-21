export interface CommentData {
  /// 帖子 Id
  postId: string;

  /// 消費時間
  spentTime: string;

  /// 評論內容
  comment: string;

  /// 地區代碼
  areaCode: string;

  /// 照片 id list
  photoIds: string[];

  /// 照片對應 id/value
  photoSource: { [name: string]: string };
}
