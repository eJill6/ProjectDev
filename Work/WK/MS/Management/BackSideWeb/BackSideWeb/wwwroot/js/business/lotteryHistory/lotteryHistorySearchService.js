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
var lotteryHistorySearchService = (function (_super) {
    __extends(lotteryHistorySearchService, _super);
    function lotteryHistorySearchService(pageApiUrlSetting) {
        var _this = _super.call(this, pageApiUrlSetting) || this;
        _this.$startDate = $("#jqStartDate");
        _this.$endDate = $("#jqEndDate");
        var isAllowEmpty = true;
        _this.initStartAndEndDatePicker(_this.$startDate, _this.$endDate, isAllowEmpty);
        return _this;
    }
    lotteryHistorySearchService.prototype.getSubmitData = function () {
        var data = new lotteryHistorySearchParam();
        var startDate = new Date(this.$startDate.val().toString());
        var endDate = new Date(this.$endDate.val().toString());
        if (!this.$startDate.val().toString()) {
            alert('开始日期不得为空');
            return null;
        }
        if (!this.$endDate.val().toString()) {
            alert('结束日期不得为空');
            return null;
        }
        if (isNaN(startDate.getTime()) || isNaN(endDate.getTime())) {
            alert('日期格式不符');
            return null;
        }
        var dayDifference = (endDate.getTime() - startDate.getTime()) / (1000 * 60 * 60 * 24);
        if (dayDifference > 90) {
            alert('日期超过90天');
            return null;
        }
        if (startDate > endDate) {
            alert('开始日期不能大于结束日期');
            return null;
        }
        data.startDate = this.$startDate.val();
        data.endDate = this.$endDate.val();
        data.LotteryID = $("#jqLotteryTypeSelectList").data().value;
        data.IssueNo = $("#IssueNo").val();
        data.IsLottery = $("#jqIsLotterySelectList").data().value;
        return data;
    };
    return lotteryHistorySearchService;
}(baseSearchGridService));
