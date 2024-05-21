export interface ExpenseInfoModel {
  /// 支付金額
  amount: string;

  /// 標題
  title: string;

  /// 交易時間
  transactionTime: string;

  /*
  None：0,
  广场帖解锁：1,
  寻芳阁帖解锁：2,
  官方帖解锁：3,
  体验帖解锁：4,
  VIP：51
  */
  category: number;

  /// 支出: 1，退款: 3
  transactionType: number;
}
