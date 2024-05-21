class adminIncomeSearchService extends baseSearchGridService {
    private $startDate = $("#jqStartDate");
    private $endDate = $("#jqEndDate");

    constructor(searchApiUrlSetting: ISearchApiUrlSetting) {
        super(searchApiUrlSetting);

        const isAllowEmpty: boolean = true;
        this.initStartAndEndDatePicker(this.$startDate, this.$endDate, isAllowEmpty);
    }

    protected getSubmitData(): ISearchGridParam {
        if (!this.$startDate.val().toString()) {
            alert('开始日期不得为空');
            return null;
        }

        if (!this.$endDate.val().toString()) {
            alert('结束日期不得为空');
            return null;
        }
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
}