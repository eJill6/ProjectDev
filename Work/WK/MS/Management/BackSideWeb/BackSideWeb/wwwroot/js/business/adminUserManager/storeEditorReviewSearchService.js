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
var storeEditorReviewSearchService = (function (_super) {
    __extends(storeEditorReviewSearchService, _super);
    function storeEditorReviewSearchService(searchApiUrlSetting) {
        var _this = _super.call(this, searchApiUrlSetting) || this;
        _this.defaultPageSize = 15;
        _this.$tabSelectorItems = $('.pageTab ul a');
        _this.$startDate = $("#jqStartDate");
        _this.$endDate = $("#jqEndDate");
        var isAllowEmpty = true;
        _this.initStartAndEndDatePicker(_this.$startDate, _this.$endDate, isAllowEmpty);
        return _this;
    }
    storeEditorReviewSearchService.prototype.getSubmitData = function () {
        if (!this.$startDate.val().toString()) {
            alert('开始日期不得为空');
            return null;
        }
        if (!this.$endDate.val().toString()) {
            alert('结束日期不得为空');
            return null;
        }
        var data = new storeEditorReviewSearchParam();
        data.userId = $("#jqUserId").val();
        data.beginDate = this.$startDate.val();
        data.endDate = this.$endDate.val();
        return data;
    };
    storeEditorReviewSearchService.prototype.openEdit = function (link, keyContent) {
        var url = $(link).data('url');
        var area = {
            width: 950,
            height: 700
        };
        this.openView({
            url: url,
            keyContent: keyContent,
            area: area,
            title: '审核'
        });
    };
    storeEditorReviewSearchService.prototype.openDetail = function (link, keyContent) {
        var url = $(link).data('url');
        var area = {
            width: 950,
            height: 700,
        };
        this.openView({
            url: url,
            keyContent: keyContent,
            area: area,
            title: '检视'
        });
    };
    return storeEditorReviewSearchService;
}(baseSearchGridService));
