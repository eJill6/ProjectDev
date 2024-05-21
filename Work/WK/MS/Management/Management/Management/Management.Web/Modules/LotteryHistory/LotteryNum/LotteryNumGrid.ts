import { Decorators, EntityGrid } from '@serenity-is/corelib';
import { tryFirst, first, indexOf, formatNumber, coalesce, trimToNull, parseInteger, notifySuccess, notifyWarning, notifyError, text, Dictionary } from "@serenity-is/corelib/q";
import { Format } from '@serenity-is/corelib/slick';
import { LotteryNumColumns, LotteryNumRow, LotteryNumService } from '../../ServerTypes/LotteryHistory';
import { LotteryNumDialog } from './LotteryNumDialog';



@Decorators.registerClass('Management.LotteryHistory.LotteryNumGrid')
export class LotteryNumGrid extends EntityGrid<LotteryNumRow, any> {
    protected getColumnsKey() { return LotteryNumColumns.columnsKey; }
    protected getDialogType() { return LotteryNumDialog; }
    protected getIdProperty() { return LotteryNumRow.idProperty; }
    protected getInsertPermission() { return LotteryNumRow.insertPermission; }
    protected getLocalTextPrefix() { return LotteryNumRow.localTextPrefix; }
    protected getService() { return LotteryNumService.baseUrl; }

    constructor(container: JQuery) {
        super(container);
    }

    protected getColumns() {
        var columns = super.getColumns();
        var isLottery = first(columns, function (x) { return x.field === LotteryNumRow.Fields.IsLottery; });
        var drawTimeConsuming = first(columns, function (x) { return x.field === LotteryNumRow.Fields.DrawTimeConsuming; });

        isLottery.format = function (ctx) {
            var status = ctx.value;
            var value = status == true ? "已开奖":"未开奖";
            return value;
        };

        drawTimeConsuming.format = function (ctx) {
            if (ctx.value != null) {
                return ctx.value + "秒";
            }
        };

        return columns;
    }
    protected onViewProcessData(response: any) {
        response = super.onViewProcessData(response);
        if (response.TotalCount === 0) {
            notifyWarning("查无资料");
            return;
        }
        return response;
    }

    protected getButtons() {
        var buttons = super.getButtons();
        buttons.splice(indexOf(buttons, x => x.cssClass == "search-button"), 1);
        buttons.splice(indexOf(buttons, x => x.cssClass == "add-button"), 1);
            buttons.push({
                title: '撤单',
                cssClass: 'apply-changes-button',
                icon: 'fa fa-trash',
                onClick: e => this.cancellationClick(),
                separator: true
            });
            buttons.push({
                title: '人工开奖',
                icon: 'fa fa-gears',
                cssClass: 'apply-changes-button',
                onClick: e => this.manualLotteryClick(),
                separator: true
            });
        return buttons;
    }
    private manualLotteryClick() {

    }
    private cancellationClick() {

    }
}