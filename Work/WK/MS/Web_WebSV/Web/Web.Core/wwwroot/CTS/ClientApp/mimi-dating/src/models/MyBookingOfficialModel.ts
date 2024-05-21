import { MyBookingStatusType } from "@/enums";

export interface MyBookingOfficialModel {
  /// 狀態
  status: MyBookingStatusType;

  /// 第幾頁
  pageNo: number;
}
