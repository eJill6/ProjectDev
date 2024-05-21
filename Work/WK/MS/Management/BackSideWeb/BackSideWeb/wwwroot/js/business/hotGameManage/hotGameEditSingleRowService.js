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
var hotGameEditSingleRowService = (function (_super) {
    __extends(hotGameEditSingleRowService, _super);
    function hotGameEditSingleRowService() {
        var _this = _super !== null && _super.apply(this, arguments) || this;
        _this.$imageFile = $("#jqImageFile");
        return _this;
    }
    hotGameEditSingleRowService.prototype.serializeFormData = function ($form) {
        var formObject = this.formUtilService.serializeObject($form);
        var formData = this.formUtilService.objectToFormData(formObject);
        formData.append(this.$imageFile.attr("name"), this.$imageFile[0]["files"][0]);
        return formData;
    };
    hotGameEditSingleRowService.prototype.loadSubGameCodesDropdownPartial = function (key, value) {
        var url = globalVariables.GetUrl("/DoubleLayerDropdown/GetHotGameSubGamesDropdown");
        var dropdownMenuSetting = { "SettingId": "GameCode" };
        $.ajax2({
            url: url,
            type: "POST",
            data: { productCode: value, dropdownMenuSetting: dropdownMenuSetting },
            success: function (response) {
                var $selector = $("#jqGameCodeDiv");
                if (response != null && response.length > 0) {
                    $selector.show();
                    $selector.find("label").next().remove();
                    $selector.find("label")[0].insertAdjacentHTML('afterend', response);
                }
                else {
                    $selector.hide();
                }
            },
            isAutoHideLoading: true,
        });
    };
    return hotGameEditSingleRowService;
}(editSingleRowService));
