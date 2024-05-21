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
var slotGameEditSingleRowService = (function (_super) {
    __extends(slotGameEditSingleRowService, _super);
    function slotGameEditSingleRowService() {
        var _this = _super !== null && _super.apply(this, arguments) || this;
        _this.$imageFile = $("#jqImageFile");
        _this.$isHot = $("#IsHot");
        return _this;
    }
    slotGameEditSingleRowService.prototype.serializeFormData = function ($form) {
        var formObject = this.formUtilService.serializeObject($form);
        var formData = this.formUtilService.objectToFormData(formObject);
        formData.append(this.$imageFile.attr("name"), this.$imageFile[0]["files"][0]);
        formData.set(this.$isHot.attr("name"), this.$isHot[0].checked.toString());
        return formData;
    };
    return slotGameEditSingleRowService;
}(editSingleRowService));
