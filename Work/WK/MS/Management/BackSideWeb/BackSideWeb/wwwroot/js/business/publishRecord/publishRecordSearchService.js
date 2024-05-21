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
var publishRecordSearchService = (function (_super) {
    __extends(publishRecordSearchService, _super);
    function publishRecordSearchService(pageApiUrlSetting) {
        var _this = _super.call(this, pageApiUrlSetting) || this;
        _this.defaultPageSize = 30;
        _this.$startDate = $("#jqStartDate");
        _this.$endDate = $("#jqEndDate");
        var isAllowEmpty = true;
        _this.initStartAndEndDatePicker(_this.$startDate, _this.$endDate, isAllowEmpty);
        return _this;
    }
    publishRecordSearchService.prototype.getInsertViewArea = function () {
        return {
            width: 800,
            height: 800
        };
    };
    publishRecordSearchService.prototype.getUpdateViewArea = function () {
        throw new Error("Method not implemented.");
    };
    publishRecordSearchService.prototype.getSubmitData = function () {
        var data = new publishRecordSearchParam();
        data.beginDate = this.$startDate.val();
        data.endDate = this.$endDate.val();
        data.postId = $("#postId").val();
        data.title = $("#postTitle").val();
        data.userId = $("#memberNo").val();
        data.postType = $("#jqPostRegionalSelectList").data().value;
        data.status = $("#jqPostStatusSelectList").data().value;
        data.dateTimeType = $("#jqTimeTypeSelectList").data().value;
        return data;
    };
    publishRecordSearchService.prototype.openExamineView = function (link, keyContent) {
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
    publishRecordSearchService.prototype.openPostDetail = function (link, keyContent) {
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
    publishRecordSearchService.prototype.batchReview = function (type) {
        var selectedValues = this.getSelectedValues();
        if (selectedValues.length === 0) {
            alert("未选择帖子，请再确认");
            return;
        }
        if (selectedValues.length > 50) {
            alert("批量审核最大同时50笔,请再确认");
            return;
        }
        var isAutoHideLoading = false;
        $.ajax2({
            url: "/PublishRecord/BatchReview",
            type: "POST",
            data: {
                postIds: selectedValues,
                type: type
            },
            isAutoHideLoading: isAutoHideLoading,
            success: function (response) {
                var isShowSuccessMessage = false;
                new baseReturnModelService(response).responseHandler(function () {
                    $.fn.loading("hide");
                    $.toast("完成批量审核");
                    $("#selectAllPostId").prop("checked", false);
                    new pagerService(window.document).reloadCurrentPage();
                }, isAutoHideLoading, isShowSuccessMessage);
            }
        });
    };
    publishRecordSearchService.prototype.openPostEdit = function (link, keyContent) {
        var url = $(link).data('url');
        var area = {
            width: 700,
            height: 650,
        };
        this.openView({
            url: url,
            keyContent: keyContent,
            area: area,
            title: '编辑'
        });
    };
    publishRecordSearchService.prototype.getSelectedValues = function () {
        var selectedValues = [];
        var checkboxes = document.querySelectorAll('input[name="selectPostId"]:checked');
        checkboxes.forEach(function (checkbox) {
            selectedValues.push(checkbox.value);
        });
        return selectedValues;
    };
    return publishRecordSearchService;
}(baseCRUDService));
