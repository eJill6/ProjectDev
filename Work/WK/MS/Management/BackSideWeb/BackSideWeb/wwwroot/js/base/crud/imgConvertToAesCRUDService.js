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
var imgConvertToAesCRUDService = (function (_super) {
    __extends(imgConvertToAesCRUDService, _super);
    function imgConvertToAesCRUDService() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    imgConvertToAesCRUDService.prototype.doAfterSearch = function (htmlContents, self) {
        _super.prototype.doAfterSearch.call(this, htmlContents, self);
        var aesService = new decryptoService();
        aesService.fetchAllAESImage();
    };
    return imgConvertToAesCRUDService;
}(baseCRUDService));
