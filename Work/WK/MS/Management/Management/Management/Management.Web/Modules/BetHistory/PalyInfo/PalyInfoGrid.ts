import { Decorators, EntityGrid, Widget, LookupEditor, LookupEditorOptions } from '@serenity-is/corelib';
import { PalyInfoColumns, PalyInfoRow, PalyInfoService } from '../../ServerTypes/BetHistory';
import { PalyInfoDialog } from './PalyInfoDialog';
import { Column } from "@serenity-is/sleekgrid";
import { confirm, isEmptyOrNull, isTrimmedEmpty, notifySuccess, outerHtml, localText, trimToEmpty, trimToNull } from "@serenity-is/corelib/q";


@Decorators.registerClass('Management.BetHistory.PalyInfoGrid')
export class PalyInfoGrid extends EntityGrid<PalyInfoRow, any> {
    protected getColumnsKey() { return PalyInfoColumns.columnsKey; }
    protected getDialogType() { return PalyInfoDialog; }
    protected getIdProperty() { return PalyInfoRow.idProperty; }
    protected getInsertPermission() { return PalyInfoRow.insertPermission; }
    protected getLocalTextPrefix() { return PalyInfoRow.localTextPrefix; }
    protected getService() { return PalyInfoService.baseUrl; }

    constructor(container: JQuery) {
        super(container);
    }

    private LotteryType: LookupEditor; 

    //protected getColumns(): Column[] {

    //    var columns: Column[] = [];
    //    columns.push({ field: 'Key', width: 300, sortable: false });

    //    columns.push({
    //        field: 'SourceText',
    //        width: 300,
    //        sortable: false,
    //        format: ctx => {
    //            return outerHtml($('<a/>')
    //                .addClass('source-text')
    //                .text(ctx.value || ''));
    //        }
    //    });

    //    return columns;

    //}

    protected createToolbarExtensions(): void {

        let opt: LookupEditorOptions = {
            lookupKey: 'SystemSettings.LotteryInfo'
        };
        this.LotteryType = Widget.create({
            type: LookupEditor,
            element: el => el.appendTo(this.toolbar.element).attr('placeholder', '--- ' +
                localText('Db.BetHistory.PalyInfo.SelectedLottery') + ' ---'),
            options: opt
        })
    }
}