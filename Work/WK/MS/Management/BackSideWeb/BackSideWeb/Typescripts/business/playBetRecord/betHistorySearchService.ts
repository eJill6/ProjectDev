class betHistorySearchService extends baseSearchGridService {

    private $startDate = $("#jqStartDate");
    private $endDate = $("#jqEndDate");

    constructor(pageApiUrlSetting: IPageApiUrlSetting) {
        super(pageApiUrlSetting);

        const isAllowEmpty: boolean = true;
        this.initStartAndEndDatePicker(this.$startDate, this.$endDate, isAllowEmpty);
    }

    protected getSubmitData(): ISearchGridParam {
        const data = new betHistorySearchParam();

        // 将日期字符串转换为日期对象
        const startDate = new Date(this.$startDate.val().toString());
        const endDate = new Date(this.$endDate.val().toString());

        // 检查开始时间是否为空
        if (!this.$startDate.val().toString()) {
            alert('开始日期不得为空');
            return null; // 或者返回其他适当的值，以指示错误状态
        }

        // 检查结束时间是否为空
        if (!this.$endDate.val().toString()) {
            alert('结束日期不得为空');
            return null;
        }

        // 检查日期格式是否有效
        if (isNaN(startDate.getTime()) || isNaN(endDate.getTime())) {
            alert('日期格式不符');
            return null;
        }

        // 检查日期范围是否超过 90 天
        const dayDifference = (endDate.getTime() - startDate.getTime()) / (1000 * 60 * 60 * 24);
        if (dayDifference > 90) {
            alert('日期超过90天');
            return null;
        }

        // 检查开始时间是否大于结束时间
        if (startDate > endDate) {
            alert('开始日期不能大于结束日期');
            return null;
        }
        data.startDate = this.$startDate.val();
        data.endDate = this.$endDate.val();
        data.lotteryID = $("#jqLotteryTypeSelectList").data().value;
        data.palyCurrentNum = $("#palyCurrentNum").val();
        data.userId = $("#userId").val();
        data.IsFactionAward = $("#jqIsFactionAwardSelectList").data().value;
        data.isWin = $("#jqIsWinSelectList").data().value;
        data.roomId = $("#roomId").val();
        return data;
    }

    protected override updateGridContent(response, htmlContents: IHtmlSearchContent, self: any) {
        //先處理元表格，在去處理jqDiv
        super.updateGridContent(response, htmlContents, self);
        const $response = $(response);
        let $jqBody: JQuery<HTMLElement> = $response.find(`.jqDiv`);
        let bodyHtml = $jqBody.html();
        $(`#myDiv`).html(bodyHtml);
    }

    openGridDetail(link: HTMLElement, keyContent: string) {
        const url: string = $(link).data('url');
        const area = {
            width: 450,
            height: 600,
        } as layerArea;

        this.openView({
            url: url,
            keyContent: keyContent,
            area: area,
            title: '订单详细'
        });
    }
}