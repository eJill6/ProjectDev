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
var postWeightSearchService = (function (_super) {
    __extends(postWeightSearchService, _super);
    function postWeightSearchService(pageApiUrlSetting) {
        var _this = _super.call(this, pageApiUrlSetting) || this;
        _this.$tabSelectorItems = $('.pageTab ul a');
        _this.defaultPageSize = 1000;
        $(document).ready(function () {
            _this.registerClickEvent();
            _this.$tabSelectorItems.first().click();
        });
        return _this;
    }
    postWeightSearchService.prototype.getInsertViewArea = function () {
        return {
            width: 600,
            height: 400,
        };
    };
    postWeightSearchService.prototype.getUpdateViewArea = function () {
        return {
            width: 600,
            height: 400,
        };
    };
    postWeightSearchService.prototype.registerClickEvent = function () {
        var _this = this;
        this.$tabSelectorItems.click(function (event) {
            event.preventDefault();
            _this.$tabSelectorItems.removeClass("active");
            $(event.currentTarget).addClass("active");
            $("#jqMemberFilter").show();
            $("#jqContentRoot").show();
            $(".btn_insert ").show();
            $("#jqGoldStoreFilter").hide();
            $("#jqGoldStoreContentRoot").hide();
            var tabId = $(event.currentTarget).attr("id");
            $("#postTypeTemp").text(tabId);
            if (tabId !== "3" && tabId !== "4") {
                var $jqSearchBtn = $(".jqSearchBtn");
                $jqSearchBtn.click();
            }
            if (tabId === "3" || tabId === "4") {
                $(".btn_insert ").hide();
                $("#jqMemberFilter").hide();
                $("#jqContentRoot").hide();
                $("#jqGoldStoreFilter").show();
                $("#jqGoldStoreContentRoot").show();
                switch (tabId) {
                    case "3":
                        $(".EditGoldStore ").hide();
                        $(".EditGoldStoreRecommend ").show();
                        $(".GoldStore ").text("推荐 3");
                        _this.GetGoldStoreData(1);
                        break;
                    case "4":
                        $(".EditGoldStore ").show();
                        $(".EditGoldStoreRecommend ").hide();
                        $(".GoldStore ").text("TOP 3");
                        _this.GetGoldStoreData(2);
                        break;
                    default:
                }
            }
        });
    };
    postWeightSearchService.prototype.getSubmitData = function () {
        var data = new postWeightSearchSearchParam();
        data.postType = $('#postTypeTemp').text();
        return data;
    };
    postWeightSearchService.prototype.batchReview = function (type) {
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
            url: "/PostWeight/BatchReview",
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
                    $.toast("完成批量移除");
                    $("#selectAllPostId").prop("checked", false);
                    new pagerService(window.document).reloadCurrentPage();
                }, isAutoHideLoading, isShowSuccessMessage);
            }
        });
    };
    postWeightSearchService.prototype.GetGoldStoreData = function (type) {
        $.ajax2({
            url: "/PostWeight/GetGoldStoreData",
            type: "GET",
            data: {
                type: type
            },
            success: function (response) {
                if (response.isSuccess === true) {
                    var $goldStoreGridContent = $("#jqGoldStoreGridContent");
                    $goldStoreGridContent.empty();
                    $.each(response.datas, function (index, item) {
                        var $tableRow = $("<tr>");
                        $tableRow.append($("<td>").text(item.top));
                        $tableRow.append($("<td>").text(item.userId));
                        $goldStoreGridContent.append($tableRow);
                    });
                    $("#jqGoldStoreContentRoot").show();
                }
            }
        });
    };
    postWeightSearchService.prototype.openGoldStoreEdit = function (link) {
        var url = $(link).data('url');
        var area = {
            width: 400,
            height: 360,
        };
        this.openView({
            url: url,
            area: area,
            title: '编辑'
        });
    };
    postWeightSearchService.prototype.openGoldStoreRecommendEdit = function (link) {
        var url = $(link).data('url');
        var area = {
            width: 400,
            height: 360,
        };
        this.openView({
            url: url,
            area: area,
            title: '编辑'
        });
    };
    postWeightSearchService.prototype.getSelectedValues = function () {
        var selectedValues = [];
        var checkboxes = document.querySelectorAll('input[name="selectPostId"]:checked');
        checkboxes.forEach(function (checkbox) {
            selectedValues.push(checkbox.value);
        });
        return selectedValues;
    };
    return postWeightSearchService;
}(baseCRUDService));
var postWeightSearchSearchParam = (function (_super) {
    __extends(postWeightSearchSearchParam, _super);
    function postWeightSearchSearchParam() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    return postWeightSearchSearchParam;
}(PagingRequestParam));
