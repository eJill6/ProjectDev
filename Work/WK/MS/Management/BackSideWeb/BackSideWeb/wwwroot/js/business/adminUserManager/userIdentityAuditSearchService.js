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
var userIdentityAuditSearchService = (function (_super) {
    __extends(userIdentityAuditSearchService, _super);
    function userIdentityAuditSearchService(pageApiUrlSetting) {
        var _this = _super.call(this, pageApiUrlSetting) || this;
        _this.defaultPageSize = 15;
        _this.$tabSelectorItems = $('.pageTab ul a');
        _this.$startDate = $("#jqStartDate");
        _this.$endDate = $("#jqEndDate");
        var isAllowEmpty = true;
        _this.initStartAndEndDatePicker(_this.$startDate, _this.$endDate, isAllowEmpty);
        $(document).ready(function () {
            _this.registerClickEvent();
            _this.$tabSelectorItems.first().click();
        });
        return _this;
    }
    userIdentityAuditSearchService.prototype.registerClickEvent = function () {
        var _this = this;
        this.$tabSelectorItems.click(function (event) {
            event.preventDefault();
            _this.$tabSelectorItems.removeClass("active");
            $(event.currentTarget).addClass("active");
            var tabId = $(event.currentTarget).attr("id");
            $("#userIdentityTypeTemp").text(tabId);
            $("#type").val(tabId);
            var $jqSearchBtn = $(".jqSearchBtn");
            $jqSearchBtn.click();
        });
    };
    userIdentityAuditSearchService.prototype.getSubmitData = function () {
        if (!this.$startDate.val().toString()) {
            alert('开始日期不得为空');
            return null;
        }
        if (!this.$endDate.val().toString()) {
            alert('结束日期不得为空');
            return null;
        }
        var data = new userIdentityAuditSearchParam();
        data.applyIdentity = $('#type').val();
        data.userId = $("#jqUserId").val();
        data.beginDate = this.$startDate.val();
        data.endDate = this.$endDate.val();
        return data;
    };
    userIdentityAuditSearchService.prototype.openEdit = function (link, keyContent) {
        var url = $(link).data('url');
        var area = {
            width: 550,
            height: 580
        };
        this.openView({
            url: url,
            keyContent: keyContent,
            area: area,
            title: '身份认证'
        });
    };
    userIdentityAuditSearchService.prototype.openDetail = function (link, keyContent) {
        var url = $(link).data('url');
        var area = {
            width: 500,
            height: 700,
        };
        this.openView({
            url: url,
            keyContent: keyContent,
            area: area,
            title: '检视'
        });
    };
    return userIdentityAuditSearchService;
}(baseSearchGridService));
