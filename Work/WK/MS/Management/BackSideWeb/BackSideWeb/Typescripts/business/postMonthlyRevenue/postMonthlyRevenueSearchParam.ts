class postMonthlyRevenueSearchParam extends PagingRequestParam implements ISearchGridParam {

    /// <summary>
    /// 開始時間
    /// </summary>
    beginTime: string | number | string[] | undefined;

    /// <summary>
    /// 結束時間
    /// </summary>
    endTime: string | number | string[] | undefined;
}