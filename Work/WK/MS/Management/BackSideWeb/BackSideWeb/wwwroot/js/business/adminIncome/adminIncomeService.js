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
var adminIncomeService = (function (_super) {
    __extends(adminIncomeService, _super);
    function adminIncomeService(pageApiUrlSetting) {
        var _this = _super.call(this, pageApiUrlSetting) || this;
        _this.$startDate = $("#jqStartDate");
        _this.$endDate = $("#jqEndDate");
        var isAllowEmpty = true;
        _this.initStartAndEndDatePicker(_this.$startDate, _this.$endDate, isAllowEmpty);
        return _this;
    }
    adminIncomeService.prototype.getInsertViewArea = function () {
        return {
            width: 600,
            height: 280,
        };
    };
    adminIncomeService.prototype.getUpdateViewArea = function () {
        return {
            width: 600,
            height: 355,
        };
    };
    adminIncomeService.prototype.getSubmitData = function () {
        var data = new adminIncomeSearchParam();
        data.PostId = $("#jqPostId").val();
        data.Id = $("#jqId").val();
        data.UserId = $("#jqUserId").val();
        data.ReportId = $("#jqReportId").val();
        data.PostType = $('#jqPostTypeItems').data().value;
        data.Status = $("#jqStatusItems").data().value;
        data.DateTimeType = $("#jqDateTimeTypeItems").data().value;
        data.ApplyIdentity = $("#jqUserIdentitySelectList").data().value;
        data.BeginDate = this.$startDate.val();
        data.EndDate = this.$endDate.val();
        return data;
    };
    adminIncomeService.prototype.openDetail = function (link, keyContent, title) {
        var url = $(link).data('url');
        var area = {
            width: 800,
            height: 600
        };
        this.openView({
            url: url,
            keyContent: keyContent,
            area: area,
            title: title
        });
    };
    adminIncomeService.prototype.openUpdateView2 = function (link, keyContent) {
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
    return adminIncomeService;
}(baseCRUDService));
