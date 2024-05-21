import { Decorators, EntityDialog } from '@serenity-is/corelib';
import { FrontsideMenuForm, FrontsideMenuRow, FrontsideMenuService } from '../../ServerTypes/ProductManagement';

@Decorators.registerClass('Management.ProductManagement.FrontsideMenuDialog')
export class FrontsideMenuDialog extends EntityDialog<FrontsideMenuRow, any> {
    protected getFormKey() { return FrontsideMenuForm.formKey; }
    protected getIdProperty() { return FrontsideMenuRow.idProperty; }
    protected getLocalTextPrefix() { return FrontsideMenuRow.localTextPrefix; }
    protected getNameProperty() { return FrontsideMenuRow.nameProperty; }
    protected getService() { return FrontsideMenuService.baseUrl; }
    protected getDeletePermission() { return FrontsideMenuRow.deletePermission; }
    protected getInsertPermission() { return FrontsideMenuRow.insertPermission; }
    protected getUpdatePermission() { return FrontsideMenuRow.updatePermission; }

    protected form = new FrontsideMenuForm(this.idPrefix);
}