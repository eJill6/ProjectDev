import { Decorators, EntityDialog } from '@serenity-is/corelib';
import { ProfitLossForm, ProfitLossRow, ProfitLossService } from '../../ServerTypes/ProfitlossTest';

@Decorators.registerClass('Management.ProfitlossTest.ProfitLossDialog')
export class ProfitLossDialog extends EntityDialog<ProfitLossRow, any> {
    protected getFormKey() { return ProfitLossForm.formKey; }
    protected getIdProperty() { return ProfitLossRow.idProperty; }
    protected getLocalTextPrefix() { return ProfitLossRow.localTextPrefix; }
    protected getNameProperty() { return ProfitLossRow.nameProperty; }
    protected getService() { return ProfitLossService.baseUrl; }
    protected getDeletePermission() { return ProfitLossRow.deletePermission; }
    protected getInsertPermission() { return ProfitLossRow.insertPermission; }
    protected getUpdatePermission() { return ProfitLossRow.updatePermission; }

    protected form = new ProfitLossForm(this.idPrefix);
}