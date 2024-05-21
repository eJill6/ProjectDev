class adminBookingRefundSearchService extends baseSearchGridService {
    private $startDate = $("#jqStartDate");
    private $endDate = $("#jqEndDate");

    constructor(searchApiUrlSetting: ISearchApiUrlSetting) {
        super(searchApiUrlSetting);

        const isAllowEmpty: boolean = true;
        this.initStartAndEndDatePicker(this.$startDate, this.$endDate, isAllowEmpty);
    }

    protected getSubmitData(): ISearchGridParam {
        const data = new adminBookingRefundSearchParam();
        data.BookingId = $("#jqBookingId").val();
        data.UserId = $("#jqUserId").val();
        data.PaymentType = $('#jqPaymentTypeItems').data().value;
        data.ApplyReason = $("#jqApplyReasonItems").data().value;
        data.ReasonType = $("#jqApplyReasonItems").data().value;
        data.UserIdentity = $("#jqUserIdentitySelectList").data().value;

        data.BeginDate = this.$startDate.val();

        data.EndDate = this.$endDate.val();

        return data;
    }

    openAudit(link: HTMLElement, keyContent: string) {
        const url: string = $(link).data('url');
        const area = {
            width: 800,
            height: 600
        } as layerArea;

        this.openView({
            url: url,
            keyContent: keyContent,
            area: area,
            title: '退款审核'
        });
    }
}