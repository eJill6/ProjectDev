interface ISearchGridParam {
    pageNo: number,
    pageSize: number,
    isChangePage: boolean,
}

class PagingRequestParam implements ISearchGridParam {
    pageNo: number;
    pageSize: number;
    isChangePage: boolean = false;
}

interface IHtmlGridSearchContent extends IHtmlSearchContent {
    $gridContentRoot: any;
    $gridContentFooter: any;
    $pagination: any;
}

class htmlGridSearchContent extends htmlSearchContent implements IHtmlGridSearchContent {
    $gridContentRoot: any;
    $gridContentFooter: any;
    $pagination: any;
}

interface ISearchGridService extends IBaseSearchService {
    changePage(pageNo: number, pageSize: number);
}

abstract class baseSearchGridService extends baseSearchService implements ISearchGridService {
    private readonly searchFormSelector: string = "#jqSearchFilterForm";
    private htmlGridSearchContent: IHtmlGridSearchContent;
    protected submitData: ISearchGridParam;
    protected tempSubmitData: ISearchGridParam;
    protected readonly formUtilService = new formUtilService();

    protected readonly defaultPageSize: number = 12;
    protected readonly paginationSettingId: number = 0; // 參考 PaginationSetting.cs 的 SettingId

    constructor(searchApiUrlSetting: ISearchApiUrlSetting,
        htmlGridSearchContent: IHtmlGridSearchContent = {
            $contentBody: $('#jqGridContent'),
            $gridContentRoot: $('#jqContentRoot'),
            $gridContentFooter: $('#jqContentFooter'),
            $pagination: $('.jqPagination'),
        }) {
        super(searchApiUrlSetting, htmlGridSearchContent.$contentBody);

        this.htmlGridSearchContent = htmlGridSearchContent;
    }

    protected getSubmitData(): ISearchGridParam {
        return this.formUtilService.serializeObject($(this.searchFormSelector)) as ISearchGridParam;
    }

    search() {
        this.submitData = this.getSubmitData();
        this.submitData.isChangePage = false;
        this.submitData.pageNo = 1;
        const pageSize = this.getCurrentPageSize();
        this.submitData.pageSize = pageSize > 0 ? pageSize : this.defaultPageSize;

        this.doSearch(this.submitData);
    }

    changePage = (pageNo: number, pageSize: number) => {
        this.submitData.isChangePage = true;
        this.submitData.pageNo = pageNo;
        this.submitData.pageSize = pageSize > 0 ? pageSize : this.defaultPageSize;
        this.doSearch(this.submitData);
    }

    protected override doRefreshSearchResult() {
        let self = this;
        let isGridVisible: boolean = self.htmlGridSearchContent.$gridContentRoot.is(":visible");

        if (!isGridVisible) {
            return;
        }

        let $pageNoLink: JQuery<HTMLElement> = self.htmlGridSearchContent.$pagination.find(".jqPageNoLink.active");
        let pageNo: number = 1;
        let pageSize: number = this.defaultPageSize;

        if ($pageNoLink.length == 1) {
            pageNo = Number($pageNoLink.text().trim());
            pageSize = self.getCurrentPageSize();
        }

        self.changePage(pageNo, pageSize);
    }

    protected override getHtmlContents(): IHtmlSearchContent {
        return this.htmlGridSearchContent;
    }

    protected override updateGridContent(response, htmlContents: IHtmlSearchContent, self: any) {
        const $htmlGridSearchContent = htmlContents as htmlGridSearchContent;
        const $response = $(response);

        let $jqBody: JQuery<HTMLElement> = $response.find(".jqBody");
        let bodyHtml = $jqBody.html();

        if ($jqBody.find("tr").length == 0) {
            let $table = $htmlGridSearchContent.$contentBody.closest("table");
            let searchGridService = self as baseSearchGridService;
            bodyHtml = searchGridService.GetNoSearchResultHtml($table);
        }

        $htmlGridSearchContent.$contentBody.html(bodyHtml);
        $htmlGridSearchContent.$gridContentFooter.html($response.find(".jqFooter").html());
        $htmlGridSearchContent.$pagination.html($response.find(".jqPagination").html());
    }

    protected GetNoSearchResultHtml($table: JQuery<HTMLElement>,): string {
        let colspan: number = $table.find("th").length;
        let $noDataCell: JQuery<HTMLElement> = $("<td/>");
        $noDataCell.attr("colspan", colspan).text('查无资料');
        let $noDataRow: JQuery<HTMLElement> = $("<tr/>").append($noDataCell);

        return $noDataRow[0].outerHTML;
    }

    protected doAfterSearch(htmlContents: IHtmlSearchContent, self: any): void {
        const $htmlGridSearchContent = htmlContents as htmlGridSearchContent;
        $htmlGridSearchContent.$gridContentRoot.show();

        $('.jqHoverData').hoverData();
    }

    protected override validateSubmitData() {
        let isValid = super.validateSubmitData();

        if (isValid && this.submitData.isChangePage === false) {
            let $form = $(this.searchFormSelector);

            if ($form.length == 1) {
                isValid = $form.valid();
            }
        }

        return isValid;
    }

    export(): void {
        this.submitData = this.getSubmitData();

        if (!this.validateSubmitData()) {
            return;
        }

        this.submitData.isChangePage = false;
        this.submitData.pageNo = 1;
        this.submitData.pageSize = 100000 + 1; // 規格限定上限10萬筆, 如果有超過則需要至少+1讓資料超過上限

        const formattedDate: string = this.formatDateTimeString(new Date());
        const filename: string = $('h1').text() + '_' + formattedDate;

        $.ajax2({
            url: this.searchApiUrlSetting.exportApiUrl,
            type: "POST",
            data: this.submitData,
            xhrFields: {
                responseType: 'blob'
            },
            success: (response) => {
                new fileDownloadService().download(response, filename);
            }
        });
    }

    private formatDateTimeString(date: Date): string {
        const year: number = date.getFullYear();
        const month: number = date.getMonth() + 1; // Month is zero-based (0 for January).
        const day: number = date.getDate();
        const hours: number = date.getHours();
        const minutes: number = date.getMinutes();

        const formattedMonth: string = month.toString().padStart(2, '0');
        const formattedDay: string = day.toString().padStart(2, '0');
        const formattedHours: string = hours.toString().padStart(2, '0');
        const formattedMinutes: string = minutes.toString().padStart(2, '0');

        return `${year}${formattedMonth}${formattedDay}${formattedHours}${formattedMinutes}`;
    }

    protected openView(setting: openViewSetting) {
        let url: string = setting.url;

        if (setting.keyContent) {
            url = url.concat(`?keyContent=${encodeURIComponent(setting.keyContent)}`);
        }

        let title: string = ' ';

        if (setting.title) {
            title = setting.title;
        }

        let param = {
            title: title,
            url: url,
            area: setting.area
        } as openLayerParam;

        let layerServ: layerService = new layerService();
        layerServ.open(param);
    }

    private getCurrentPageSize() {
        const pageSize = Number($(`#jqPageSize_${this.paginationSettingId}`).val());

        return pageSize;
    }
}

class openViewSetting {
    url: string;
    keyContent?: string;
    area?: layerArea;
    title?: string;
}