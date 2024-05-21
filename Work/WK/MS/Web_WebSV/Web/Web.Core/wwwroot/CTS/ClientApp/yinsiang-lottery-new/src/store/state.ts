import {
  IssueNo,
  LotteryInfo,
  PlayConfig,
  Settings,
  IndexedRebatePro,
  BetInfo,
  MsSetting,
  LotteryMenuInfo,
} from "@/models";
import { TimeRules } from "@/enums";

export const state = {
  isLoading: false,
  lotteryInfo: {} as unknown as LotteryInfo,
  playConfigs: [] as PlayConfig[],
  settings: {} as unknown as Settings,
  msSetting: {} as unknown as MsSetting,
  currentPlayModeId: 0,
  currentPlayTypeId: 0,
  currentPlayTypeRadioId: 0 as unknown as number,
  issueNo: {} as unknown as IssueNo,
  issueNoSecondLeft: 0,
  closureSecondLeft: 0,
  timeRule: TimeRules.unknown,
  rebatePros: [] as IndexedRebatePro[],
  currentRebateId: 0,
  currnetBetInfo: [] as BetInfo[],
  baseAmount: 5,
  balance: 0,
  formattedBalance: "",
  currentPlayTypeSelected: 0,
  selectedNumbers: {} as { [key: string]: Array<Array<string>> },
  lotteryMenuInfo: {} as LotteryMenuInfo[],
  gameType: "" as string,
  /**輪盤用的指標，0對應23號碼 */
  rouletteIndex: -1,
  /**輪盤用的，增加角度以利轉圈漂亮 */
  increaseDegree: false as boolean,
};

export type State = typeof state;
