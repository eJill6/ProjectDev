import { BookingPaymentType } from "@/enums";

export interface BookingOfficialModel {
  /// 帖子 Id
  postId: string;

  /// 发帖人原身份
  postUserIdentity: number;

  /// 官方帖價格ID
  postPriceId: number;

  /// 支付方式。1：預約支付、2：全額支付
  paymentType: BookingPaymentType;
}
