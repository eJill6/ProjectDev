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
var LiveGameManageDataType;
(function (LiveGameManageDataType) {
    LiveGameManageDataType[LiveGameManageDataType["GameCenter"] = 1] = "GameCenter";
    LiveGameManageDataType[LiveGameManageDataType["DirectPlay"] = 2] = "DirectPlay";
    LiveGameManageDataType[LiveGameManageDataType["MiseLottery"] = 3] = "MiseLottery";
})(LiveGameManageDataType || (LiveGameManageDataType = {}));
var liveGameEditSingleRowService = (function (_super) {
    __extends(liveGameEditSingleRowService, _super);
    function liveGameEditSingleRowService() {
        var _this = _super !== null && _super.apply(this, arguments) || this;
        _this.$imageFile = $("#jqImageFile");
        return _this;
    }
    liveGameEditSingleRowService.prototype.serializeFormData = function ($form) {
        var formObject = this.formUtilService.serializeObject($form);
        var formData = this.formUtilService.objectToFormData(formObject);
        formData.append(this.$imageFile.attr("name"), this.$imageFile[0]["files"][0]);
        return formData;
    };
    liveGameEditSingleRowService.prototype.dataTypeChange = function (text, value) {
        var attr = "datatype_".concat(value);
        var $jqDataTypePanel = $(".jqDataTypePanel");
        $jqDataTypePanel.hide();
        var $jqDataTypePanelVisible = $jqDataTypePanel.filter("[".concat(attr, "='on']"));
        $jqDataTypePanelVisible.show();
        if (!liveGameEditSingleRowService.isLoadingData()) {
            $jqDataTypePanel.find("input").not("button.dropdown_toggle input").val("");
            $jqDataTypePanel.find("input[data-val-number],input[type='number']").not("button.dropdown_toggle input").val("0");
            $jqDataTypePanelVisible.find("button.dropdown_toggle:visible").each(function (index, value) {
                new dropDownService("#".concat(value.id)).setSelectedIndex(0);
            });
            if (parseInt(value) != LiveGameManageDataType.MiseLottery) {
                $("#LotteryId").val("0");
            }
        }
        if (!$("#ProductCode").is(":visible")) {
            liveGameEditSingleRowService.setLoadingData(false);
            return;
        }
        var liveGameManageDataTypeValue = value;
        var dropdownMenuSetting = {
            SettingId: "GameCode",
            Callback: "window.editSingleRowService.productCodeChange"
        };
        var url = globalVariables.GetUrl("/DoubleLayerDropdown/GetGameDropdown");
        var dropDownServ = new dropDownService("#ProductCode");
        $.ajax2({
            url: url,
            type: "POST",
            data: { liveGameManageDataTypeValue: liveGameManageDataTypeValue, dropdownMenuSetting: dropdownMenuSetting },
            success: function (response) {
                dropDownServ.updateMenu(response);
                var dataModel = window["dataModel"];
                if (liveGameEditSingleRowService.isLoadingData() && dataModel) {
                    if (!dropDownServ.setSelectedValue(dataModel.ProductCode)) {
                        dropDownServ.setSelectedIndex(0);
                    }
                }
                else {
                    dropDownServ.setSelectedIndex(0);
                }
            }
        });
    };
    liveGameEditSingleRowService.prototype.productCodeChange = function (text, value) {
        var url = globalVariables.GetUrl("/DoubleLayerDropdown/GetSubGameDropdown");
        var dataTypeDropDownServ = new dropDownService("#LiveGameManageDataTypeValue");
        var gameCodeDropDownServ = new dropDownService("#GameCode");
        var liveGameManageDataTypeValue = parseInt(dataTypeDropDownServ.getSelectedValue());
        var productCode = value;
        if (productCode === '') {
            gameCodeDropDownServ.clearMenu();
            $("#jqGameCodeDiv").hide();
            liveGameEditSingleRowService.setLoadingData(false);
            return;
        }
        var self = this;
        var dropdownMenuSetting = {
            SettingId: "GameCode",
        };
        $.ajax2({
            url: url,
            type: "POST",
            data: { liveGameManageDataTypeValue: liveGameManageDataTypeValue, productCode: productCode, dropdownMenuSetting: dropdownMenuSetting },
            success: function (response) {
                var menuCount = gameCodeDropDownServ.updateMenu(response);
                $("#jqGameCodeDiv").toggle(menuCount > 0);
                var dataModel = window["dataModel"];
                if (liveGameEditSingleRowService.isLoadingData() && dataModel) {
                    if (!gameCodeDropDownServ.setSelectedValue(dataModel.GameCode)) {
                        gameCodeDropDownServ.setSelectedIndex(0);
                    }
                }
                else {
                    gameCodeDropDownServ.setSelectedIndex(0);
                }
                liveGameEditSingleRowService.setLoadingData(false);
            }
        });
    };
    liveGameEditSingleRowService.setLoadingData = function (isLoadingData) {
        window["isLoadingData"] = isLoadingData;
    };
    liveGameEditSingleRowService.isLoadingData = function () {
        return window["isLoadingData"];
    };
    return liveGameEditSingleRowService;
}(editSingleRowService));
$(function () {
    var dropDownServ = new dropDownService("#LiveGameManageDataTypeValue");
    window["isLoadingData"] = true;
    dropDownServ.triggerChange();
});
