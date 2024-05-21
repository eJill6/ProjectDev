class adminPostTransactionSearchService extends baseSearchGridService {
    private $startDate = $("#jqBeginDate");
    private $endDate = $("#jqEndDate");

    constructor(searchApiUrlSetting: ISearchApiUrlSetting) {
        super(searchApiUrlSetting);

        const isAllowEmpty: boolean = true;
        this.initStartAndEndDatePicker(this.$startDate, this.$endDate, isAllowEmpty);
    }

    protected getSubmitData(): ISearchGridParam {
        const data = new adminPostTransactionSearchParam();
        data.postId = $('#jqPostId').val();
        data.id = $("#jqId").val();
        data.userId = $("#jqUserId").val();
        data.postType = $("#jqPostType").data().value;
        data.unlockType = $("#jqUnlockType").data().value;
        data.beginDate = this.$startDate.val();
        data.endDate = this.$endDate.val();

        return data;
    }
}