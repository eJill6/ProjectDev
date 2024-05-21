class evaluateRecordSearchService extends baseCRUDService {
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

    protected getSubmitData(): ISearchGridParam {
        const data = new evaluateRecordSearchParam();
        data.beginDate = this.$startDate.val();
        data.endDate = this.$endDate.val();
        data.postId = $("#postId").val();
        data.id = $("#commentId").val();
        data.userId = $("#memberNo").val();
        data.postType = $("#jqPostRegionalSelectList").data().value;
        data.status = $("#jqCommentStatusSelectList").data().value;
        data.dateTimeType = $("#jqTimeTypeSelectList").data().value;

        return data;
    }

    openUpdateView2(link: HTMLElement, keyContent: string) {
        const url: string = $(link).data('url');
        const area = {
            width: 800,
            height: 650,
        } as layerArea;

        this.openView({
            url: url,
            keyContent: keyContent,
            area: area
        });
    }
}