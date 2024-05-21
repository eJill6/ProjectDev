class adminBookingRefundSearchParam extends PagingRequestParam implements ISearchGridParam {
    BookingId: string | number | string[] | undefined;
    UserId: string | number | string[] | undefined;
    PaymentType: any;
    ApplyReason: any;
    ReasonType: any;
    UserIdentity: string | number | string[] | undefined;

    BeginDate: string | number | string[] | undefined;

    EndDate: string | number | string[] | undefined;
}