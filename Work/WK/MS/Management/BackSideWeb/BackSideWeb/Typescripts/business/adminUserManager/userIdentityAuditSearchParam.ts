class userIdentityAuditSearchParam extends PagingRequestParam implements ISearchGridParam {
    originalIdentity: any;
    applyIdentity: any;
    userId: string | number | string[] | undefined;
    beginDate: string | number | string[] | undefined;
    endDate: string | number | string[] | undefined;
}