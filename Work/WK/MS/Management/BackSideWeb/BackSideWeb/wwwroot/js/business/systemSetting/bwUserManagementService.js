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
var bwUserManagementService = (function (_super) {
    __extends(bwUserManagementService, _super);
    function bwUserManagementService(pageApiUrlSetting) {
        return _super.call(this, pageApiUrlSetting) || this;
    }
    bwUserManagementService.prototype.openGoogleQRCode = function (link, keyContent) {
        var url = $(link).data('url');
        var area = {
            width: 420,
        };
        this.openView({
            url: url,
            keyContent: keyContent,
            area: area
        });
    };
    return bwUserManagementService;
}(baseCRUDService));
