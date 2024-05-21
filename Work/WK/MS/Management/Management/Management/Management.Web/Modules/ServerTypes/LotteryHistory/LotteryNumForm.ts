import { DateEditor, StringEditor, IntegerEditor, BooleanEditor, PrefixedContext } from "@serenity-is/corelib";
import { initFormType } from "@serenity-is/corelib/q";

export interface LotteryNumForm {
    CurrentLotteryTime: DateEditor;
    LotteryType: StringEditor;
    CurrentLotteryNum: StringEditor;
    LotteryId: IntegerEditor;
    IssueNo: StringEditor;
    AddTime: DateEditor;
    UpdateTime: DateEditor;
    IsLottery: BooleanEditor;
    Msg: StringEditor;
}

export class LotteryNumForm extends PrefixedContext {
    static formKey = 'LotteryHistory.LotteryNum';
    private static init: boolean;

    constructor(prefix: string) {
        super(prefix);

        if (!LotteryNumForm.init)  {
            LotteryNumForm.init = true;

            var w0 = DateEditor;
            var w1 = StringEditor;
            var w2 = IntegerEditor;
            var w3 = BooleanEditor;

            initFormType(LotteryNumForm, [
                'CurrentLotteryTime', w0,
                'LotteryType', w1,
                'CurrentLotteryNum', w1,
                'LotteryId', w2,
                'IssueNo', w1,
                'AddTime', w0,
                'UpdateTime', w0,
                'IsLottery', w3,
                'Msg', w1
            ]);
        }
    }
}
