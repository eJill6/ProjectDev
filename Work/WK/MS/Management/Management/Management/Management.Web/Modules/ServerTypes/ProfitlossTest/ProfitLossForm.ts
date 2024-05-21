import { IntegerEditor, DateEditor, StringEditor, DecimalEditor, PrefixedContext } from "@serenity-is/corelib";
import { initFormType } from "@serenity-is/corelib/q";

export interface ProfitLossForm {
    UserId: IntegerEditor;
    ProfitLossTime: DateEditor;
    ProfitLossType: StringEditor;
    ProfitLossMoney: DecimalEditor;
    WinMoney: DecimalEditor;
    PrizeMoney: DecimalEditor;
    AllBetMoney: DecimalEditor;
    GameType: StringEditor;
    PlayId: StringEditor;
    Memo: StringEditor;
}

export class ProfitLossForm extends PrefixedContext {
    static formKey = 'ProfitlossTest.ProfitLoss';
    private static init: boolean;

    constructor(prefix: string) {
        super(prefix);

        if (!ProfitLossForm.init)  {
            ProfitLossForm.init = true;

            var w0 = IntegerEditor;
            var w1 = DateEditor;
            var w2 = StringEditor;
            var w3 = DecimalEditor;

            initFormType(ProfitLossForm, [
                'UserId', w0,
                'ProfitLossTime', w1,
                'ProfitLossType', w2,
                'ProfitLossMoney', w3,
                'WinMoney', w3,
                'PrizeMoney', w3,
                'AllBetMoney', w3,
                'GameType', w2,
                'PlayId', w2,
                'Memo', w2
            ]);
        }
    }
}
