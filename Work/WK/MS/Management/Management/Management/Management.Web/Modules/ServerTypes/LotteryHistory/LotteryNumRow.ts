import { fieldsProxy } from "@serenity-is/corelib/q";

export interface LotteryNumRow {
    CurrentLotteryId?: number;
    CurrentLotteryTime?: string;
    LotteryType?: string;
    CurrentLotteryNum?: string;
    LotteryId?: number;
    IssueNo?: string;
    AddTime?: string;
    UpdateTime?: string;
    IsLottery?: boolean;
    Msg?: string;
    DrawTimeConsuming?: number;
}

export abstract class LotteryNumRow {
    static readonly idProperty = 'CurrentLotteryId';
    static readonly nameProperty = 'LotteryType';
    static readonly localTextPrefix = 'LotteryHistory.LotteryNum';
    static readonly deletePermission = 'LotteryHistory:LotteryNum';
    static readonly insertPermission = 'LotteryHistory:LotteryNum';
    static readonly readPermission = 'LotteryHistory:LotteryNum';
    static readonly updatePermission = 'LotteryHistory:LotteryNum';

    static readonly Fields = fieldsProxy<LotteryNumRow>();
}
