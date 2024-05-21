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
var optionsSearchService = (function (_super) {
    __extends(optionsSearchService, _super);
    function optionsSearchService(searchApiUrlSetting) {
        var _this = _super.call(this, searchApiUrlSetting) || this;
        _this.defaultPageSize = 1000;
        return _this;
    }
    optionsSearchService.prototype.getInsertViewArea = function () {
        return {
            width: 600,
            height: 480,
        };
    };
    optionsSearchService.prototype.getUpdateViewArea = function () {
        return {
            width: 600,
            height: 480,
        };
    };
    optionsSearchService.prototype.getSubmitData = function () {
        var data = new optionsSearchParam();
        data.postType = $('#jqPostTypeSelectList').data().value;
        data.optionType = $('#jqOptionTypeSelectList').data().value;
        return data;
    };
    return optionsSearchService;
}(baseCRUDService));
var optionsSearchParam = (function (_super) {
    __extends(optionsSearchParam, _super);
    function optionsSearchParam() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    return optionsSearchParam;
}(PagingRequestParam));
