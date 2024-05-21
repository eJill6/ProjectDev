class dailyUsersSearchService extends baseSearchGridService {

    private $startDate = $("#jqStartDate");
    private $endDate = $("#jqEndDate");

    constructor(pageApiUrlSetting: IPageApiUrlSetting) {
        super(pageApiUrlSetting);

        const isAllowEmpty: boolean = true;
        this.initStartAndEndDatePicker(this.$startDate, this.$endDate, isAllowEmpty);
    }

    protected getSubmitData(): ISearchGridParam {
        const data = new dailyUsersSearchParam();
        data.beginTime = this.$startDate.val();
        data.endTime = this.$endDate.val();
        return data;
    }    

    protected openGridDetail(element, datetime) {

        const url: string = $(element).data("url")
        const area = {
            width: 800,
            height: 600
        } as layerArea;

        this.openView({
            url: url,
            keyContent: datetime,
            area: area,
            title: datetime
        });

    }
}