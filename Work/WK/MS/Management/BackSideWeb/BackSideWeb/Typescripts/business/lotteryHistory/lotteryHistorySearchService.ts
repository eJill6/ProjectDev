class lotteryHistorySearchService extends baseSearchGridService {

    private $startDate = $("#jqStartDate");
    private $endDate = $("#jqEndDate");

    constructor(pageApiUrlSetting: IPageApiUrlSetting) {
        super(pageApiUrlSetting);

        const isAllowEmpty: boolean = true;
        this.initStartAndEndDatePicker(this.$startDate, this.$endDate, isAllowEmpty);
    }

    protected getSubmitData(): ISearchGridParam {
        const data = new lotteryHistorySearchParam();

        const startDate = new Date(this.$startDate.val().toString());
        const endDate = new Date(this.$endDate.val().toString());

        if (!this.$startDate.val().toString()) {
            alert('开始日期不得为空');
            return null; 
        }

        if (!this.$endDate.val().toString()) {
            alert('结束日期不得为空');
            return null;
        }

        if (isNaN(startDate.getTime()) || isNaN(endDate.getTime())) {
            alert('日期格式不符');
            return null;
        }

        const dayDifference = (endDate.getTime() - startDate.getTime()) / (1000 * 60 * 60 * 24);
        if (dayDifference > 90) {
            alert('日期超过90天');
            return null;
        }

        if (startDate > endDate) {
            alert('开始日期不能大于结束日期');
            return null;
        }
        data.startDate = this.$startDate.val();
        data.endDate = this.$endDate.val();
        data.LotteryID = $("#jqLotteryTypeSelectList").data().value;
        data.IssueNo = $("#IssueNo").val();
        data.IsLottery = $("#jqIsLotterySelectList").data().value;
        return data;
    }
}