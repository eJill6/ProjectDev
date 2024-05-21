import { StringEditor, IntegerEditor, DecimalEditor, DateEditor, BooleanEditor, PrefixedContext } from "@serenity-is/corelib";
import { initFormType } from "@serenity-is/corelib/q";

export interface PalyInfoForm {
    PalyCurrentNum: StringEditor;
    PalyNum: StringEditor;
    PlayTypeId: IntegerEditor;
    LotteryId: IntegerEditor;
    UserName: StringEditor;
    NoteNum: IntegerEditor;
    SingleMoney: DecimalEditor;
    NoteMoney: DecimalEditor;
    NoteTime: DateEditor;
    IsWin: BooleanEditor;
    WinMoney: DecimalEditor;
    IsFactionAward: IntegerEditor;
    PlayTypeRadioId: IntegerEditor;
    RebatePro: DecimalEditor;
    RebateProMoney: StringEditor;
    WinNum: IntegerEditor;
    UserId: IntegerEditor;
    NoticeId: IntegerEditor;
    LotteryTime: DateEditor;
    UserRebatePro: DecimalEditor;
    Multiple: IntegerEditor;
    OrderKey: StringEditor;
    CurrencyUnit: DecimalEditor;
    Ratio: IntegerEditor;
    SourceType: StringEditor;
    MemoJson: StringEditor;
    ClientIp: StringEditor;
    RoomId: StringEditor;
    ResultJson: StringEditor;
}

export class PalyInfoForm extends PrefixedContext {
    static formKey = 'BetHistory.PalyInfo';
    private static init: boolean;

    constructor(prefix: string) {
        super(prefix);

        if (!PalyInfoForm.init)  {
            PalyInfoForm.init = true;

            var w0 = StringEditor;
            var w1 = IntegerEditor;
            var w2 = DecimalEditor;
            var w3 = DateEditor;
            var w4 = BooleanEditor;

            initFormType(PalyInfoForm, [
                'PalyCurrentNum', w0,
                'PalyNum', w0,
                'PlayTypeId', w1,
                'LotteryId', w1,
                'UserName', w0,
                'NoteNum', w1,
                'SingleMoney', w2,
                'NoteMoney', w2,
                'NoteTime', w3,
                'IsWin', w4,
                'WinMoney', w2,
                'IsFactionAward', w1,
                'PlayTypeRadioId', w1,
                'RebatePro', w2,
                'RebateProMoney', w0,
                'WinNum', w1,
                'UserId', w1,
                'NoticeId', w1,
                'LotteryTime', w3,
                'UserRebatePro', w2,
                'Multiple', w1,
                'OrderKey', w0,
                'CurrencyUnit', w2,
                'Ratio', w1,
                'SourceType', w0,
                'MemoJson', w0,
                'ClientIp', w0,
                'RoomId', w0,
                'ResultJson', w0
            ]);
        }
    }
}
