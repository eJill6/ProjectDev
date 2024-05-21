import { Decorators, EntityDialog } from '@serenity-is/corelib';
import { PalyInfoForm, PalyInfoRow, PalyInfoService } from '../../ServerTypes/BetHistory';

@Decorators.registerClass('Management.BetHistory.PalyInfoDialog')
export class PalyInfoDialog extends EntityDialog<PalyInfoRow, any> {
    protected getFormKey() { return PalyInfoForm.formKey; }
    protected getIdProperty() { return PalyInfoRow.idProperty; }
    protected getLocalTextPrefix() { return PalyInfoRow.localTextPrefix; }
    protected getNameProperty() { return PalyInfoRow.nameProperty; }
    protected getService() { return PalyInfoService.baseUrl; }
    protected getDeletePermission() { return PalyInfoRow.deletePermission; }
    protected getInsertPermission() { return PalyInfoRow.insertPermission; }
    protected getUpdatePermission() { return PalyInfoRow.updatePermission; }

    protected form = new PalyInfoForm(this.idPrefix);
}