import { Decorators, EntityDialog, ToolButton } from "@serenity-is/corelib";
import { indexOf } from "@serenity-is/corelib/q";
import { UserForm, UserRow, UserService } from "../";

@Decorators.registerClass()
export class UserDialog extends EntityDialog<UserRow, any> {
    protected getFormKey() { return UserForm.formKey; }
    protected getIdProperty() { return UserRow.idProperty; }
    protected getIsActiveProperty() { return UserRow.isActiveProperty; }
    protected getLocalTextPrefix() { return UserRow.localTextPrefix; }
    protected getNameProperty() { return UserRow.nameProperty; }
    protected getService() { return UserService.baseUrl; }

    protected form = new UserForm(this.idPrefix);

    constructor() {
        super();

        this.form.Password.addValidationRule(this.uniqueName, e => {
            if (this.form.Password.value.length < 7)
                return "Password must be at least 7 characters!";
        });

        this.form.PasswordConfirm.addValidationRule(this.uniqueName, e => {

            if (this.form.Password.value != this.form.PasswordConfirm.value)
                return "The passwords entered doesn't match!";

        });
    }

    protected getToolbarButtons(): ToolButton[] {
        let buttons = super.getToolbarButtons();
        buttons.splice(indexOf(buttons, item => item.cssClass == "apply-changes-button"), 1);

        return buttons;
    }

    protected updateInterface() {
        super.updateInterface();
        this.toolbar.findButton("edit-permissions-button").toggleClass("disabled", this.isNewOrDeleted());
    }

    protected afterLoadEntity() {
        super.afterLoadEntity();

        // these fields are only required in new record mode
        this.form.Password.element.toggleClass('required', this.isNew())
            .closest('.field').find('sup').toggle(this.isNew());
        this.form.PasswordConfirm.element.toggleClass('required', this.isNew())
            .closest('.field').find('sup').toggle(this.isNew());
    }
}
