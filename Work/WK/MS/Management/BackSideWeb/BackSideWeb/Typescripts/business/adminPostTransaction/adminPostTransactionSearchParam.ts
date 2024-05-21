class adminPostTransactionSearchParam extends PagingRequestParam implements ISearchGridParam {
    id: string | number | string[] | undefined;
    postId: string | number | string[] | undefined;
    userId: string | number | string[] | undefined;
    postType: string | number | string[] | undefined;
    unlockType: string | number | string[] | undefined;
    beginDate: string | number | string[] | undefined;
    endDate: string | number | string[] | undefined;
}