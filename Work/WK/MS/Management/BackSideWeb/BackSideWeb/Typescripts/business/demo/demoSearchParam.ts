class demoSearchParam extends PagingRequestParam implements ISearchGridParam {
    typeValue: any;
    menuName: string | number | string[] | undefined;
    minSort: string | number | string[] | undefined;
    maxSort: string | number | string[] | undefined;
    startDate: string | number | string[] | undefined;
    endDate: string | number | string[] | undefined;
}