import { Decorators, EntityGrid } from '@serenity-is/corelib';
import { ProfitLossColumns, ProfitLossRow, ProfitLossService } from '../../ServerTypes/ProfitlossTest';
import { ProfitLossDialog } from './ProfitLossDialog';

@Decorators.registerClass('Management.ProfitlossTest.ProfitLossGrid')
export class ProfitLossGrid extends EntityGrid<ProfitLossRow, any> {
    protected getColumnsKey() { return ProfitLossColumns.columnsKey; }
    protected getDialogType() { return ProfitLossDialog; }
    protected getIdProperty() { return ProfitLossRow.idProperty; }
    protected getInsertPermission() { return ProfitLossRow.insertPermission; }
    protected getLocalTextPrefix() { return ProfitLossRow.localTextPrefix; }
    protected getService() { return ProfitLossService.baseUrl; }

    constructor(container: JQuery) {
        super(container);
    }
    protected getColumns() {
        var columns = super.getColumns();
         columns.splice(columns.length, 1,
            {
                field: "ShowQRCode",
                name: "操作",
                format: ctx => "<button type='button' class='Insert'>新增</button><button type='button' class='update'>修改</button>",
                width: 160,
                minWidth: 160,
                maxWidth: 160
            });

        return columns;
    }
}