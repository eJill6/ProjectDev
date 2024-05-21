class adminIncomeSearchParam extends PagingRequestParam implements ISearchGridParam {
    PostId: string | number | string[] | undefined;
    Id: string | number | string[] | undefined;
    UserId: string | number | string[] | undefined;
    ReportId: string | number | string[] | undefined;
    PostType: string | number | string[] | undefined;
    Status: string | number | string[] | undefined;
    DateTimeType: string | number | string[] | undefined;

    ApplyIdentity: string | number | string[] | undefined;

    BeginDate: string | number | string[] | undefined;
    EndDate: string | number | string[] | undefined;
}