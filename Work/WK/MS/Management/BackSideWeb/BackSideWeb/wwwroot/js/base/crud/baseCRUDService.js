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
var baseCRUDService = (function (_super) {
    __extends(baseCRUDService, _super);
    function baseCRUDService(pageApiUrlSetting, htmlGridSearchContent) {
        if (htmlGridSearchContent === void 0) { htmlGridSearchContent = {
            $contentBody: $('#jqGridContent'),
            $gridContentRoot: $('#jqContentRoot'),
            $gridContentFooter: $('#jqContentFooter'),
            $pagination: $('.jqPagination'),
        }; }
        var _this = _super.call(this, pageApiUrlSetting, htmlGridSearchContent) || this;
        _this.pageApiUrlSetting = pageApiUrlSetting;
        return _this;
    }
    baseCRUDService.prototype.getInsertViewArea = function () {
        return undefined;
    };
    baseCRUDService.prototype.getUpdateViewArea = function () {
        return undefined;
    };
    baseCRUDService.prototype.openInsertView = function () {
        this.openView({
            url: this.pageApiUrlSetting.insertViewUrl,
            area: this.getInsertViewArea()
        });
    };
    baseCRUDService.prototype.openUpdateView = function (keyContent) {
        this.openView({
            url: this.pageApiUrlSetting.updateViewUrl,
            keyContent: keyContent,
            area: this.getUpdateViewArea()
        });
    };
    baseCRUDService.prototype.delete = function (keyContent) {
        var url = this.pageApiUrlSetting.deleteApiUrl;
        var isAutoHideLoading = false;
        $.alert("确定要删除？", "确定", function () {
            $.ajax2({
                url: url,
                type: "POST",
                data: { keyContent: keyContent },
                success: function (response) {
                    new baseReturnModelService(response).responseHandler(function () {
                        new pagerService(window.document).reloadCurrentPage();
                    }, isAutoHideLoading);
                },
                isAutoHideLoading: isAutoHideLoading,
            });
        }, "取消");
    };
    return baseCRUDService;
}(baseSearchGridService));
