import { PostType } from "@/enums";

export interface OverviewPostTypeStatisticModel {
  /// 帖子類型
  type: PostType;

  /// 已發帖
  publishedCount: number;

  /// 解鎖次數
  unlockCount: number;

  /// 總收益(累計收益)
  totalIncome: string;
}
