export enum ViewOfficialReportStatus {
  /// 可以回報
  CanReport = 0,
  /// 未預約過
  NoAppointment = 1,

  /// 已回報過
  HasReported = 2,

  /// 預約超过72小时后不可投诉
  Overtime = 3,

  /// 所有预约单已完成
  AllAppointmentsFinished = 4
}
