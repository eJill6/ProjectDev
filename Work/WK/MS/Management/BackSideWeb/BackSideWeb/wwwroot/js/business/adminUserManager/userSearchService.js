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
var userSearchService = (function (_super) {
    __extends(userSearchService, _super);
    function userSearchService(searchApiUrlSetting) {
        var _this = _super.call(this, searchApiUrlSetting) || this;
        _this.defaultPageSize = 15;
        _this.$startDate = $("#jqStartDate");
        _this.$endDate = $("#jqEndDate");
        var isAllowEmpty = true;
        _this.initStartAndEndDatePicker(_this.$startDate, _this.$endDate, isAllowEmpty);
        return _this;
    }
    userSearchService.prototype.getSubmitData = function () {
        var data = new userSearchParam();
        data.userId = $("#jqUserId").val();
        data.beginDate = this.$startDate.val();
        data.endDate = this.$endDate.val();
        data.userIdentity = $('#jqUserIdentitySelectList').data().value;
        return data;
    };
    userSearchService.prototype.validateSubmitData = function () {
        var data = this.submitData;
        if ((data.beginDate == '' && data.endDate != '') || (data.beginDate != '' && data.endDate == '')) {
            alert('请同时填入开始与结束日期');
            return false;
        }
        return _super.prototype.validateSubmitData.call(this);
    };
    userSearchService.prototype.openUserDetail = function (link, keyContent) {
        var url = $(link).data('url');
        var area = {
            width: 800,
            height: 600
        };
        this.openView({
            url: url,
            keyContent: keyContent,
            area: area,
            title: '会员详情'
        });
    };
    userSearchService.prototype.openIdentityEdit = function (link, keyContent) {
        var url = $(link).data('url');
        var area = {
            width: 800,
            height: 600
        };
        this.openView({
            url: url,
            keyContent: keyContent,
            area: area,
            title: '身份编辑'
        });
    };
    userSearchService.prototype.openEarnestMoneyEdit = function (link, keyContent) {
        var url = $(link).data('url');
        var area = {
            width: 800,
            height: 600
        };
        this.openView({
            url: url,
            keyContent: keyContent,
            area: area,
            title: '调整保证金'
        });
    };
    userSearchService.prototype.openStoreEdit = function (link, keyContent) {
        var url = $(link).data('url');
        var area = {
            width: 470,
            height: 700
        };
        this.openView({
            url: url,
            keyContent: keyContent,
            area: area,
            title: '编辑店铺'
        });
    };
    return userSearchService;
}(baseSearchGridService));
