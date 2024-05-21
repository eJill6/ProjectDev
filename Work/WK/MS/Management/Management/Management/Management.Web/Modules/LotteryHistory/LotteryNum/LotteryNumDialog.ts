import { Decorators, EntityDialog } from '@serenity-is/corelib';
import { LotteryNumForm, LotteryNumRow, LotteryNumService } from '../../ServerTypes/LotteryHistory';

@Decorators.registerClass('Management.LotteryHistory.LotteryNumDialog')
export class LotteryNumDialog extends EntityDialog<LotteryNumRow, any> {
    protected getFormKey() { return LotteryNumForm.formKey; }
    protected getIdProperty() { return LotteryNumRow.idProperty; }
    protected getLocalTextPrefix() { return LotteryNumRow.localTextPrefix; }
    protected getNameProperty() { return LotteryNumRow.nameProperty; }
    protected getService() { return LotteryNumService.baseUrl; }
    protected getDeletePermission() { return LotteryNumRow.deletePermission; }
    protected getInsertPermission() { return LotteryNumRow.insertPermission; }
    protected getUpdatePermission() { return LotteryNumRow.updatePermission; }

    protected form = new LotteryNumForm(this.idPrefix);
}