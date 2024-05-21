class monthlyUsersSearchService extends baseSearchGridService {

    private $startDate = $("#jqStartDate");
    private $endDate = $("#jqEndDate");

    constructor(pageApiUrlSetting: IPageApiUrlSetting) {
        super(pageApiUrlSetting);
        this.setDefaultDateValues();
    }

    private setDefaultDateValues() {
        const currentDate = new Date();
        const startMonth = currentDate.getMonth() > 0 ? currentDate.getMonth() - 1 : 11;
        this.$startDate.val(currentDate.getFullYear() + '-' + ('0' + (startMonth + 1)).slice(-2));
        this.$endDate.val(currentDate.getFullYear() + '-' + ('0' + (currentDate.getMonth() + 1)).slice(-2));
    }

    protected getSubmitData(): ISearchGridParam {
        const data = new monthlyUsersSearchParam();
        data.beginTime = this.$startDate.val();
        data.endTime = this.$endDate.val();
        return data;
    }

}