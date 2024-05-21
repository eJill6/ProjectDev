import { LotteryPlayInfo } from "./LotteryPlayInfo";

export interface LotteryPlanData {
    currentIssueNo: string,
	lotteryID: number,
	currentLotteryPlan: LotteryPlayInfo,
	lotteryPlayInfos: LotteryPlayInfo[]
};
