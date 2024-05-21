export interface ProductListModel {
  /// 帖子 Id
  postId: string;

  /// 封面照片
  coverUrl: string;

  /// 標題
  title: string;

  /// 身高
  height: string;

  /// 年齡
  age: string;

  /// 罩杯
  cup: string;

  /// 服務項目
  serviceItem: string[];

  /// 職業
  job: string;

  /// 地區編碼
  areaCode: string;

  /// 是否為精選
  isFeatured: boolean;

  /// 期望收入
  lowPrice: string;

  /// 收藏數
  favorites: string;
  
  /// 評論數
  comments: string;

  /// 觀看數
  views: string;

  /// 更新時間
  updateTime: string;
}
