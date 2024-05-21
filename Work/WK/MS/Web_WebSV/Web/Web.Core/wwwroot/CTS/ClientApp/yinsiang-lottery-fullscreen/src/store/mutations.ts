import {
  AppData,
  IssueNo,
  IndexedRebatePro,
  BetInfo,
  LotteryMenuInfo,
  ChangLongSelectedItem,
  RebatePros,
  LongDragonInfo,
  ChangLongDateTimeModel,
  BetsLog,
} from "@/models";
import { MutationTree } from "vuex";
import { State } from "./state";
import { NavContentType } from "@/enums";

export enum MutationType {
  SetIsLoading = "M_IsLoading",
  SetLottery = "M_SetLottery",
  SetCurrentPlayMode = "M_SetCurrentPlayMode",
  SetCurrentPlayType = "M_SetCurrentPlayType",
  SetCurrentPlayTypeRadio = "M_SetCurrentPlayTypeRadio",
  SetIssueNo = "M_SetIssueNo",
  DrawIssueNo = "M_DrawIssueNo",
  SetIssueNoSecondLeft = "M_IssueNoSecondLeft",
  SetClosureSecondLeft = "M_SetClosureSecondLeft",
  SetTimeRuleStatus = "M_SetTimeRuleStatus",
  SetBalance = "M_SetBalance",
  SetRebatePros = "M_SetRebatePros",
  SetCurrentRebatePro = "M_SetCurrentRebatePro",
  SetCurrentBetInfo = "M_SetCurrentBetInfo",
  RemoveCurrentBetInfoById = "M_RemoveCurrentBetInfoById",
  SetBaseAmount = "M_SetBaseAmount",
  SetPlayType = "M_SetPlayType",
  SetNumbers = "M_SetNumbers",
  SetAllLotteryInfo = "M_SetAllLotteryInfo",
  SetGameType = "M_SetGameType",
  InitData = "M_InitData",
  SetNavContent = "M_SetNavContent",
  SetChangLongNumbers = "M_SetChangLongNumbers",
  SetAllRebatePros = "M_SetAllRebatePros",
  SetAllLotteryIssueNo = "M_SetAllIssueNo",
  SetChangLongInfo = "M_SetChangLongInfo",
  SetFilterChangLongCount = "M_SetFilterChangLongCount",
  ReloadBetHistory = "M_ReloadBetHistory",
  SetStopAnimation = "M_SetStopAnimation",
  SetChangLongDateTimeInfo = "M_SetChangLongDateTimeInfo",
  SetRouletteIndex = "M_SetRouletteIndex",
  SetIncreaseDegree = "M_SetIncreaseDegree",
  SetBetsLog = "M_SetBetsLog",
}

export type Mutations<S = State> = {
  [MutationType.SetIsLoading](state: S, payload: boolean): void;
  [MutationType.SetLottery](state: S, payload: AppData): void;
  [MutationType.SetCurrentPlayMode](state: S, payload: number): void;
  [MutationType.SetCurrentPlayType](state: S, payload: number): void;
  [MutationType.SetCurrentPlayTypeRadio](state: S, payload: number): void;
  [MutationType.SetIssueNo](state: S, payload: IssueNo): void;
  [MutationType.DrawIssueNo](state: S, payload: string): void;
  [MutationType.SetIssueNoSecondLeft](state: S, payload: number): void;
  [MutationType.SetClosureSecondLeft](state: S, payload: number): void;
  [MutationType.SetTimeRuleStatus](state: S, payload: number): void;
  [MutationType.SetBalance](
    state: S,
    payload: { balance: number; formattedBalance: string }
  ): void;
  [MutationType.SetRebatePros](state: S, payload: IndexedRebatePro[]): void;
  [MutationType.SetCurrentRebatePro](state: S, payload: number): void;
  [MutationType.SetCurrentBetInfo](state: S, payload: BetInfo[]): void;
  [MutationType.RemoveCurrentBetInfoById](state: S, payload: string): void;
  [MutationType.SetBaseAmount](state: S, payload: number): void;
  [MutationType.SetPlayType](state: S, payload: number): void;
  [MutationType.SetNumbers](
    state: S,
    payload: { [key: string]: Array<Array<string>> } | null
  ): void;
  [MutationType.SetAllLotteryInfo](
    state: S,
    payload: Array<LotteryMenuInfo>
  ): void;
  [MutationType.SetGameType](state: S, payload: string): void;
  [MutationType.InitData](state: S): void;
  [MutationType.SetNavContent](state: S, payload: NavContentType): void;
  [MutationType.SetChangLongNumbers](
    state: S,
    payload: ChangLongSelectedItem | null
  ): void;
  [MutationType.SetAllRebatePros](state: S, payload: RebatePros): void;
  [MutationType.SetAllLotteryIssueNo](state: S, payload: IssueNo[]): void;
  [MutationType.SetChangLongInfo](state: S, payload: LongDragonInfo[]): void;
  [MutationType.SetFilterChangLongCount](state: S, payload: number): void;
  [MutationType.ReloadBetHistory](state: S, payload: boolean): void;
  [MutationType.SetStopAnimation](state: S, payload: boolean): void;
  [MutationType.SetChangLongDateTimeInfo](
    state: S,
    payload: ChangLongDateTimeModel[]
  ): void;
  [MutationType.SetRouletteIndex](state: S, payload: number): void;
  [MutationType.SetIncreaseDegree](state: S, payload: boolean): void;
  [MutationType.SetBetsLog](state: S, payload: BetsLog[]): void;
};

export const mutations: MutationTree<State> & Mutations = {
  [MutationType.SetIsLoading](state, payload) {
    state.isLoading = payload;
  },
  [MutationType.SetCurrentPlayMode](state, payload) {
    state.currentPlayModeId = payload;
  },
  [MutationType.SetCurrentPlayType](state, payload) {
    state.currentPlayTypeId = payload;
  },
  [MutationType.SetCurrentPlayTypeRadio](state, payload) {
    state.currentPlayTypeRadioId = payload;
  },
  [MutationType.SetIssueNo](state, payload) {
    if (!payload.lastDrawNumber) {
      state.lastIssueNo = state.issueNo;
    }
    state.issueNo = payload;
  },
  [MutationType.DrawIssueNo](state, payload) {
    state.issueNo.lastIssueNoIsLottery = !!payload;
    state.issueNo.lastDrawNumber = payload;
  },
  [MutationType.SetIssueNoSecondLeft](state, payload) {
    state.issueNoSecondLeft = payload;
  },
  [MutationType.SetClosureSecondLeft](state, payload) {
    state.closureSecondLeft = payload;
  },
  [MutationType.SetTimeRuleStatus](state, payload) {
    state.timeRule = payload;
  },
  [MutationType.SetLottery](state, payload) {
    state.lotteryInfo = payload.lotteryInfo;
    state.playConfigs = payload.playConfigs;
    state.settings = payload.settings;
    state.msSetting = payload.msSetting;
  },
  [MutationType.SetBalance](state, payload) {
    state.balance = payload.balance;
    state.formattedBalance = payload.formattedBalance;
  },
  [MutationType.SetRebatePros](state, rebatePros) {
    state.rebatePros = rebatePros;
  },
  [MutationType.SetCurrentRebatePro](state, rebateProId) {
    state.currentRebateId = rebateProId;
  },
  [MutationType.SetCurrentBetInfo](state, payload) {
    state.currnetBetInfo = payload;
  },
  [MutationType.RemoveCurrentBetInfoById](state, payload) {
    let currnetBetInfo = state.currnetBetInfo.filter((x) => x);

    let index = currnetBetInfo.findIndex((x) => x.id === payload);
    let betItem = currnetBetInfo.find((x) => x.id === payload) as BetInfo;

    if (betItem) {
      // 目前長龍投注以 lotteryId 存不存在當成長龍投注依據
      if (
        betItem.lotteryId &&
        betItem.lotteryId === state.selectedChangLongNumbers.lotteryId
      ) {
        let betChangLongIndex = state.selectedChangLongNumbers.content.indexOf(
          betItem.selectedBetNumber
        );
        delete state.selectedChangLongNumbers.content[betChangLongIndex];
      } else {
        let playType = state.selectedNumbers[betItem.playTypeRadioName];
        let playTypeIndex = playType.findIndex((x) => {
          return x.indexOf(betItem.selectedBetNumber) > -1;
        });
        let playTypeRadioIndex = state.selectedNumbers[
          betItem.playTypeRadioName
        ][playTypeIndex].indexOf(betItem.selectedBetNumber);

        state.selectedNumbers[betItem.playTypeRadioName][playTypeIndex].splice(
          playTypeRadioIndex,
          1,
          ""
        );
      }
    }
    // 回寫過濾掉的部份
    state.currnetBetInfo = currnetBetInfo;

    if (index > -1) state.currnetBetInfo.splice(index, 1);
  },
  [MutationType.SetBaseAmount](state, payload) {
    state.baseAmount = payload;
  },
  [MutationType.SetPlayType](state, payload) {
    state.currentPlayTypeSelected = payload;
  },
  [MutationType.SetNumbers](state, payload) {
    state.selectedNumbers = payload
      ? payload
      : ({} as { [key: string]: Array<Array<string>> });
  },
  [MutationType.SetAllLotteryInfo](state, payload) {
    state.lotteryMenuInfo = payload;
  },
  [MutationType.SetGameType](state, payload) {
    state.gameType = payload;
  },
  [MutationType.InitData](state) {
    state.currentPlayTypeSelected = 0;
    state.currentPlayModeId = 0;
    state.currentPlayTypeRadioId = 0;
    state.currnetBetInfo = [];
    state.selectedNumbers = {} as { [key: string]: Array<Array<string>> };
    state.selectedChangLongNumbers = {} as ChangLongSelectedItem;
  },
  [MutationType.SetNavContent](state, payload) {
    state.navContentName = payload;
  },
  [MutationType.SetChangLongNumbers](state, payload) {
    state.selectedChangLongNumbers = payload
      ? payload
      : ({} as ChangLongSelectedItem);
  },
  [MutationType.SetAllRebatePros](state, rebatePros) {
    state.allRebatePros = rebatePros;
  },
  [MutationType.SetAllLotteryIssueNo](state, payload) {
    state.allLotteryIssueNo = payload;
  },
  [MutationType.SetChangLongInfo](state, payload) {
    state.changlongInfo = payload;
  },
  [MutationType.SetFilterChangLongCount](state, payload) {
    state.filterChangLongCount = payload;
  },
  [MutationType.ReloadBetHistory](state, payload) {
    state.reloadBetHistory = payload;
  },
  [MutationType.SetStopAnimation](state, payload) {
    state.isStopAnimation = payload;
  },
  [MutationType.SetChangLongDateTimeInfo](
    state,
    payload: ChangLongDateTimeModel[]
  ) {
    state.changLongLotteryTimeInfo = payload;
  },
  [MutationType.SetRouletteIndex](state, payload) {
    state.rouletteIndex = payload;
  },
  [MutationType.SetIncreaseDegree](state, payload) {
    state.increaseDegree = payload;
  },
  [MutationType.SetBetsLog](state, payload){
    state.betsLog = payload
  }
};
