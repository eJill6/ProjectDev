class adminPostTransactionService extends baseCRUDService {
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

    private $startDate = $("#jqBeginDate");
    private $endDate = $("#jqEndDate");

    constructor(pageApiUrlSetting: IPageApiUrlSetting) {
        super(pageApiUrlSetting);

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