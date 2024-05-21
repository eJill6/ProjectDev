class storeEditorReviewSearchService extends baseSearchGridService {
    protected override readonly defaultPageSize = 15;

    protected $tabSelectorItems: JQuery<HTMLElement> = $('.pageTab ul a');

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
        const data = new storeEditorReviewSearchParam();
        data.userId = $("#jqUserId").val();
        data.beginDate = this.$startDate.val();
        data.endDate = this.$endDate.val();
        return data;
    }

    openEdit(link: HTMLElement, keyContent: string) {
        const url: string = $(link).data('url');
        const area = {
            width: 950,
            height: 700
        } as layerArea;

        this.openView({
            url: url,
            keyContent: keyContent,
            area: area,
            title: '审核'
        });
    }
    openDetail(link: HTMLElement, keyContent: string) {
        const url: string = $(link).data('url');
        const area = {
            width: 950,
            height: 700,
        } as layerArea;

        this.openView({
            url: url,
            keyContent: keyContent,
            area: area,
            title: '检视'
        });
    }
}