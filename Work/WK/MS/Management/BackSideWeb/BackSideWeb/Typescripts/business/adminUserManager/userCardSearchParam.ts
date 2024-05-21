class userCardSearchParam extends PagingRequestParam implements ISearchGridParam {
    payType: any;
    id: string | number | string[] | undefined;
    userId: string | number | string[] | undefined;
    beginDate: string | number | string[] | undefined;
    endDate: string | number | string[] | undefined;
}