class pagerService {
    private document: Document;

    constructor(document: Document) {
        this.document = document;
    }

    gotoPage(pageNumber: number, pageCount: number, pageSizeSelector: string, callback: Function) {
        let $pageSize = $(pageSizeSelector);
        let pageSize = $pageSize.val().toString();

        if (isNaN(parseInt(pageSize))) {
            let defaultValue = $pageSize.attr("defaultValue");
            $pageSize.val(defaultValue);
            pageSize = defaultValue
        }

        if (pageNumber >= 1 && pageNumber <= pageCount && typeof callback === "function") {
            callback(pageNumber, pageSize);
        }
    }

    reloadCurrentPage() {
        let $jqPagination = $(".jqPagination", this.document);
        let pageNo = $jqPagination.find("a.active").text().trim();
        this.gotoPageByUI(pageNo);
    }

    reloadPageSizeChange() {
        this.gotoPageByUI("1");
    }

    private gotoPageByUI(pageNo: string) {
        let $jqPagination = $(".jqPagination", this.document);
        let $jqPageNo = $jqPagination.find(".jqPageNo");
        $jqPageNo.val(pageNo);
        $jqPagination.find(".jqChangePageBtn").trigger("click");
    }
}