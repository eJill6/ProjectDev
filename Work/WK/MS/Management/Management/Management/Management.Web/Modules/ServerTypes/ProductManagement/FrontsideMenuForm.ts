import { StringEditor, IntegerEditor, BooleanEditor, DateEditor, PrefixedContext } from "@serenity-is/corelib";
import { initFormType } from "@serenity-is/corelib/q";

export interface FrontsideMenuForm {
    MenuName: StringEditor;
    EngName: StringEditor;
    PicName: StringEditor;
    ProductCode: StringEditor;
    GameCode: StringEditor;
    Type: IntegerEditor;
    Sort: IntegerEditor;
    AppSort: IntegerEditor;
    Url: StringEditor;
    IsActive: BooleanEditor;
    CreateDate: DateEditor;
    CreateUser: StringEditor;
    UpdateDate: DateEditor;
    UpdateUser: StringEditor;
}

export class FrontsideMenuForm extends PrefixedContext {
    static formKey = 'ProductManagement.FrontsideMenu';
    private static init: boolean;

    constructor(prefix: string) {
        super(prefix);

        if (!FrontsideMenuForm.init)  {
            FrontsideMenuForm.init = true;

            var w0 = StringEditor;
            var w1 = IntegerEditor;
            var w2 = BooleanEditor;
            var w3 = DateEditor;

            initFormType(FrontsideMenuForm, [
                'MenuName', w0,
                'EngName', w0,
                'PicName', w0,
                'ProductCode', w0,
                'GameCode', w0,
                'Type', w1,
                'Sort', w1,
                'AppSort', w1,
                'Url', w0,
                'IsActive', w2,
                'CreateDate', w3,
                'CreateUser', w0,
                'UpdateDate', w3,
                'UpdateUser', w0
            ]);
        }
    }
}
