import { Decorators, EntityGrid, DateEditor } from '@serenity-is/corelib';
import { notifyError, notifyWarning, isEmptyOrNull } from "@serenity-is/corelib/q";
export class BaseGrid<TItem> extends EntityGrid<any, any> {
    protected autoSize;
    protected pageload: boolean = false;
    constructor(container: JQuery) {
        super(container);
        var _that = this;
    }
    protected getPagerOptions() {
        let opt = super.getPagerOptions();
        //Options in the dropdown
        opt.rowsPerPageOptions = [20, 100, 500, 2500, 10000, 50000];
        return opt;
    }

    protected ShowLoading() {
        this.ToggleLoading(true);
    }

    protected HideLoading() {
        this.ToggleLoading(false);
    }

    protected ToggleLoading(isShow: boolean) {
        $("#loader").css("display", isShow ? "block" : "none");
    }

    protected createQuickSearchInput() {
        // do nothing 隱藏 QuickSearch
        //Serenity.GridUtils.addQuickSearchInput(this.toolbar.element, this.view, this.getQuickSearchFields(), () => this.persistSettings(null));
    }

    protected getButtons() {
        var buttons = super.getButtons();
        var _that = this;
        var newButtons = [];
        var allowAddButton = false;
        var permissionKeyPrefix = this.getLocalTextPrefix().replace(".", ":");
        var inserPermissionKey = permissionKeyPrefix + ":Insert";

        // 判斷是否有新增按鈕的權限
        //if (Authorization.hasPermission(inserPermissionKey)) {
        //    allowAddButton = true;
        //}

        for (let entry of buttons) {
            var cs = entry.cssClass;

            // 隱藏 refresh、column-picker-button
            if (cs === "refresh-button" ||
                cs === "column-picker-button") {
                continue;
            }
            else if (!allowAddButton && cs === "add-button") {
                // 沒有新增按鈕權限
                continue;
            }

            newButtons.push(entry);
        }

        // add Search button
        newButtons.push({
            title: '查询',
            cssClass: 'search-button',
            onClick: () => {
                _that.refresh();
            }
        });

        return newButtons;
    }

    protected getColumns() {
        var columns = super.getColumns();

        $.each(columns, (i, k) => {
            // column center
            k.headerCssClass = "align-left";
            // 加上框線
            k.cssClass = k.cssClass + " borderlr ";
        });

        return columns;
    }

    protected onViewProcessData(response: any) {
        response = super.onViewProcessData(response);
        var _that = this;

        if (response.Entities.length == 0 && response.TotalCount === 0) {
            notifyWarning("查无资料");
        }

        if (!_that.pageload) {
            _that.pageload = true;
        }

        this.HideLoading();
        return response;
    }

    protected onViewSubmit(): boolean {
        this.ShowLoading();

        return !super.onViewSubmit();
    }

    /**
* 取得 QuickFilter range time filter
*
* @param  {*} columnName -  time column name
* @param  {*} criteria -  this.view.params.Criteria
* @param  {*} defaultDay - default range day
* @return {Serenity.QuickFilter}
*/
    protected getQuickFiltersRangeDate(columnName: string, title: string, defaultDay?: number) {
        let timeInputHtml = '<input type="text" id="{id}" class="_timeQuickFilterTime" style="width: 80px"/>';
        let orderTimeFilter = this.dateRangeQuickFilter(columnName, title);
        orderTimeFilter.init = w => {
            let startDate = new Date(),
                startDateWidget = w as DateEditor,
                startTimeId = columnName + "-StartTime",
                endTimeId = columnName + "-EndTime",
                setDefaultDate = true;

            // todo defaultDay
            if (defaultDay !== undefined && defaultDay === 0) {
                setDefaultDate = false;
            }

            //startDateWidget.element.nextAll('.ui-datepicker-trigger:first')
            //	.after(timeInputHtml.replace('{id}', startTimeId));

            let endDateWidget = w.element.nextAll(".s-DateEditor").getWidget(DateEditor);
            //w.element.nextAll(".s-DateEditor").nextAll('.ui-datepicker-trigger')
            //	.after(timeInputHtml.replace('{id}', endTimeId));

            if (setDefaultDate) {
                startDateWidget.valueAsDate = startDate;
                endDateWidget.valueAsDate = startDate;
            }
        };

        return orderTimeFilter;
    }
}
