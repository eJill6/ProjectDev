export interface BookingDetailModel {
  priceId: number;

  /// 套餐名稱
  comboName: string;

  /// 套餐價格(餘額)
  comboPrice: string;

  /// 全額價格(鑽石)
  fullPrice: string;

  /// 預約價格(鑽石)
  bookingPrice: string;

  /// 服務時間、次數或包含項目
  service: string;
}
