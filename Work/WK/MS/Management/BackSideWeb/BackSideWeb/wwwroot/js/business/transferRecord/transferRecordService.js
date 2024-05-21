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
var transferRecordService = (function (_super) {
    __extends(transferRecordService, _super);
    function transferRecordService(searchApiUrlSetting) {
        var _this = _super.call(this, searchApiUrlSetting) || this;
        var minStartDate = new Date();
        minStartDate.setMonth(minStartDate.getMonth() - 3);
        var isAllowEmpty = true;
        _this.initStartAndEndDatePicker($('#StartDate'), $('#EndDate'), isAllowEmpty, minStartDate);
        return _this;
    }
    transferRecordService.prototype.validateSubmitData = function () {
        var data = this.submitData;
        if (data.UserID.trim() == '' && data.ProductCode.trim() == '') {
            alert('请输入用户ID或指定产品查询');
            return false;
        }
        return _super.prototype.validateSubmitData.call(this);
    };
    return transferRecordService;
}(baseSearchGridService));
var transferRecordSubmitData = (function (_super) {
    __extends(transferRecordSubmitData, _super);
    function transferRecordSubmitData() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    return transferRecordSubmitData;
}(PagingRequestParam));
