class adminBookingSearchParam extends PagingRequestParam implements ISearchGridParam {
    BookingId: string | number | string[] | undefined;
    UserId: string | number | string[] | undefined;
    PaymentType: any;
    OrderStatus: any;

    UserIdentity: string | number | string[] | undefined;

    DateTimeType: string | number | string[] | undefined;
    BeginDate: string | number | string[] | undefined;
    EndDate: string | number | string[] | undefined;
}