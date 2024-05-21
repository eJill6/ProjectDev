export interface BetInfo {
  id: string;
  lotteryId?: number | null;
  lotteryTypeName?: string | null;
  currentIssueNo?: string | null;
  lotteryTime?: string | null;
  betAmount?: number | null;
  playTypeRadioName: string;
  selectedBetNumber: string;
  odds: string;
  gameTypeName?: string | null;
  gameTypeId?: number | null;
}
