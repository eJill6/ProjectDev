import { PostType, ReportType } from "@/enums";

export interface ReportDataModel {
  /// 舉報類型 0：騙子、1：廣告騷擾、2：貨不對版、3：無效聯絡方式

  reportType: ReportType.Fraud;

  /// 描述

  describe: string;

  /// 被檢舉的帖子 Id

  postId: string;

  /// 證據圖片

  photoIds: string[];
}
