export interface OfficialCommentModel {
  /// 頭像
  avatarUrl: string;

  /// 評論當下暱稱
  nickname: string;

  /// 顏值評分
  facialScore: number;

  /// 服務質量
  serviceQuality: number;

  /// 評論內容
  comment: string;

  /// 發布時間(審核通過) = 審核通過時間
  publishTime: string;
}
