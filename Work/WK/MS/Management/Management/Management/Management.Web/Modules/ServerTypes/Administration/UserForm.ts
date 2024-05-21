import { StringEditor, LookupEditor, PasswordEditor, PrefixedContext } from "@serenity-is/corelib";
import { initFormType } from "@serenity-is/corelib/q";

export interface UserForm {
    Username: StringEditor;
    Roles: LookupEditor;
    Password: PasswordEditor;
    PasswordConfirm: PasswordEditor;
}

export class UserForm extends PrefixedContext {
    static formKey = 'Administration.User';
    private static init: boolean;

    constructor(prefix: string) {
        super(prefix);

        if (!UserForm.init)  {
            UserForm.init = true;

            var w0 = StringEditor;
            var w1 = LookupEditor;
            var w2 = PasswordEditor;

            initFormType(UserForm, [
                'Username', w0,
                'Roles', w1,
                'Password', w2,
                'PasswordConfirm', w2
            ]);
        }
    }
}
