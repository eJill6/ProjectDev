import { MediaType, SourceType } from "@/enums";

export interface MergeUploadModel {
  /// 媒體類型 0:圖片, 1:影片
  mediaType: MediaType;

  /// 媒體的來源 0:Banner, 1:贴子, 2:舉報, 3: 評論
  sourceType: SourceType;

  /// 所有分割的路徑
  paths: string[];

  /// 副檔名
  suffix: string;
}
