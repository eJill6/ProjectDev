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
var postOverviewService = (function (_super) {
    __extends(postOverviewService, _super);
    function postOverviewService(searchApiUrlSetting) {
        return _super.call(this, searchApiUrlSetting) || this;
    }
    postOverviewService.prototype.updateGridContent = function (response, htmlContents, self) {
        var $response = $(response);
        for (var i = 1; i <= 5; i++) {
            var $jqBody = $response.find(".jqBody".concat(i));
            var bodyHtml = $jqBody.html();
            $("#jqGridContent".concat(i)).html(bodyHtml);
        }
    };
    return postOverviewService;
}(baseSearchGridService));
