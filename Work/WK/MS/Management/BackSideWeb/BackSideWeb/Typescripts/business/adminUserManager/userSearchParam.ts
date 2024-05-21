class userSearchParam extends PagingRequestParam implements ISearchGridParam {
    userIdentity: any;
    vipId: any;
    userId: string | number | string[] | undefined;
    isOpen: any;
    beginDate: string | number | string[] | undefined;
    endDate: string | number | string[] | undefined;
}