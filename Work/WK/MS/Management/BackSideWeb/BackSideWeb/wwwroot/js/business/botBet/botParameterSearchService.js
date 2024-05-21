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
var botParameterSearchService = (function (_super) {
    __extends(botParameterSearchService, _super);
    function botParameterSearchService(pageApiUrlSetting) {
        var _this = _super.call(this, pageApiUrlSetting) || this;
        _this.$tabSelectorItems = $('.pageTab ul a');
        _this.isFirstClick = true;
        _this.lotteryPatchTypeSelect = document.getElementById("lotteryPatchType");
        _this.botGroupSelect = document.getElementById("botGroupSelect");
        _this.timeTypeSelect = document.getElementById("timeTypeSelect");
        $(document).ready(function () {
            _this.registerClickEvent();
            _this.$tabSelectorItems.first().click();
            _this.botGroupSelect.onchange = function () {
                _this.updateTimeTypeOptions();
            };
        });
        return _this;
    }
    botParameterSearchService.prototype.getInsertViewArea = function () {
        return {
            width: 600,
            height: 700,
        };
    };
    botParameterSearchService.prototype.getUpdateViewArea = function () {
        return {
            width: 600,
            height: 700,
        };
    };
    botParameterSearchService.prototype.registerClickEvent = function () {
        var _this = this;
        this.$tabSelectorItems.click(function (event) {
            event.preventDefault();
            _this.$tabSelectorItems.removeClass("active");
            $(event.currentTarget).addClass("active");
            var tabId = $(event.currentTarget).attr("id");
            $("#lotteryPatchType").text(tabId);
            _this.updateTimeTypeOptions();
            if (!_this.isFirstClick) {
                var $jqSearchBtn = $(".jqSearchBtn");
                $jqSearchBtn.click();
            }
            else {
                _this.isFirstClick = false;
            }
        });
    };
    botParameterSearchService.prototype.getSubmitData = function () {
        var data = new botParameterSearchServiceParam();
        data.lotteryPatchType = $('#lotteryPatchType').text();
        data.botGroup = this.botGroupSelect.value;
        data.timeType = this.timeTypeSelect.value;
        data.settingGroup = $('#settingGroupSelect').data().value;
        return data;
    };
    botParameterSearchService.prototype.updateTimeTypeOptions = function () {
        var lotteryPatchTypeSelectValue = this.lotteryPatchTypeSelect.innerText;
        var botGroupSelectValue = this.botGroupSelect.value;
        this.timeTypeSelect.innerHTML = '';
        if (botGroupSelectValue === '' || botGroupSelectValue === null || botGroupSelectValue === "全部") {
            this.addOption(this.timeTypeSelect, "全部", '');
            this.addOption(this.timeTypeSelect, 'T1', '0');
            this.addOption(this.timeTypeSelect, 'T2', '1');
            this.addOption(this.timeTypeSelect, 'T3', '2');
            this.addOption(this.timeTypeSelect, 'T4', '3');
            return;
        }
        if (lotteryPatchTypeSelectValue === '1') {
            if (botGroupSelectValue === '2') {
                this.addOption(this.timeTypeSelect, "全部", '');
                this.addOption(this.timeTypeSelect, 'T1', '0');
                this.addOption(this.timeTypeSelect, 'T2', '1');
            }
            else {
                this.addOption(this.timeTypeSelect, "全部", '');
                this.addOption(this.timeTypeSelect, 'T1', '0');
                this.addOption(this.timeTypeSelect, 'T2', '1');
                this.addOption(this.timeTypeSelect, 'T3', '2');
                this.addOption(this.timeTypeSelect, 'T4', '3');
            }
        }
        else if (lotteryPatchTypeSelectValue === '0') {
            if (botGroupSelectValue === '0') {
                this.addOption(this.timeTypeSelect, "全部", '');
                this.addOption(this.timeTypeSelect, 'T1', '0');
                this.addOption(this.timeTypeSelect, 'T2', '1');
                this.addOption(this.timeTypeSelect, 'T3', '2');
                this.addOption(this.timeTypeSelect, 'T4', '3');
            }
            else if (botGroupSelectValue === '1') {
                this.addOption(this.timeTypeSelect, "全部", '');
                this.addOption(this.timeTypeSelect, 'T1', '0');
                this.addOption(this.timeTypeSelect, 'T2', '1');
                this.addOption(this.timeTypeSelect, 'T3', '2');
            }
            else if (botGroupSelectValue === '2') {
                this.addOption(this.timeTypeSelect, "全部", '');
                this.addOption(this.timeTypeSelect, 'T1', '0');
                this.addOption(this.timeTypeSelect, 'T2', '1');
            }
        }
    };
    botParameterSearchService.prototype.addOption = function (selectElement, text, value) {
        var option = document.createElement('option');
        option.text = text;
        option.value = value;
        selectElement.add(option);
    };
    return botParameterSearchService;
}(baseCRUDService));
var botParameterSearchServiceParam = (function (_super) {
    __extends(botParameterSearchServiceParam, _super);
    function botParameterSearchServiceParam() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    return botParameterSearchServiceParam;
}(PagingRequestParam));
