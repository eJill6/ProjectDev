import { StringEditor, IntegerEditor, PrefixedContext } from "@serenity-is/corelib";
import { initFormType } from "@serenity-is/corelib/q";

export interface LotteryInfoForm {
    LotteryType: StringEditor;
    TypeUrl: StringEditor;
    GameTypeId: IntegerEditor;
    Priority: IntegerEditor;
    OfficialLotteryUrl: StringEditor;
    NumberTrendUrl: StringEditor;
    Status: IntegerEditor;
    DefaultSec: IntegerEditor;
    AppPriority: IntegerEditor;
    HotNew: IntegerEditor;
    MaxBonusMoney: IntegerEditor;
    Notice: StringEditor;
    RecommendSort: IntegerEditor;
}

export class LotteryInfoForm extends PrefixedContext {
    static formKey = 'SystemSettings.LotteryInfo';
    private static init: boolean;

    constructor(prefix: string) {
        super(prefix);

        if (!LotteryInfoForm.init)  {
            LotteryInfoForm.init = true;

            var w0 = StringEditor;
            var w1 = IntegerEditor;

            initFormType(LotteryInfoForm, [
                'LotteryType', w0,
                'TypeUrl', w0,
                'GameTypeId', w1,
                'Priority', w1,
                'OfficialLotteryUrl', w0,
                'NumberTrendUrl', w0,
                'Status', w1,
                'DefaultSec', w1,
                'AppPriority', w1,
                'HotNew', w1,
                'MaxBonusMoney', w1,
                'Notice', w0,
                'RecommendSort', w1
            ]);
        }
    }
}
