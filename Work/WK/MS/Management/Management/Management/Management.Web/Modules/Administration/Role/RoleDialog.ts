import { RoleRow, RoleForm, RoleService } from "../";
import { RolePermissionDialog } from "../RolePermission/RolePermissionDialog";
import { Texts } from "../../ServerTypes/Texts"
import { Decorators, EntityDialog, ToolButton } from "@serenity-is/corelib";
import { indexOf } from "@serenity-is/corelib/q";

const editPermissions = "edit-permissions";

@Decorators.registerClass('Management.Administration.RoleDialog')
export class RoleDialog extends EntityDialog<RoleRow, any> {
    protected getFormKey() { return RoleForm.formKey; }
    protected getIdProperty() { return RoleRow.idProperty; }
    protected getLocalTextPrefix() { return RoleRow.localTextPrefix; }
    protected getNameProperty() { return RoleRow.nameProperty; }
    protected getService() { return RoleService.baseUrl; }

    protected form = new RoleForm(this.idPrefix);

    protected getToolbarButtons(): ToolButton[] {
        let buttons = super.getToolbarButtons();

        buttons.splice(indexOf(buttons, item => item.cssClass == "apply-changes-button"), 1);

        buttons.push({
            title: Texts.Site.RolePermissionDialog.EditButton,
            cssClass: editPermissions,
            icon: 'fa-lock text-green',
            onClick: () => {
                new RolePermissionDialog({
                    roleID: this.entity.RoleId,
                    title: this.entity.RoleName
                }).dialogOpen();
            }
        });

        return buttons;
    }

    protected updateInterface() {
        super.updateInterface();

        //TODO:SEAN-role id不存在時，選權限的彈窗會爆炸
        this.toolbar.findButton(editPermissions).toggleClass("disabled", this.isNewOrDeleted());
    }
}