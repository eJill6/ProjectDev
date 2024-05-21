import { StringEditor, PrefixedContext } from "@serenity-is/corelib";
import { initFormType } from "@serenity-is/corelib/q";

export interface LoginForm {
    Token: StringEditor;
}

export class LoginForm extends PrefixedContext {
    static formKey = 'Membership.Login';
    private static init: boolean;

    constructor(prefix: string) {
        super(prefix);

        if (!LoginForm.init)  {
            LoginForm.init = true;

            var w0 = StringEditor;

            initFormType(LoginForm, [
                'Token', w0
            ]);
        }
    }
}
