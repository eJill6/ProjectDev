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
var advertisingContentSearchService = (function (_super) {
    __extends(advertisingContentSearchService, _super);
    function advertisingContentSearchService(searchApiUrlSetting) {
        var _this = _super.call(this, searchApiUrlSetting) || this;
        _this.defaultPageSize = 1000;
        return _this;
    }
    advertisingContentSearchService.prototype.getInsertViewArea = function () {
        throw new Error("Method not implemented.");
    };
    advertisingContentSearchService.prototype.getUpdateViewArea = function () {
        return {
            width: 600,
            height: 420,
        };
    };
    advertisingContentSearchService.prototype.getSubmitData = function () {
        var data = new advertisingContentSearchParam();
        return data;
    };
    return advertisingContentSearchService;
}(baseCRUDService));
var advertisingContentSearchParam = (function (_super) {
    __extends(advertisingContentSearchParam, _super);
    function advertisingContentSearchParam() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    return advertisingContentSearchParam;
}(PagingRequestParam));
