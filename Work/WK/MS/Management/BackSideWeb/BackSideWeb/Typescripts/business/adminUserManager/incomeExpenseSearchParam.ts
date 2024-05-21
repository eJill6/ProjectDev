class incomeExpenseSearchParam extends PagingRequestParam implements ISearchGridParam {
    category: any | undefined;
    postType: any | undefined;
    id: string | number | string[] | undefined;
    userId: string | number | string[] | undefined;
    beginDate: string | number | string[] | undefined;
    endDate: string | number | string[] | undefined;
}