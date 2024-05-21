import { BookingPaymentType,MyBookingStatusType } from "@/enums";

export interface BookingManageModel {
  userId: number;

  nickname: string;

  avatarUrl: string;

  /// 標題
  title: string;

  /// 狀態
  status: MyBookingStatusType;

  /// 聯絡資訊
  contact: string;

  /// 預約時間
  bookingTime: string;

  /// 接單時間
  acceptTime: string;

  /// 取消時間
  cancelTime: string;

  /// 完成時間
  finishTime: string;

  /// 支付類型
  paymentType: BookingPaymentType;

  /// 預約ID
  bookingId: string;

  /// 狀態
  statusText: string;

  /// 支付金額
  paymentMoney: string;
}
