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
var PagingRequestParam = (function () {
    function PagingRequestParam() {
        this.isChangePage = false;
    }
    return PagingRequestParam;
}());
var htmlGridSearchContent = (function (_super) {
    __extends(htmlGridSearchContent, _super);
    function htmlGridSearchContent() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    return htmlGridSearchContent;
}(htmlSearchContent));
var baseSearchGridService = (function (_super) {
    __extends(baseSearchGridService, _super);
    function baseSearchGridService(searchApiUrlSetting, htmlGridSearchContent) {
        if (htmlGridSearchContent === void 0) { htmlGridSearchContent = {
            $contentBody: $('#jqGridContent'),
            $gridContentRoot: $('#jqContentRoot'),
            $gridContentFooter: $('#jqContentFooter'),
            $pagination: $('.jqPagination'),
        }; }
        var _this = _super.call(this, searchApiUrlSetting, htmlGridSearchContent.$contentBody) || this;
        _this.searchFormSelector = "#jqSearchFilterForm";
        _this.formUtilService = new formUtilService();
        _this.defaultPageSize = 12;
        _this.paginationSettingId = 0;
        _this.changePage = function (pageNo, pageSize) {
            _this.submitData.isChangePage = true;
            _this.submitData.pageNo = pageNo;
            _this.submitData.pageSize = pageSize > 0 ? pageSize : _this.defaultPageSize;
            _this.doSearch(_this.submitData);
        };
        _this.htmlGridSearchContent = htmlGridSearchContent;
        return _this;
    }
    baseSearchGridService.prototype.getSubmitData = function () {
        return this.formUtilService.serializeObject($(this.searchFormSelector));
    };
    baseSearchGridService.prototype.search = function () {
        this.submitData = this.getSubmitData();
        this.submitData.isChangePage = false;
        this.submitData.pageNo = 1;
        var pageSize = this.getCurrentPageSize();
        this.submitData.pageSize = pageSize > 0 ? pageSize : this.defaultPageSize;
        this.doSearch(this.submitData);
    };
    baseSearchGridService.prototype.doRefreshSearchResult = function () {
        var self = this;
        var isGridVisible = self.htmlGridSearchContent.$gridContentRoot.is(":visible");
        if (!isGridVisible) {
            return;
        }
        var $pageNoLink = self.htmlGridSearchContent.$pagination.find(".jqPageNoLink.active");
        var pageNo = 1;
        var pageSize = this.defaultPageSize;
        if ($pageNoLink.length == 1) {
            pageNo = Number($pageNoLink.text().trim());
            pageSize = self.getCurrentPageSize();
        }
        self.changePage(pageNo, pageSize);
    };
    baseSearchGridService.prototype.getHtmlContents = function () {
        return this.htmlGridSearchContent;
    };
    baseSearchGridService.prototype.updateGridContent = function (response, htmlContents, self) {
        var $htmlGridSearchContent = htmlContents;
        var $response = $(response);
        var $jqBody = $response.find(".jqBody");
        var bodyHtml = $jqBody.html();
        if ($jqBody.find("tr").length == 0) {
            var $table = $htmlGridSearchContent.$contentBody.closest("table");
            var searchGridService = self;
            bodyHtml = searchGridService.GetNoSearchResultHtml($table);
        }
        $htmlGridSearchContent.$contentBody.html(bodyHtml);
        $htmlGridSearchContent.$gridContentFooter.html($response.find(".jqFooter").html());
        $htmlGridSearchContent.$pagination.html($response.find(".jqPagination").html());
    };
    baseSearchGridService.prototype.GetNoSearchResultHtml = function ($table) {
        var colspan = $table.find("th").length;
        var $noDataCell = $("<td/>");
        $noDataCell.attr("colspan", colspan).text('查无资料');
        var $noDataRow = $("<tr/>").append($noDataCell);
        return $noDataRow[0].outerHTML;
    };
    baseSearchGridService.prototype.doAfterSearch = function (htmlContents, self) {
        var $htmlGridSearchContent = htmlContents;
        $htmlGridSearchContent.$gridContentRoot.show();
        $('.jqHoverData').hoverData();
    };
    baseSearchGridService.prototype.validateSubmitData = function () {
        var isValid = _super.prototype.validateSubmitData.call(this);
        if (isValid && this.submitData.isChangePage === false) {
            var $form = $(this.searchFormSelector);
            if ($form.length == 1) {
                isValid = $form.valid();
            }
        }
        return isValid;
    };
    baseSearchGridService.prototype.export = function () {
        this.submitData = this.getSubmitData();
        if (!this.validateSubmitData()) {
            return;
        }
        this.submitData.isChangePage = false;
        this.submitData.pageNo = 1;
        this.submitData.pageSize = 100000 + 1;
        var formattedDate = this.formatDateTimeString(new Date());
        var filename = $('h1').text() + '_' + formattedDate;
        $.ajax2({
            url: this.searchApiUrlSetting.exportApiUrl,
            type: "POST",
            data: this.submitData,
            xhrFields: {
                responseType: 'blob'
            },
            success: function (response) {
                new fileDownloadService().download(response, filename);
            }
        });
    };
    baseSearchGridService.prototype.formatDateTimeString = function (date) {
        var year = date.getFullYear();
        var month = date.getMonth() + 1;
        var day = date.getDate();
        var hours = date.getHours();
        var minutes = date.getMinutes();
        var formattedMonth = month.toString().padStart(2, '0');
        var formattedDay = day.toString().padStart(2, '0');
        var formattedHours = hours.toString().padStart(2, '0');
        var formattedMinutes = minutes.toString().padStart(2, '0');
        return "".concat(year).concat(formattedMonth).concat(formattedDay).concat(formattedHours).concat(formattedMinutes);
    };
    baseSearchGridService.prototype.openView = function (setting) {
        var url = setting.url;
        if (setting.keyContent) {
            url = url.concat("?keyContent=".concat(encodeURIComponent(setting.keyContent)));
        }
        var title = ' ';
        if (setting.title) {
            title = setting.title;
        }
        var param = {
            title: title,
            url: url,
            area: setting.area
        };
        var layerServ = new layerService();
        layerServ.open(param);
    };
    baseSearchGridService.prototype.getCurrentPageSize = function () {
        var pageSize = Number($("#jqPageSize_".concat(this.paginationSettingId)).val());
        return pageSize;
    };
    return baseSearchGridService;
}(baseSearchService));
var openViewSetting = (function () {
    function openViewSetting() {
    }
    return openViewSetting;
}());
