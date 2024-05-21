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
var storeManageSearchService = (function (_super) {
    __extends(storeManageSearchService, _super);
    function storeManageSearchService(searchApiUrlSetting) {
        var _this = _super.call(this, searchApiUrlSetting) || this;
        _this.defaultPageSize = 15;
        return _this;
    }
    storeManageSearchService.prototype.getSubmitData = function () {
        var data = new storeManageSearchParam();
        data.userId = $("#jqUserId").val();
        data.isOpen = $("#jqisOpen").data().value;
        return data;
    };
    storeManageSearchService.prototype.openStoreEdit = function (link, keyContent) {
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
    storeManageSearchService.prototype.openDetail = function (link, keyContent) {
        var url = $(link).data('url');
        var area = {
            width: 400,
            height: 600,
        };
        this.openView({
            url: url,
            keyContent: keyContent,
            area: area,
            title: '详细'
        });
    };
    return storeManageSearchService;
}(baseSearchGridService));
