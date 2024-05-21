var pagerService = (function () {
    function pagerService(document) {
        this.document = document;
    }
    pagerService.prototype.gotoPage = function (pageNumber, pageCount, pageSizeSelector, callback) {
        var $pageSize = $(pageSizeSelector);
        var pageSize = $pageSize.val().toString();
        if (isNaN(parseInt(pageSize))) {
            var defaultValue = $pageSize.attr("defaultValue");
            $pageSize.val(defaultValue);
            pageSize = defaultValue;
        }
        if (pageNumber >= 1 && pageNumber <= pageCount && typeof callback === "function") {
            callback(pageNumber, pageSize);
        }
    };
    pagerService.prototype.reloadCurrentPage = function () {
        var $jqPagination = $(".jqPagination", this.document);
        var pageNo = $jqPagination.find("a.active").text().trim();
        this.gotoPageByUI(pageNo);
    };
    pagerService.prototype.reloadPageSizeChange = function () {
        this.gotoPageByUI("1");
    };
    pagerService.prototype.gotoPageByUI = function (pageNo) {
        var $jqPagination = $(".jqPagination", this.document);
        var $jqPageNo = $jqPagination.find(".jqPageNo");
        $jqPageNo.val(pageNo);
        $jqPagination.find(".jqChangePageBtn").trigger("click");
    };
    return pagerService;
}());
