class adminBookingSearchService extends baseSearchGridService {
    private $startDate = $("#jqStartDate");
    private $endDate = $("#jqEndDate");

    constructor(searchApiUrlSetting: ISearchApiUrlSetting) {
        super(searchApiUrlSetting);

        const isAllowEmpty: boolean = true;
        this.initStartAndEndDatePicker(this.$startDate, this.$endDate, isAllowEmpty);
    }

    protected getSubmitData(): ISearchGridParam {
        const data = new adminBookingSearchParam();
        data.BookingId = $("#jqBookingId").val();
        data.UserId = $("#jqUserId").val();
        data.PaymentType = $('#jqPaymentTypeItems').data().value;
        data.OrderStatus = $("#jqOrderStatusItems").data().value;
        data.DateTimeType = $("#jqDateTimeTypeItems").data().value;

        data.UserIdentity = $("#jqUserIdentitySelectList").data().value;

        data.BeginDate = this.$startDate.val();
        data.EndDate = this.$endDate.val();
        return data;
    }
}