class storeEditorReviewSearchParam extends PagingRequestParam implements ISearchGridParam {
    userId: string | number | string[] | undefined;
    beginDate: string | number | string[] | undefined;
    endDate: string | number | string[] | undefined;
}