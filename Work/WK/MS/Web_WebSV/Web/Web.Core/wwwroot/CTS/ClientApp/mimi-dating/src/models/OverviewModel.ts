import { OverviewPostTypeStatisticModel } from "./OverviewPostTypeStatisticModel";

export interface OverviewModel {
  /// 頭像
  avatarUrl: string;

  /// 名稱
  nickname: string;

  /// 用戶等級
  level: number;

  /// 該帖用戶身份
  userIdentity: number;

  /// 卡的類型
  cardType: number[];

  /// 保證金
  earnestMoney: string;

  /// 本月收益
  income: string;

  /// 暫鎖收益
  freezeIncome: string;

  /// 累積發佈上限
  publishLimit: number;

  /// 剩餘發佈次數
  remainPublish: number;

  /// 積分
  integral: number;

  /// 發帖統計
  statistic: OverviewPostTypeStatisticModel[];

  //是否營業
  isOpen: boolean;

  ///免费发帖次数
  remainingFreeUnlock:number;
  ///显示给用户看的发帖次数
  showRemainingSend:number;
}


