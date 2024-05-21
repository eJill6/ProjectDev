class adminIncomeService extends baseCRUDService {
    protected getInsertViewArea(): layerArea {
        return {
            width: 600,
            height: 280,
        } as layerArea;
    }

    protected getUpdateViewArea(): layerArea {
        return {
            width: 600,
            height: 355,
        } as layerArea;
    }

    private $startDate = $("#jqStartDate");
    private $endDate = $("#jqEndDate");

    constructor(pageApiUrlSetting: IPageApiUrlSetting) {
        super(pageApiUrlSetting);

        const isAllowEmpty: boolean = true;
        this.initStartAndEndDatePicker(this.$startDate, this.$endDate, isAllowEmpty);
    }

    protected getSubmitData(): ISearchGridParam {
        const data = new adminIncomeSearchParam();
        data.PostId = $("#jqPostId").val();
        data.Id = $("#jqId").val();
        data.UserId = $("#jqUserId").val();
        data.ReportId = $("#jqReportId").val();
        data.PostType = $('#jqPostTypeItems').data().value;
        data.Status = $("#jqStatusItems").data().value;
        data.DateTimeType = $("#jqDateTimeTypeItems").data().value;

        data.ApplyIdentity = $("#jqUserIdentitySelectList").data().value;

        data.BeginDate = this.$startDate.val();
        data.EndDate = this.$endDate.val();

        return data;
    }

    openDetail(link: HTMLElement, keyContent: string, title: string) {
        const url: string = $(link).data('url');
        const area = {
            width: 800,
            height: 600
        } as layerArea;

        this.openView({
            url: url,
            keyContent: keyContent,
            area: area,
            title: title
        });
    }

    openUpdateView2(link:HTMLElement, keyContent: string) {
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