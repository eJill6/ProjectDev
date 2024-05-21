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
var adminPostTransactionService = (function (_super) {
    __extends(adminPostTransactionService, _super);
    function adminPostTransactionService(pageApiUrlSetting) {
        var _this = _super.call(this, pageApiUrlSetting) || this;
        _this.$startDate = $("#jqBeginDate");
        _this.$endDate = $("#jqEndDate");
        var isAllowEmpty = true;
        _this.initStartAndEndDatePicker(_this.$startDate, _this.$endDate, isAllowEmpty);
        return _this;
    }
    adminPostTransactionService.prototype.getInsertViewArea = function () {
        return {
            width: 600,
            height: 280,
        };
    };
    adminPostTransactionService.prototype.getUpdateViewArea = function () {
        return {
            width: 600,
            height: 355,
        };
    };
    adminPostTransactionService.prototype.getSubmitData = function () {
        var data = new adminPostTransactionSearchParam();
        data.postId = $('#jqPostId').val();
        data.id = $("#jqId").val();
        data.userId = $("#jqUserId").val();
        data.postType = $("#jqPostType").data().value;
        data.unlockType = $("#jqUnlockType").data().value;
        data.beginDate = this.$startDate.val();
        data.endDate = this.$endDate.val();
        return data;
    };
    return adminPostTransactionService;
}(baseCRUDService));
