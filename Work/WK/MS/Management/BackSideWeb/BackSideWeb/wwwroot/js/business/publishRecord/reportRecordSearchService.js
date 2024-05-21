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
var reportRecordSearchService = (function (_super) {
    __extends(reportRecordSearchService, _super);
    function reportRecordSearchService(pageApiUrlSetting) {
        var _this = _super.call(this, pageApiUrlSetting) || this;
        _this.$startDate = $("#jqStartDate");
        _this.$endDate = $("#jqEndDate");
        var isAllowEmpty = true;
        _this.initStartAndEndDatePicker(_this.$startDate, _this.$endDate, isAllowEmpty);
        return _this;
    }
    reportRecordSearchService.prototype.getInsertViewArea = function () {
        throw new Error("Method not implemented.");
    };
    reportRecordSearchService.prototype.getUpdateViewArea = function () {
        throw new Error("Method not implemented.");
    };
    reportRecordSearchService.prototype.getSubmitData = function () {
        var data = new reportRecordSearchParam();
        data.beginDate = this.$startDate.val();
        data.endDate = this.$endDate.val();
        data.postId = $("#postId").val();
        data.id = $("#reportId").val();
        data.userId = $("#memberNo").val();
        data.postType = $("#jqPostRegionalSelectList").data().value;
        data.status = $("#jqStatusSelectList").data().value;
        data.reportType = $("#jqReportTypeSelectList").data().value;
        return data;
    };
    reportRecordSearchService.prototype.openUpdateView2 = function (link, keyContent) {
        var url = $(link).data('url');
        var area = {
            width: 800,
            height: 600,
        };
        this.openView({
            url: url,
            keyContent: keyContent,
            area: area
        });
    };
    return reportRecordSearchService;
}(baseCRUDService));
