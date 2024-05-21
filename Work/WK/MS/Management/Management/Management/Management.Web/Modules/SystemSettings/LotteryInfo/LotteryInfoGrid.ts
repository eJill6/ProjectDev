import { Decorators, EntityGrid, EnumEditor, ListRequest } from '@serenity-is/corelib';
import { LotteryInfoColumns, LotteryInfoRow, LotteryInfoService } from '../../ServerTypes/SystemSettings';
import { LotteryInfoDialog } from './LotteryInfoDialog';
import { BaseGrid } from '../../Common/BaseGrid';
import { first, indexOf, formatNumber, coalesce, trimToNull, parseInteger, notifySuccess, notifyWarning, notifyError, text, Dictionary } from "@serenity-is/corelib/q";
import { openGridDialog } from '../../Common/DialogUtils'
export enum LotteryInfoEnum {
    "WEB排序",
    "APP排序"
}
class DictionaryParm {
    [index: string]: string;
}

//@Decorators.registerClass('Management.SystemSettings.LotteryInfoGrid')
export class LotteryInfoGrid extends BaseGrid<LotteryInfoRow> {
    protected getColumnsKey() { return LotteryInfoColumns.columnsKey; }
    protected getDialogType() { return LotteryInfoDialog; }
    protected getIdProperty() { return LotteryInfoRow.idProperty; }
    protected getInsertPermission() { return LotteryInfoRow.insertPermission; }
    protected getLocalTextPrefix() { return LotteryInfoRow.localTextPrefix; }
    protected getService() { return LotteryInfoService.baseUrl; }
    private pendingChanges: Dictionary<any> = {};

    constructor(container: JQuery) {
        super(container);
        this.slickContainer.on('change', '.edit:input', (e) => this.inputsChange(e));
        this.slickContainer.on('change', '.editSwitch:input', (e) => this.switchChange(e));
    }
    protected getColumns() {
        var columns = super.getColumns();
        var IsOpen = first(columns, function (x) { return x.field === LotteryInfoRow.Fields.Status; });
        var LotteryType = first(columns, function (x) { return x.field === LotteryInfoRow.Fields.LotteryType; });
        var num = ctx => this.numericInputFormatter(ctx);
        LotteryType.format = function (ctx) {
            var html = `<a style='cursor: pointer;' class="inline-action edit-row fa " title="edit">${ctx.value}</a>`;
            return html;
        };
        IsOpen.format = function (ctx) {
            var status = ctx.value;
            var checked = "";
            if (status == 1) {
                checked = "checked";
                ctx.value = 0;
            }
            else
                ctx.value = 1;
            var html = `
<label class="switch">
  <input class="editSwitch" type="checkbox" ${checked}>
  <span class="slider round"></span>
</label>
`;
            return html;
        };
        first(columns, x => x.field === LotteryInfoRow.Fields.WebSeq).format = num;
        first(columns, x => x.field === LotteryInfoRow.Fields.AppSeq).format = num;
        first(columns, x => x.field === LotteryInfoRow.Fields.MaxBonusMoney).format = num;
        first(columns, x => x.field === LotteryInfoRow.Fields.CustomMoney).format = num;
        return columns;
    }

    protected getQuickFilters() {
        let filters = super.getQuickFilters();
        var columns = super.getColumns();
        //var TimeName = (first(columns, function (x) { return x.field === fld.HotNew; })).name;
        //let TransactionTime = this.getQuickFiltersRangeDate("", TimeName);
        //filters.push(TransactionTime);
        filters.push({
            field: "SortType",
            type: EnumEditor,
            options: {
                enumType: LotteryInfoEnum
            },
            title: "排序依据",
            init: (w) => {
                w.element.getWidget(EnumEditor).value = "0";
            }
        });
        return filters;
    }
    protected getButtons() {
        var buttons = super.getButtons();
        buttons.splice(indexOf(buttons, x => x.cssClass == "search-button"), 1);
        buttons.push({
            title: '提交',
            cssClass: 'apply-changes-button disabled',
            onClick: e => this.saveClick(),
            separator: true
        });
        return buttons;
    }
    //進入頁面直接取資料
    protected onViewSubmit() {
        this.ToggleLoading(true);
        let request = this.view.params as ListRequest;
        var sortTypeName = this.toolbar.element.find("#select2-chosen-1")[0].innerText;
        var sortType;
        if (sortTypeName == "APP排序") {
            sortType = "AppPriority";
        }
        else {
            sortType = "Priority";
        }
        var parameters = new DictionaryParm();
        parameters["SortType"] = sortType;
        request.EqualityFilter = parameters;
        return true;
    }
    protected onViewProcessData(response: any) {
        this.pendingChanges = {};
        this.setSaveButtonState();
        response = super.onViewProcessData(response);
        if (response.TotalCount === 0) {
            notifyWarning("查无资料");
            return;
        }
        return response;
    }
    private numericInputFormatter(ctx) {
        var klass = 'edit numeric';
        var item = ctx.item as LotteryInfoRow;
        var pending = this.pendingChanges[item.LotteryID];

        if (pending && pending[ctx.column.field] !== undefined) {
            klass += ' dirty';
        }

        var value = this.getEffectiveValue(item, ctx.column.field) as number;

        return "<input type='text' class='" + klass +
            "' data-field='" + ctx.column.field +
            "' value='" + formatNumber(value, '0.##') + "'/>";
    }
    private inputsChange(e: JQueryEventObject) {
        var cell = this.slickGrid.getCellFromEvent(e);
        var item = this.itemAt(cell.row);
        var input = $(e.target);
        var field = input.data('field');
        var text = coalesce(trimToNull(input.val()), '0');
        var pending = this.pendingChanges[item.LotteryID];

        var effective = this.getEffectiveValue(item, field);
        var oldText: string;
        if (input.hasClass("numeric"))
            oldText = formatNumber(effective, '0.##');
        else
            oldText = effective as string;

        var value;
        if (field === 'WebSeq' || field === 'AppSeq') {
            var i = parseInteger(text);
            if (isNaN(i) || i > 999 || i < 1) {
                notifyWarning('须在 1 ~ 999 之间', '', null);
                input.val(oldText);
                input.focus();
                return;
            }
            value = i;
        }
        else if (field === 'MaxBonusMoney' || field === 'CustomMoney') {
            console.log(text);
            var i = parseInteger(text);
            console.log(i);
            if (isNaN(i) || i > 99999 || i < 2) {
                notifyWarning('须在 2 ~ 99,999 之间');
                input.val(oldText);
                input.focus();
                return;
            }
            value = i;
        }
        else
            value = text;

        if (!pending) {
            this.pendingChanges[item.LotteryID] = pending = {};
        }

        pending[field] = value;
        item[field] = value;
        this.view.refresh();

        if (input.hasClass("numeric"))
            value = formatNumber(value, '0.##');

        input.val(value).addClass('dirty');

        this.setSaveButtonState();
    }

    private switchChange(e: JQueryEventObject) {
        var _that = this;
        var cell = this.slickGrid.getCellFromEvent(e);
        var item = this.itemAt(cell.row);
        var lotteryId = item.LotteryID;
        var status = item.Status === 1 ? 0 : 1;

        LotteryInfoService.UpdateLotteryStatus({
            Key: lotteryId,
            Value: status.toString()
        }, response => {
            if (response.Success) {
                _that.refresh();
                window.setTimeout(() => notifySuccess("成功"), 30);
            }
            else {
                window.setTimeout(() => notifyError(response.Message), 30);
            };
        })
    }


    private setSaveButtonState() {
        this.toolbar.findButton('apply-changes-button').toggleClass('disabled',
            Object.keys(this.pendingChanges).length === 0);
    }
    private getEffectiveValue(item, field): any {
        var pending = this.pendingChanges[item.LotteryID];
        if (pending && pending[field] !== undefined) {
            return pending[field];
        }

        return item[field];
    }
    // 移除分頁功能
    protected usePager() {
        return false;
    }
    protected onClick(e: JQueryEventObject, row: number, cell: number)
    {
        super.onClick(e, row, cell);
        var _that = this;
        if (e.isDefaultPrevented())
            return;

        var item = this.itemAt(row);
        var target = $(e.target);
        var lotteryId = item.LotteryID;

        if (target.hasClass('edit-row')) {
            let dialog = new LotteryInfoDialog(lotteryId);
            openGridDialog(dialog);
        }
    }
    private saveClick() {
        if (Object.keys(this.pendingChanges).length === 0) {
            notifyWarning("没有异动的资料");
            return;
        }
        var _that = this;
        // 使用 Object.keys() 取得所有鍵
        const keys = Object.keys(this.pendingChanges);

        // 建立 MyRow 物件的陣列
        const datas = keys.map(key => {
            const LotteryID = key;
            const { CustomMoney, MaxBonusMoney, WebSeq, AppSeq } = this.pendingChanges[key];
            return { LotteryID, CustomMoney, MaxBonusMoney, WebSeq, AppSeq };
        });
        LotteryInfoService.Update({
            items: datas
        }, response => {
            if (response.Success) {
                _that.refresh();
                window.setTimeout(() => notifySuccess("成功"), 30);
            }
            else {
                window.setTimeout(() => notifyError(response.Message), 30);
            };
        }
        )
    }
}
