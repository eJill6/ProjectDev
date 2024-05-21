import { RefundReasonType } from "@/enums";

export interface ApplyRefundModel {
  /// 申请退费的预约单
  bookingId: string;

  /// 退费类型
  reasonType: RefundReasonType;

  /// 描述
  describe: string;

  /// 證據圖片
  photoIds: string[];
}
