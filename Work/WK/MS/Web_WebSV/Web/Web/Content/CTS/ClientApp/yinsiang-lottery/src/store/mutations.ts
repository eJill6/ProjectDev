import {
  AppData,
  IssueNo,
  IndexedRebatePro,
  BetInfo,
  LotteryMenuInfo,  
} from "@/models";
import { MutationTree } from "vuex";
import { State } from "./state";

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
    let index = state.currnetBetInfo.findIndex((x) => x.id === payload);

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
  },
};
