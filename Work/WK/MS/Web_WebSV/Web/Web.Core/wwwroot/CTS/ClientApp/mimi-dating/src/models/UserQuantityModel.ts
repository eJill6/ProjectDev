import { PostType } from "@/enums";
export interface UserQuantityModel {
  /// 發送數量
  totalSend: 0;

  ///显示剩余发帖次数
  showRemainingSend:0;
  
  /// 剩餘發送次數
  remainingSend: 0;

  /// 免費剩餘解次數
  remainingFreeUnlock: 0;

  //總收益
  totalIncome: 0;
}
