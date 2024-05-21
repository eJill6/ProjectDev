class incomeExpenseSearchService extends baseSearchGridService {
    protected override readonly defaultPageSize = 15;

    private $startDate = $("#jqStartDate");
    private $endDate = $("#jqEndDate");

    constructor(searchApiUrlSetting: ISearchApiUrlSetting) {
        super(searchApiUrlSetting);

        const isAllowEmpty: boolean = false;
        this.initStartAndEndDatePicker(this.$startDate, this.$endDate, isAllowEmpty);
    }

    protected getSubmitData(): ISearchGridParam {

        var postType = $('#jqPostTypeSelectList').val() as number < 1 ? null : $('#jqPostTypeSelectList').val();
        var category = $('#jqCategorySelectList').val() as number == -1 ? null : $('#jqCategorySelectList').val();
        const data = new incomeExpenseSearchParam();
        data.id = $("#jqId").val();
        data.userId = $("#jqUserId").val();
        data.category = category;
        data.postType = postType;
        data.beginDate = this.$startDate.val();
        data.endDate = this.$endDate.val();
        return data;
    }
}