class postDailyTrendSearchService extends baseSearchGridService {

    private $startDate = $("#jqStartDate");
    private $endDate = $("#jqEndDate");

    constructor(pageApiUrlSetting: IPageApiUrlSetting) {
        super(pageApiUrlSetting);

        const isAllowEmpty: boolean = true;
        this.initStartAndEndDatePicker(this.$startDate, this.$endDate, isAllowEmpty);
    }

    protected getSubmitData(): ISearchGridParam {
        const data = new postDailyTrendSearchParam();
        data.beginTime = this.$startDate.val();
        data.endTime = this.$endDate.val();
        return data;
    }
}