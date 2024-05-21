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
var evaluateRecordSearchService = (function (_super) {
    __extends(evaluateRecordSearchService, _super);
    function evaluateRecordSearchService(pageApiUrlSetting) {
        var _this = _super.call(this, pageApiUrlSetting) || this;
        _this.$startDate = $("#jqStartDate");
        _this.$endDate = $("#jqEndDate");
        var isAllowEmpty = true;
        _this.initStartAndEndDatePicker(_this.$startDate, _this.$endDate, isAllowEmpty);
        return _this;
    }
    evaluateRecordSearchService.prototype.getInsertViewArea = function () {
        throw new Error("Method not implemented.");
    };
    evaluateRecordSearchService.prototype.getUpdateViewArea = function () {
        throw new Error("Method not implemented.");
    };
    evaluateRecordSearchService.prototype.getSubmitData = function () {
        var data = new evaluateRecordSearchParam();
        data.beginDate = this.$startDate.val();
        data.endDate = this.$endDate.val();
        data.postId = $("#postId").val();
        data.id = $("#commentId").val();
        data.userId = $("#memberNo").val();
        data.postType = $("#jqPostRegionalSelectList").data().value;
        data.status = $("#jqCommentStatusSelectList").data().value;
        data.dateTimeType = $("#jqTimeTypeSelectList").data().value;
        return data;
    };
    evaluateRecordSearchService.prototype.openUpdateView2 = function (link, keyContent) {
        var url = $(link).data('url');
        var area = {
            width: 800,
            height: 650,
        };
        this.openView({
            url: url,
            keyContent: keyContent,
            area: area
        });
    };
    return evaluateRecordSearchService;
}(baseCRUDService));
