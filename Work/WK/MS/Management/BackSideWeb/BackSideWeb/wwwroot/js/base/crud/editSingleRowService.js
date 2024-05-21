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
var editSingleRowParam = (function (_super) {
    __extends(editSingleRowParam, _super);
    function editSingleRowParam() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    return editSingleRowParam;
}(readSingleRowParam));
var editSingleRowService = (function (_super) {
    __extends(editSingleRowService, _super);
    function editSingleRowService(param) {
        var _this = _super.call(this, param) || this;
        _this.formUtilService = new formUtilService();
        _this.editSingleRowParam = param;
        return _this;
    }
    editSingleRowService.prototype.isFormValid = function ($form) {
        return $form.valid();
    };
    editSingleRowService.prototype.serializeFormData = function ($form) {
        var formData = $form.serialize();
        return formData;
    };
    editSingleRowService.prototype.handleEditResponse = function (response, isAutoHideLoading) {
        var self = this;
        new baseReturnModelService(response).responseHandler(function () {
            new pagerService(window.parent.document).reloadCurrentPage();
            $(self.editSingleRowParam.jqCloseBtnSelector).click();
        }, isAutoHideLoading);
    };
    editSingleRowService.prototype.Init = function () {
        _super.prototype.Init.call(this);
        var $form = $(this.editSingleRowParam.formSelector);
        var $jqSubmitBtnSelector = $(this.editSingleRowParam.jqSubmitBtnSelector);
        $jqSubmitBtnSelector.click(function () { $form.submit(); });
        var self = this;
        $form.submit(function (e) {
            e.preventDefault();
            if (!self.isFormValid($form)) {
                return;
            }
            var formData = self.serializeFormData($form);
            var submitUrl = self.editSingleRowParam.submitUrl;
            var isAutoHideLoading = false;
            var ajaxSetting = {
                type: 'post',
                url: submitUrl,
                data: formData,
                success: function (response) {
                    self.handleEditResponse(response, isAutoHideLoading);
                },
                isAutoHideLoading: isAutoHideLoading,
            };
            if (formData instanceof FormData) {
                ajaxSetting.processData = false;
                ajaxSetting.contentType = false;
            }
            $.ajax2(ajaxSetting);
        });
    };
    return editSingleRowService;
}(readSingleRowService));
