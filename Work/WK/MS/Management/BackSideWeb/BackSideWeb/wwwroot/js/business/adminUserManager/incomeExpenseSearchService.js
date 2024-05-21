var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (Object.prototype.hasOwnProperty.call(b, p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        if (typeof b !== "function" && b !== null)
            throw new TypeError("Class extends value " + String(b) + " is not a constructor or null");
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var incomeExpenseSearchService = (function (_super) {
    __extends(incomeExpenseSearchService, _super);
    function incomeExpenseSearchService(searchApiUrlSetting) {
        var _this = _super.call(this, searchApiUrlSetting) || this;
        _this.defaultPageSize = 15;
        _this.$startDate = $("#jqStartDate");
        _this.$endDate = $("#jqEndDate");
        var isAllowEmpty = false;
        _this.initStartAndEndDatePicker(_this.$startDate, _this.$endDate, isAllowEmpty);
        return _this;
    }
    incomeExpenseSearchService.prototype.getSubmitData = function () {
        var postType = $('#jqPostTypeSelectList').val() < 1 ? null : $('#jqPostTypeSelectList').val();
        var category = $('#jqCategorySelectList').val() == -1 ? null : $('#jqCategorySelectList').val();
        var data = new incomeExpenseSearchParam();
        data.id = $("#jqId").val();
        data.userId = $("#jqUserId").val();
        data.category = category;
        data.postType = postType;
        data.beginDate = this.$startDate.val();
        data.endDate = this.$endDate.val();
        return data;
    };
    return incomeExpenseSearchService;
}(baseSearchGridService));
