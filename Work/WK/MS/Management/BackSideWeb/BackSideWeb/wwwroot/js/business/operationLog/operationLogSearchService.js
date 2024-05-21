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
var operationLogSearchService = (function (_super) {
    __extends(operationLogSearchService, _super);
    function operationLogSearchService(searchApiUrlSetting) {
        var _this = _super.call(this, searchApiUrlSetting) || this;
        var isAllowEmpty = true;
        _this.initDefaultDatePicker(isAllowEmpty);
        return _this;
    }
    operationLogSearchService.prototype.openOperationLogDetail = function (link, keyContent) {
        var url = $(link).data('url');
        var area = {
            width: 500,
        };
        this.openView({
            url: url,
            keyContent: keyContent,
            area: area,
            title: '日志详细'
        });
    };
    return operationLogSearchService;
}(baseSearchGridService));
