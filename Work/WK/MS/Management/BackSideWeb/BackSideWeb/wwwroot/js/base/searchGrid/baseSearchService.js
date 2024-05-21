var htmlSearchContent = (function () {
    function htmlSearchContent() {
    }
    return htmlSearchContent;
}());
var intervalSetting = (function () {
    function intervalSetting() {
    }
    return intervalSetting;
}());
var baseSearchService = (function () {
    function baseSearchService(searchApiUrlSetting, htmlSearchContent) {
        if (htmlSearchContent === void 0) { htmlSearchContent = { $contentBody: $('#jqContent') }; }
        this.validatePairDateFieldSettings = [];
        this.searchApiUrlSetting = searchApiUrlSetting;
        this.htmlSearchContent = htmlSearchContent;
        this.intervalSetting = new intervalSetting();
    }
    baseSearchService.prototype.search = function () {
        this.submitData = this.getSubmitData();
        this.doSearch(this.submitData);
    };
    baseSearchService.prototype.refreshSearchResult = function (intervalSeconds) {
        this.clearIntervalId();
        if (typeof intervalSeconds !== "number" || intervalSeconds === 0) {
            return;
        }
        var self = this;
        this.intervalSetting.seconds = intervalSeconds;
        this.intervalSetting.id = setInterval(function () {
            self.doRefreshSearchResult();
        }, this.intervalSetting.seconds * 1000);
    };
    baseSearchService.prototype.doRefreshSearchResult = function () {
        this.search();
    };
    baseSearchService.prototype.clearIntervalId = function () {
        if (this.intervalSetting.id === undefined) {
            return;
        }
        clearInterval(this.intervalSetting.id);
        this.intervalSetting.id = undefined;
    };
    baseSearchService.prototype.doSearch = function (submitData) {
        if (!this.validateSubmitData()) {
            this.submitData = this.lastSubmitData;
            return;
        }
        var htmlContents = this.getHtmlContents();
        var updateGridContent = this.updateGridContent;
        var doAfterSearch = this.doAfterSearch;
        var self = this;
        $.ajax2({
            url: this.searchApiUrlSetting.searchApiUrl,
            type: "POST",
            data: submitData,
            success: function (response) {
                self.lastSubmitData = self.submitData;
                updateGridContent(response, htmlContents, self);
                doAfterSearch(htmlContents, self);
                self.refreshSearchResult(self.intervalSetting.seconds);
            }
        });
    };
    baseSearchService.prototype.getHtmlContents = function () {
        return this.htmlSearchContent;
    };
    baseSearchService.prototype.updateGridContent = function (response, htmlContents, self) {
        htmlContents.$contentBody.html(response);
    };
    baseSearchService.prototype.validateSubmitData = function () {
        for (var _i = 0, _a = this.validatePairDateFieldSettings; _i < _a.length; _i++) {
            var setting = _a[_i];
            if (!this.validatePairDateField(setting)) {
                return false;
            }
        }
        return true;
    };
    baseSearchService.prototype.validatePairDateField = function (validatePairDateFieldSetting) {
        var startDateString = validatePairDateFieldSetting[0].val().toString();
        var endDateString = validatePairDateFieldSetting[1].val().toString();
        var isAllowEmpty = validatePairDateFieldSetting[2];
        var minDate = validatePairDateFieldSetting[3];
        if (!this.validateSingleDateField(startDateString, isAllowEmpty, minDate)
            || !this.validateSingleDateField(endDateString, isAllowEmpty, minDate)) {
            return false;
        }
        var startDateValue = new Date(startDateString);
        var endDateValue = new Date(endDateString);
        if (startDateValue > endDateValue) {
            alert('日期格式不符');
            return false;
        }
        return true;
    };
    baseSearchService.prototype.validateSingleDateField = function (dateString, isAllowEmpty, minDate) {
        if ((dateString && !$.isValidDateStringFormat(dateString))
            || (!isAllowEmpty && !dateString)) {
            alert('日期格式不符');
            return false;
        }
        var minDateValue = minDate ? minDate.setHours(0, 0, 0, 0) : new Date(0).setHours(0, 0, 0, 0);
        var maxDateValue = new Date().setHours(0, 0, 0, 0);
        var dateValue = new Date(dateString).setHours(0, 0, 0, 0);
        if (dateValue < minDateValue || dateValue > maxDateValue) {
            alert('输入的日期已超过可搜查的时间范围');
            return false;
        }
        return true;
    };
    baseSearchService.prototype.initStartAndEndDatePicker = function ($startDateSelector, $endDateSelector, isAllowEmpty, minStartDate) {
        var isReadonly = false;
        $.setStartAndEndDatePicker($startDateSelector, $endDateSelector, isReadonly, minStartDate);
        $startDateSelector.attr("defaultValue", $startDateSelector.val().toString());
        $endDateSelector.attr("defaultValue", $endDateSelector.val().toString());
        this.setValidatePairDateFieldSettings($startDateSelector, $endDateSelector, isAllowEmpty, minStartDate);
    };
    baseSearchService.prototype.setValidatePairDateFieldSettings = function ($startDateSelector, $endDateSelector, isAllowEmpty, minStartDate) {
        this.validatePairDateFieldSettings.push([$startDateSelector, $endDateSelector, isAllowEmpty, minStartDate]);
    };
    baseSearchService.prototype.initDefaultDatePicker = function (isAllowEmpty) {
        var self = this;
        $.each($(".jqStartDate"), function (index, value) {
            var $startDate = $(value);
            var endDateId = $(value).attr("endDateId");
            var $endDate = $("#".concat(endDateId));
            self.initStartAndEndDatePicker($startDate, $endDate, isAllowEmpty);
        });
    };
    return baseSearchService;
}());
