class demoSearchService extends baseSearchGridService {
    private $startDate = $("#jqStartDate");
    private $endDate = $("#jqEndDate");

    constructor(searchApiUrlSetting: ISearchApiUrlSetting) {
        super(searchApiUrlSetting);

        const isAllowEmpty: boolean = true;
        this.initStartAndEndDatePicker(this.$startDate, this.$endDate, isAllowEmpty);
    }

    protected getSubmitData(): ISearchGridParam {
        const data = new demoSearchParam();
        data.typeValue = $('#jqTypeSelectList').data().value;
        data.menuName = $("#jqMenuName").val();
        data.minSort = $("#jqMinSort").val();
        data.maxSort = $("#jqMaxSort").val();
        data.startDate = this.$startDate.val();
        data.endDate = this.$endDate.val();
        return data;
    }
}