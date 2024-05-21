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
var monthlyUsersSearchService = (function (_super) {
    __extends(monthlyUsersSearchService, _super);
    function monthlyUsersSearchService(pageApiUrlSetting) {
        var _this = _super.call(this, pageApiUrlSetting) || this;
        _this.$startDate = $("#jqStartDate");
        _this.$endDate = $("#jqEndDate");
        _this.setDefaultDateValues();
        return _this;
    }
    monthlyUsersSearchService.prototype.setDefaultDateValues = function () {
        var currentDate = new Date();
        var startMonth = currentDate.getMonth() > 0 ? currentDate.getMonth() - 1 : 11;
        this.$startDate.val(currentDate.getFullYear() + '-' + ('0' + (startMonth + 1)).slice(-2));
        this.$endDate.val(currentDate.getFullYear() + '-' + ('0' + (currentDate.getMonth() + 1)).slice(-2));
    };
    monthlyUsersSearchService.prototype.getSubmitData = function () {
        var data = new monthlyUsersSearchParam();
        data.beginTime = this.$startDate.val();
        data.endTime = this.$endDate.val();
        return data;
    };
    return monthlyUsersSearchService;
}(baseSearchGridService));
