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
var demoCRUDService = (function (_super) {
    __extends(demoCRUDService, _super);
    function demoCRUDService(pageApiUrlSetting) {
        var _this = _super.call(this, pageApiUrlSetting) || this;
        _this.$startDate = $("#StartDate");
        _this.$endDate = $("#EndDate");
        var isAllowEmpty = true;
        _this.initStartAndEndDatePicker(_this.$startDate, _this.$endDate, isAllowEmpty);
        return _this;
    }
    demoCRUDService.prototype.getUpdateViewArea = function () {
        return {
            width: 600,
        };
    };
    demoCRUDService.prototype.openUpdateView2 = function (link, keyContent) {
        var url = $(link).data('url');
        this.openView({
            url: url,
            keyContent: keyContent,
        });
    };
    return demoCRUDService;
}(baseCRUDService));
