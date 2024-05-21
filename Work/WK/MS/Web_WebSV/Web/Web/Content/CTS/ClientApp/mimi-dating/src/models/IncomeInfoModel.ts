export interface IncomeInfoModel {
  unlockAmount: string;
  /// 收益
  amount: string;

  /// 解鎖使用者ID
  userId: string;

  /// Title
  title: string;

  /// 貼子 Title
  postTitle: string;

  /// 交易時間
  transactionTime: string;

  /*
  None：0,
  广场帖解锁：1,
  担保帖解锁：2,
  官方帖解锁：3,
  体验帖解锁：4,
  VIP：51
  */
  category: number;
}
