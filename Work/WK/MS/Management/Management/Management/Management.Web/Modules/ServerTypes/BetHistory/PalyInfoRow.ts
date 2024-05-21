import { fieldsProxy } from "@serenity-is/corelib/q";

export interface PalyInfoRow {
    PalyId?: string;
    PalyCurrentNum?: string;
    PalyNum?: string;
    PlayTypeId?: number;
    LotteryId?: number;
    UserName?: string;
    NoteNum?: number;
    SingleMoney?: number;
    NoteMoney?: number;
    NoteTime?: string;
    IsWin?: boolean;
    WinMoney?: number;
    IsFactionAward?: number;
    PlayTypeRadioId?: number;
    RebatePro?: number;
    RebateProMoney?: string;
    WinNum?: number;
    UserId?: number;
    NoticeId?: number;
    LotteryTime?: string;
    UserRebatePro?: number;
    Multiple?: number;
    OrderKey?: string;
    CurrencyUnit?: number;
    Ratio?: number;
    SourceType?: string;
    MemoJson?: string;
    ClientIp?: string;
    RoomId?: string;
    ResultJson?: string;
}

export abstract class PalyInfoRow {
    static readonly idProperty = 'PalyId';
    static readonly nameProperty = 'PalyId';
    static readonly localTextPrefix = 'BetHistory.PalyInfo';
    static readonly deletePermission = 'Bethistory:PalyInfo';
    static readonly insertPermission = 'Bethistory:PalyInfo';
    static readonly readPermission = 'Bethistory:PalyInfo';
    static readonly updatePermission = 'Bethistory:PalyInfo';

    static readonly Fields = fieldsProxy<PalyInfoRow>();
}
