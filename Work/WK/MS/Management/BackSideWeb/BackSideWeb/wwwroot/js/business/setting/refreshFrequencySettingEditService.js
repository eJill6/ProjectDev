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
var refreshFrequencySettingEditService = (function (_super) {
    __extends(refreshFrequencySettingEditService, _super);
    function refreshFrequencySettingEditService(param) {
        return _super.call(this, param) || this;
    }
    refreshFrequencySettingEditService.prototype.initControlEvent = function (submitValueSelector, radiosSelector) {
        this.submitValueSelector = submitValueSelector;
        var $intervalSeconds = $(submitValueSelector);
        var self = this;
        var $radioButtons = $(radiosSelector);
        $.each($radioButtons, function (index, radio) {
            var $radioButton = $(radio);
            var refElementId = $radioButton.attr("refElementId");
            if (refElementId === '') {
                $radioButton.click(function (event) {
                    var seconds = Number($(event.target).val());
                    self.setIntervalSeconds($intervalSeconds, seconds);
                });
            }
            else {
                var setIntervalSecondsFromTextBox_1 = function ($radio, $textBox) {
                    if (!$radio.prop("checked")) {
                        return;
                    }
                    var minutes = Number($textBox.val());
                    self.setIntervalSeconds($intervalSeconds, minutes * 60);
                };
                var $customTextBox_1 = $("#".concat(refElementId));
                $radioButton.click(function () {
                    setIntervalSecondsFromTextBox_1($radioButton, $customTextBox_1);
                });
                $customTextBox_1.change(function () {
                    setIntervalSecondsFromTextBox_1($radioButton, $customTextBox_1);
                });
            }
        });
    };
    refreshFrequencySettingEditService.prototype.initCustomizedTextBox = function (validMessage) {
        $.validator.addMethod("customizedInterval", this.validCustomizedTextBox);
        $.each($("input[type='radio'][refElementId!='']"), function (i, radio) {
            var refElementId = $(radio).attr("refElementId");
            var $element = $("#".concat(refElementId));
            $element.rules("add", {
                customizedInterval: true,
                messages: {
                    customizedInterval: validMessage
                }
            });
            var $jqCustomizedIntervalValidMsg = $("<span class='field-validation-valid' data-valmsg-for='".concat($element.attr("name"), "' data-valmsg-replace='true'></span>"));
            $element.closest("td").append($jqCustomizedIntervalValidMsg);
            $element.onlyNumber();
        });
    };
    refreshFrequencySettingEditService.prototype.handleEditResponse = function (response, isAutoHideLoading) {
        if (response.isSuccess) {
            var intervalSeconds = Number($(this.submitValueSelector).val());
            parent.window.services.searchGridService.refreshSearchResult(intervalSeconds);
        }
        var self = this;
        new baseReturnModelService(response).responseHandler(function () {
            $(self.editSingleRowParam.jqCloseBtnSelector).click();
        }, isAutoHideLoading);
    };
    refreshFrequencySettingEditService.prototype.setIntervalSeconds = function ($intervalSeconds, seconds) {
        $intervalSeconds.val(seconds);
    };
    refreshFrequencySettingEditService.prototype.validCustomizedTextBox = function (value, element) {
        var $element = $(element);
        var elementId = $element.attr("id");
        var $radio = $element.closest("table").find("input[refElementId='".concat(elementId, "']"));
        if (($radio.prop("checked") && $.trim(value).length == 0)) {
            return false;
        }
        return true;
    };
    return refreshFrequencySettingEditService;
}(editSingleRowService));
