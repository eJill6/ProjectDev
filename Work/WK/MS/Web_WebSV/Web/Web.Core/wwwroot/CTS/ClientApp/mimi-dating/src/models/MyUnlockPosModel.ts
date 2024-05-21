import { ReviewStatusType } from "@/enums";
import { ProductListModel } from "./ProductListModel";

export interface MyUnlockPosModel extends ProductListModel {
  /// 審核狀態
  status: ReviewStatusType;

  /// 評論編號
  commentId: string;

  /// 評論結果
  commentMemo: string;
}
