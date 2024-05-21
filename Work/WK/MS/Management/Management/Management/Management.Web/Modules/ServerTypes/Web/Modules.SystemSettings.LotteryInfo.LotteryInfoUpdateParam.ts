import { ServiceRequest } from "@serenity-is/corelib";
import { UpdateLotteryInfoRequest } from "./Modules.SystemSettings.LotteryInfo.UpdateLotteryInfoRequest";

export interface LotteryInfoUpdateParam extends ServiceRequest {
    items?: UpdateLotteryInfoRequest[];
}
