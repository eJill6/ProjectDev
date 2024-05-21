import { GetterTree } from "vuex";
import { State } from "./state";
import {
  PlayConfig,
  PlayTypeInfo,
  PlayTypeRadioInfo,
  IndexedRebatePro,
  SelectedModel,
  LotteryMenuInfo,
  RebatePros,
  ChangLongSelectedItem,
  IssueNo,
  DateTimeModel,
} from "@/models";
import {
  basePlayTypeId,
  correspondContext,
  shengXiaoContext,
} from "@/gameConfig";
import { TimeRules } from "@/enums";

export type Getters = {
  [K in keyof GetterType]: ReturnType<GetterType[K]>;
};

type GetterType = {
  currentPlayConfig(state: State): PlayConfig | null;
  currentPlayType(state: State): PlayTypeInfo | null;
  currentPlayTypeRadio(state: State, getter: Getters): PlayTypeRadioInfo | null;
  currentRebatePro(state: State): IndexedRebatePro | null;
  betCount(state: State): number;
  countdownTime(state: State): DateTimeModel;
  playConfig(
    state: State,
    getter: Getters
  ): { [key: string]: Array<Array<string>> };
  playTypeSelected(state: State, getter: Getters): SelectedModel;
  selectedNumbers(
    state: State,
    getter: Getters
  ): { [key: string]: Array<Array<string>> };
  getLotteryInfoMenu(state: State): Array<LotteryMenuInfo>;
  getGameType(state: State): string;
  selectedChangLongNumbers(
    state: State,
    getter: Getters
  ): ChangLongSelectedItem;
  allRebatePros(state: State, getter: Getters): RebatePros;
  allLotteryIssueNo(state: State, getter: Getters): IssueNo[];
  showTimeRuleStatus(state: State): TimeRules;
};

export const getters: GetterTree<State, State> & GetterType = {
  currentPlayConfig: (state) => {
    return (
      state.playConfigs.find((x) => x.playModeId === state.currentPlayModeId) ||
      null
    );
  },
  currentPlayType: (state) => {
    let playMode = state.playConfigs.find(
      (x) => x.playModeId === state.currentPlayModeId
    );

    if (!playMode) return null;

    return (
      playMode.playTypeInfos.find(
        (x) => x.info.playTypeID === state.currentPlayTypeId
      ) || null
    );
  },
  currentPlayTypeRadio: (state, getter) => {
    let currentPlayType = getter.currentPlayType;

    if (!currentPlayType) return null;

    let PlayTypeRadios = Object.values(
      currentPlayType.playTypeRadioInfos
    ).flatMap((x) => x);

    return (
      PlayTypeRadios.find(
        (x) => x.info.playTypeRadioID === state.currentPlayTypeRadioId
      ) || null
    );
  },
  currentRebatePro: (state) => {
    return state.rebatePros.find((x) => x.id === state.currentRebateId) || null;
  },
  betCount: (state) => {
    return state.currnetBetInfo.length;
  },
  countdownTime(state): DateTimeModel {
    let source =
      state.timeRule === TimeRules.closureCountdown
        ? state.closureSecondLeft
        : state.issueNoSecondLeft;
    let seconds = source % 60;

    return {
      timeRule: state.timeRule,
      secondsTotal: source,
      secondsTenDigits: `${Math.floor(seconds / 10)}`,
      secondsDigits: `${seconds % 10}`,
    };
  },
  playConfig: (state, getter) => {
    const playConfig = state.playConfigs[0];
    const playTypeInfo = playConfig.playTypeInfos.filter(
      (x) => !!basePlayTypeId.includes(x.basePlayTypeId)
    );
    const playTypeRadioInfo = playTypeInfo[0].playTypeRadioInfos[""];
    let gameType: { [key: string]: Array<Array<string>> } = {};
    playTypeRadioInfo[0].fields.forEach((info) => {
      if (!gameType[info.prompt]) {
        gameType[info.prompt] = [] as Array<Array<string>>;
      }
      gameType[info.prompt].push(info.numbers);
    });
    //對生肖做排序處理
    if (gameType[shengXiaoContext] && getter.currentRebatePro) {
      //取最小賠率排序
      const info = getter.currentRebatePro.numberOdds;
      const shengXiaoValues = Object.keys(info)
        .filter((item) => item.indexOf(shengXiaoContext) >= 0)
        .map((keyName) => info[keyName]);
      //最小賠率的index
      const indexMenor = shengXiaoValues.indexOf(Math.min(...shengXiaoValues));

      const newArray = gameType[shengXiaoContext].reduce((a, b) => {
        return a.concat(b);
      });
      //對最小賠率的index前面的生肖往後搬
      new Array(indexMenor).fill(0).forEach(function () {
        newArray.splice(newArray.length - 1, 0, newArray.splice(0, 1)[0]);
      });
      //取代原先生肖排序
      const length = newArray.length / 2;
      const first = newArray.slice(0, length);
      const second = newArray.slice(length);
      gameType[shengXiaoContext] = [first, second];
    }
    return gameType;
  },
  playTypeSelected: (state, getter) => {
    let playConfig = getter.playConfig;
    const playNames = Object.keys(playConfig);
    const playName = playNames[state.currentPlayTypeSelected];
    const playEnum = correspondContext[playName] as string;
    let model = new SelectedModel();
    model.playEnum = playEnum;
    model.playName = playName;
    model.selected = state.currentPlayTypeSelected;
    return model;
  },
  selectedNumbers: (state, getter) => {
    let playConfig = getter.playConfig;
    for (let key in playConfig) {
      const gameType = playConfig[key];
      if (!state.selectedNumbers[key]) {
        state.selectedNumbers[key] =
          gameType.map((x) => new Array(x.length)) || [];
      }
    }
    return state.selectedNumbers;
  },
  getLotteryInfoMenu: (state) => {
    return state.lotteryMenuInfo;
  },
  getGameType: (state) => {
    return state.gameType;
  },
  selectedChangLongNumbers: (state) => {
    return state.selectedChangLongNumbers;
  },
  allRebatePros: (state) => {
    return state.allRebatePros;
  },
  allLotteryIssueNo: (state) => {
    return state.allLotteryIssueNo;
  },
  showTimeRuleStatus(state) {
    return state.timeRule;
  },
};
