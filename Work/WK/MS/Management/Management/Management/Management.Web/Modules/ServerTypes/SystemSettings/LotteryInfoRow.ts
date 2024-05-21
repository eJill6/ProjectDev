import { fieldsProxy } from "@serenity-is/corelib/q";

export interface LotteryInfoRow {
    LotteryID?: number;
    LotteryType?: string;
    TypeURL?: string;
    GameTypeID?: number;
    Priority?: number;
    OfficialLotteryUrl?: string;
    NumberTrendUrl?: string;
    Status?: number;
    DefaultSec?: number;
    AppPriority?: number;
    HotNew?: number;
    MaxBonusMoney?: number;
    Notice?: string;
    RecommendSort?: number;
    CustomMoney?: number;
    WebSeq?: number;
    AppSeq?: number;
}

export abstract class LotteryInfoRow {
    static readonly idProperty = 'LotteryID';
    static readonly nameProperty = 'LotteryType';
    static readonly localTextPrefix = 'SystemSettings.LotteryInfo';
    static readonly deletePermission = 'SystemSettings:LotteryInfo';
    static readonly insertPermission = 'SystemSettings:LotteryInfo';
    static readonly readPermission = 'SystemSettings:LotteryInfo';
    static readonly updatePermission = 'SystemSettings:LotteryInfo';

    static readonly Fields = fieldsProxy<LotteryInfoRow>();
}
