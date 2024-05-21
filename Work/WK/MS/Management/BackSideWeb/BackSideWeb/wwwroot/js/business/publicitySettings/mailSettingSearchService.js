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
var mailSettingSearchService = (function (_super) {
    __extends(mailSettingSearchService, _super);
    function mailSettingSearchService(searchApiUrlSetting) {
        var _this = _super.call(this, searchApiUrlSetting) || this;
        _this.defaultPageSize = 1000;
        return _this;
    }
    mailSettingSearchService.prototype.getInsertViewArea = function () {
        throw new Error("Method not implemented.");
    };
    mailSettingSearchService.prototype.getUpdateViewArea = function () {
        return {
            width: 600,
            height: 300,
        };
    };
    mailSettingSearchService.prototype.getSubmitData = function () {
        var data = new mailSettingSearchParam();
        return data;
    };
    return mailSettingSearchService;
}(baseCRUDService));
var mailSettingSearchParam = (function (_super) {
    __extends(mailSettingSearchParam, _super);
    function mailSettingSearchParam() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    return mailSettingSearchParam;
}(PagingRequestParam));
