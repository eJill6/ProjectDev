var searchGridInitService = (function () {
    function searchGridInitService(searchBtnSelector, filterSelector, searchBtnClick) {
        this.customClearFilterActions = [];
        this.$jqSearchBtn = $(searchBtnSelector);
        this.$jqFilter = $(filterSelector);
        this.$jqSearchBtn.click(function () {
            searchBtnClick();
        });
    }
    searchGridInitService.prototype.clearFilters = function () {
        this.$jqFilter.each(function (i, element) {
            var $element = $(element);
            var defaultValue = $element.attr("defaultValue");
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
        });
    };
    searchGridInitService.prototype.addClearFiltersAction = function (action) {
        this.customClearFilterActions.push(action);
    };
    searchGridInitService.prototype.btnSearch = function () {
        this.$jqSearchBtn.click();
    };
    return searchGridInitService;
}());
