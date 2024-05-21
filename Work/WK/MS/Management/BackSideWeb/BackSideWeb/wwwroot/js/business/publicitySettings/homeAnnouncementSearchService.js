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
var homeAnnouncementSearchService = (function (_super) {
    __extends(homeAnnouncementSearchService, _super);
    function homeAnnouncementSearchService(pageApiUrlSetting) {
        var _this = _super.call(this, pageApiUrlSetting) || this;
        _this.defaultPageSize = 1000;
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
    homeAnnouncementSearchService.prototype.registerClickEvent = function () {
        var _this = this;
        this.$tabSelectorItems.click(function (event) {
            event.preventDefault();
            _this.$tabSelectorItems.removeClass("active");
            $(event.currentTarget).addClass("active");
            $("#jqAnnouncementFilter").hide();
            var tabId = $(event.currentTarget).attr("id");
            $("#homeAnnouncementTypeTemp").text(tabId);
            $("#type").val(tabId);
            if (tabId === "1") {
                $("#jqAnnouncementFilter").show();
                $(".AnnouncementType1").hide();
                $(".AnnouncementType2").show();
                $(".btn_insert").show();
            }
            if (tabId === "2") {
                $(".AnnouncementType1").show();
                $(".AnnouncementType2").hide();
                $(".btn_insert").hide();
            }
            var $jqSearchBtn = $(".jqSearchBtn");
            $jqSearchBtn.click();
        });
    };
    homeAnnouncementSearchService.prototype.getInsertViewArea = function () {
        return {
            width: 600,
            height: 700,
        };
    };
    homeAnnouncementSearchService.prototype.getUpdateViewArea = function () {
        return {
            width: 600,
            height: 500,
        };
    };
    homeAnnouncementSearchService.prototype.openHomeAnnouncementEdit = function (link, keyContent) {
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
    homeAnnouncementSearchService.prototype.getSubmitData = function () {
        var data = new homeAnnouncementSearchSearchParam();
        data.type = $('#type').val();
        data.title = $('#title').val();
        data.beginDate = $('#jqStartDate').val();
        data.endDate = $('#jqEndDate').val();
        data.isActive = $('#jqIsActiveSelectList').data().value;
        return data;
    };
    return homeAnnouncementSearchService;
}(baseCRUDService));
var homeAnnouncementSearchSearchParam = (function (_super) {
    __extends(homeAnnouncementSearchSearchParam, _super);
    function homeAnnouncementSearchSearchParam() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    return homeAnnouncementSearchSearchParam;
}(PagingRequestParam));
