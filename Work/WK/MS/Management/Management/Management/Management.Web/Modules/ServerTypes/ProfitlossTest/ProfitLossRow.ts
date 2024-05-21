import { fieldsProxy } from "@serenity-is/corelib/q";

export interface ProfitLossRow {
    ProfitLossId?: string;
    UserId?: number;
    ProfitLossTime?: string;
    ProfitLossType?: string;
    ProfitLossMoney?: number;
    WinMoney?: number;
    PrizeMoney?: number;
    AllBetMoney?: number;
    GameType?: string;
    PlayId?: string;
    Memo?: string;
}

export abstract class ProfitLossRow {
    static readonly idProperty = 'ProfitLossId';
    static readonly nameProperty = 'ProfitLossId';
    static readonly localTextPrefix = 'ProfitlossTest.ProfitLoss';
    static readonly deletePermission = 'Administration:General';
    static readonly insertPermission = 'Administration:General';
    static readonly readPermission = 'Administration:General';
    static readonly updatePermission = 'Administration:General';

    static readonly Fields = fieldsProxy<ProfitLossRow>();
}
