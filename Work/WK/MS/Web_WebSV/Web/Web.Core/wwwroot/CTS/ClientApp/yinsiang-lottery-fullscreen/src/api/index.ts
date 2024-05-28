import httpEncoding from "./httpEncoding";

import {
  AppData,
  IssueNo,
  RebatePro,
  IndexedRebatePro,
  ResultHistory,
  LotteryMenuInfo,
  LongDragonInfo,
  RebatePros,
  BetsLog,
} from "@/models";
import { LotteryPlanData } from "@/models";

const getAppData = () => {
  let appData = (<any>window).lotterySpa.appData as AppData;
  return Object.assign({}, appData);
};

const getImageUrl = (imageName: string) => {
  if (!Boolean(imageName)) return "";
  const isAssetImage = imageName.startsWith("@/assets/images/");

  if (imageName.startsWith("http")) {
    return imageName;
  }

  let appData = getAppData();
  let cdnUrl = appData.settings.cdnUrl;
  if (cdnUrl) {
    if (isAssetImage) {
      const path = imageName.split("/");
      const fileName = path.pop();
      return cdnUrl + fileName;
    } else {
      return cdnUrl + imageName;
    }
  }

  return isAssetImage
    ? require("@/assets/images/" + imageName.replace("@/assets/images/", ""))
    : imageName;
};

const getNextIssueNoAsync = (lotteryId: number) =>
  httpEncoding.lotterySpaGetAsync<IssueNo>("GetNextIssueNo", { lotteryId });
const getNextIssueNosAsync = () =>
  httpEncoding.lotterySpaGetAsync<IssueNo[]>("GetNextIssueNos");

const getBalanceAsync = async () => {
  let result = await httpEncoding.playformPostAsync<{
    AvailableScoresDecimal: number;
    AvailableScores: string;
  }>("/Home/GetRefreshBalanceInfo");

  return {
    balance: result.AvailableScoresDecimal,
    formattedBalance: result.AvailableScores,
  };
};

const getRebateProAsync = async (
  lotteryId: number,
  playTypeId: number,
  playTypeRadioId: number
) => {
  let result = await httpEncoding.lotterySpaGetAsync<RebatePro[]>(
    "GetRebatePro",
    { lotteryId, playTypeId, playTypeRadioId, isSingleRebatePro: true }
  );

  return result.map((x, index) => ({
    id: index + 1,
    ...x,
  })) as IndexedRebatePro[];
};

const getRebateProsAsync = async () => {
  let result = await httpEncoding.lotterySpaGetAsync<RebatePros>(
    "GetRebatePros"
  );
  return result;
};

const getIssueHistoryAsync = async (
  lotteryId: number,
  count: number,
  nextCursor: string
) => {
  type Result = {
    nextCursor: string;
    list: { issueNo: string; currentLotteryNum: string }[];
  };

  let result = await httpEncoding.lotterySpaGetAsync<Result>("IssueHistory", {
    lotteryId,
    count,
    nextCursor,
  });

  var list = result.list.map((x) => ({
    issueNo: x.issueNo,
    drawNumbers: (x.currentLotteryNum || "").split(","),
  }));

  return { list, nextCursor: result.nextCursor };
};

const postOrderAsync = async (params: {
  currentIssueNo: string;
  selectedNums: string;
  amount: number;
  price: string;
  currencyUnit: string | null;
  ratio: number | null;
  lotteryId: number;
  playType: number;
  playTypeName: string;
  playTypeRadio: number;
  playTypeRadioName: string;
  habitRebatePro: number;
  roomId: string;
}) => {
  // uuid
  let key = "xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx".replace(
    /[xy]/g,
    function (c) {
      var r = (Math.random() * 16) | 0,
        v = c == "x" ? r : (r & 0x3) | 0x8;
      return v.toString(16);
    }
  );
  let times = new Date();
  let body = { ...params, key, times };

  return await httpEncoding.lotterySpaPostAsync<{
    noteTime: string;
    playID: number;
    success: boolean;
  }>("PlaceOrder", body);
};

const getOrderHistoryAsync = async (
  lotteryId: number | null,
  status: string,
  searchDate: string,
  nextCursor: string,
  count: number,
  roomId: string
) => {
  return await httpEncoding.lotterySpaGetAsync<ResultHistory>(
    "GetSpecifyOrderList",
    {
      lotteryId,
      status,
      searchDate,
      cursor: nextCursor,
      pageSize: count,
      roomId,
    }
  );
};

const getFollwBetOrderAsync = async (palyId: string, lotteryId: number) => {
  return await httpEncoding.lotterySpaGetAsync<ResultHistory>("GetFollowBet", {
    palyId,
    lotteryId,
  });
};

const getViewModel = async (id: string) => {
  return await httpEncoding.lotterySpaGetAsync<AppData>("getViewModel", {
    id,
    isFullscreen: true,
  });
};

const getAllLotteryInfo = async () =>
  await httpEncoding.lotterySpaGetAsync<LotteryMenuInfo[]>("GetAllLotteryInfo");

const getLongData = async (lotteryId: number) =>
  await httpEncoding.lotterySpaGetAsync<LongDragonInfo>("GetLongData", {
    lotteryId,
  });

const getLongDatas = async () =>
  await httpEncoding.lotterySpaGetAsync<LongDragonInfo[]>("GetLongDatas");

const getLotteryPlanData = async (lotteryId: number, planType: number) =>
  await httpEncoding.lotterySpaGetAsync<LotteryPlanData>("GetLotteryPlanData", {
    lotteryId,
    planType,
  });

const getUnawardedSummary = async (params: {
  lotteryId: number;
  currentIssueNo: string;
  roomId: string;
}) => {
  return await httpEncoding.lotterySpaGetAsync<BetsLog[]>(
    "GetUnawardedSummary",
    params
  );
};

export default {
  getAppData,
  getNextIssueNoAsync,
  getBalanceAsync,
  getRebateProAsync,
  getIssueHistoryAsync,
  postOrderAsync,
  getOrderHistoryAsync,
  getFollwBetOrderAsync,
  getViewModel,
  getAllLotteryInfo,
  getImageUrl,
  getLongData,
  getLotteryPlanData,
  getLongDatas,
  getRebateProsAsync,
  getNextIssueNosAsync,
  getUnawardedSummary,
};