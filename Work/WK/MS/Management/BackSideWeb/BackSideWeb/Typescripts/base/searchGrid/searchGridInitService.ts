class searchGridInitService {
    private $jqSearchBtn: any;
    private $jqFilter: any;
    private customClearFilterActions: Function[] = [];

    constructor(searchBtnSelector: string, filterSelector: string, searchBtnClick: Function) {
        this.$jqSearchBtn = $(searchBtnSelector);
        this.$jqFilter = $(filterSelector);

        this.$jqSearchBtn.click(function () {
            searchBtnClick();
        });
    }

    clearFilters(): void {
        this.$jqFilter.each(function (i, element) {
            let $element: JQuery<HTMLElement> = $(element);
            let defaultValue = $element.attr("defaultValue");

            if (defaultValue === undefined) {
                defaultValue = '';
            }

            if ($element.hasClass("jqDropDownMenu")) {
                $element.find(".jqDrowDownMenuItem:first").click();
            }
            else {
                $element.val(defaultValue);
            }
        });

        $.each(this.customClearFilterActions, function (i, action) {
            action();
        })
    }

    addClearFiltersAction(action:Function) {
        this.customClearFilterActions.push(action);
    }

    btnSearch(): void {
        this.$jqSearchBtn.click();
    }
}