export interface CommentModel {
  /// 頭像
  avatarUrl: "";

  /// 評論當下暱稱
  nickname: "";

  /// 消費當下時間
  spentTime: "";

  /// 區域代碼
  areaCode: "";

  /// 評論內容
  comment: "";

  /// 發布時間(審核通過) = 審核通過時間
  publishTime: "";

  /// 評論上傳的照片
  photoUrls: string[];
}
