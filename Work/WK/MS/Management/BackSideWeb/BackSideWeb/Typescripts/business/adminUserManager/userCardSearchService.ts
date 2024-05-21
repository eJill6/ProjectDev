class userCardSearchService extends baseSearchGridService {
    protected override readonly defaultPageSize = 15;

    private $startDate = $("#jqStartDate");
    private $endDate = $("#jqEndDate");

    constructor(searchApiUrlSetting: ISearchApiUrlSetting) {
        super(searchApiUrlSetting);

        const isAllowEmpty: boolean = false;
        this.initStartAndEndDatePicker(this.$startDate, this.$endDate, isAllowEmpty);
    }

    protected getSubmitData(): ISearchGridParam {
        const data = new userCardSearchParam();
        data.id = $("#jqId").val();
        data.userId = $("#jqUserId").val();
        data.payType = $('#jqPayTypeSelectList').data().value;
        data.beginDate = this.$startDate.val();
        data.endDate = this.$endDate.val();

        return data;
    }
}