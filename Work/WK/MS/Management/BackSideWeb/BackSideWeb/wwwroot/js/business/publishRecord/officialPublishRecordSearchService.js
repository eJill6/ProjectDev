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
var officialPublishRecordSearchService = (function (_super) {
    __extends(officialPublishRecordSearchService, _super);
    function officialPublishRecordSearchService(pageApiUrlSetting) {
        var _this = _super.call(this, pageApiUrlSetting) || this;
        _this.defaultPageSize = 30;
        _this.$startDate = $("#jqStartDate");
        _this.$endDate = $("#jqEndDate");
        var isAllowEmpty = true;
        _this.initStartAndEndDatePicker(_this.$startDate, _this.$endDate, isAllowEmpty);
        return _this;
    }
    officialPublishRecordSearchService.prototype.getInsertViewArea = function () {
        return {
            width: 800,
            height: 600
        };
    };
    officialPublishRecordSearchService.prototype.getUpdateViewArea = function () {
        throw new Error("Method not implemented.");
    };
    officialPublishRecordSearchService.prototype.getSubmitData = function () {
        if (!this.$startDate.val().toString()) {
            alert('开始日期不得为空');
            return null;
        }
        if (!this.$endDate.val().toString()) {
            alert('结束日期不得为空');
            return null;
        }
        var data = new officialPublishRecordSearchParam();
        data.beginDate = this.$startDate.val();
        data.endDate = this.$endDate.val();
        data.postId = $("#postId").val();
        data.title = $("#postTitle").val();
        data.userId = $("#memberNo").val();
        data.userIdentity = $("#jqUserIdentitySelectList").data().value;
        data.status = $("#jqPostStatusSelectList").data().value;
        data.dateTimeType = $("#jqTimeTypeSelectList").data().value;
        return data;
    };
    officialPublishRecordSearchService.prototype.openUpdateView2 = function (link, keyContent) {
        var url = $(link).data('url');
        var area = {
            width: 600,
            height: 800,
        };
        this.openView({
            url: url,
            keyContent: keyContent,
            area: area
        });
    };
    officialPublishRecordSearchService.prototype.openOfficialPostDetail = function (link, keyContent) {
        var url = $(link).data('url');
        var area = {
            width: 600,
            height: 800,
        };
        this.openView({
            url: url,
            keyContent: keyContent,
            area: area,
            title: '帖子检视'
        });
    };
    officialPublishRecordSearchService.prototype.openOfficialPostEdit = function (link, keyContent) {
        var url = $(link).data('url');
        var area = {
            width: 700,
            height: 650,
        };
        this.openView({
            url: url,
            keyContent: keyContent,
            area: area,
            title: '帖子编辑'
        });
    };
    officialPublishRecordSearchService.prototype.EditLock = function (element) {
        var self = this;
        var $element = $(element);
        var url = $element.data("url");
        var postid = $element.data("postid");
        var status = $element.data("status");
        this.doEditLock(url, postid, status);
    };
    officialPublishRecordSearchService.prototype.ModifyDeleteStatus = function (element) {
        var self = this;
        var $element = $(element);
        var url = $element.data("url");
        var postid = $element.data("postid");
        var userid = $element.data("userid");
        var isdelete = $element.data("isdelete");
        this.doModifyDelete(url, postid, userid, isdelete);
    };
    officialPublishRecordSearchService.prototype.doEditLock = function (url, postid, status) {
        var self = this;
        var isAutoHideLoading = true;
        var param = {
            postid: postid,
            status: status
        };
        $.ajax2({
            url: url,
            type: "POST",
            data: param,
            success: function (response) {
                new baseReturnModelService(response).responseHandler(function () {
                    new pagerService(window.document).reloadCurrentPage();
                }, isAutoHideLoading);
            },
            isAutoHideLoading: isAutoHideLoading,
        });
    };
    officialPublishRecordSearchService.prototype.doModifyDelete = function (url, postid, userid, isdelete) {
        var self = this;
        var confirmProductName = "下架";
        if (!isdelete) {
            confirmProductName = "上架";
        }
        window.confirm("\u662F\u5426".concat(confirmProductName, "\u8BE5\u5E16\u5B50\u5417\uFF1F"), function () {
            var isAutoHideLoading = true;
            var param = {
                postid: postid,
                userid: userid,
                isdelete: isdelete
            };
            $.ajax2({
                url: url,
                type: "POST",
                data: param,
                success: function (response) {
                    new baseReturnModelService(response).responseHandler(function () {
                        new pagerService(window.document).reloadCurrentPage();
                    }, isAutoHideLoading);
                },
                isAutoHideLoading: isAutoHideLoading,
            });
        });
    };
    return officialPublishRecordSearchService;
}(baseCRUDService));
