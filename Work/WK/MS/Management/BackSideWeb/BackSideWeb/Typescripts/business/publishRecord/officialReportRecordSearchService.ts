class officialReportRecordSearchService extends baseCRUDService {
    protected getInsertViewArea(): layerArea {
        throw new Error("Method not implemented.");
    }
    protected getUpdateViewArea(): layerArea {
        throw new Error("Method not implemented.");
    }

    private $startDate = $("#jqStartDate");
    private $endDate = $("#jqEndDate");

    constructor(pageApiUrlSetting: IPageApiUrlSetting) {
        super(pageApiUrlSetting);

        const isAllowEmpty: boolean = true;
        this.initStartAndEndDatePicker(this.$startDate, this.$endDate, isAllowEmpty);
    }
    //reportId,postId,memberNo,jqPostRegionalSelectList,jqReportTypeSelectList,jqStatusSelectList
    protected getSubmitData(): ISearchGridParam {
        const data = new officialReportRecordSearchParam();
        data.beginDate = this.$startDate.val();
        data.endDate = this.$endDate.val();
        data.postId = $("#postId").val();
        data.id = $("#reportId").val();
        data.userId = $("#memberNo").val();
        data.status = $("#jqStatusSelectList").data().value;
        data.reportType = $("#jqReportTypeSelectList").data().value;

        return data;
    }

    openUpdateView2(link: HTMLElement, keyContent: string) {
        const url: string = $(link).data('url');
        const area = {
            width: 800,
            height: 600,
        } as layerArea;

        this.openView({
            url: url,
            keyContent: keyContent,
            area: area
        });
    }
}