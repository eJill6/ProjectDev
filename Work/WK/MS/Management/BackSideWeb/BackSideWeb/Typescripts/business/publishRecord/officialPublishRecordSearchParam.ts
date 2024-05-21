class officialPublishRecordSearchParam extends PagingRequestParam implements ISearchGridParam {
    /// <summary>
    /// 帖子編號
    /// </summary>
    postId: string | number | string[] | undefined;

    /// <summary>
    /// 標題
    /// </summary>
    title: string | number | string[] | undefined;

    /// <summary>
    /// 使用者編號
    /// </summary>
    userId: string | number | string[] | undefined;

    /// <summary>
    /// 帖子狀態
    /// </summary>
    status: string | number;
    /// <summary>
    /// 排序方式   
    /// </summary>
    sortType: string | number;

    userIdentity: string | number | string[] | undefined;

    /// <summary>
    /// 0: 首次送审时间
    /// 1: 再次送审时间
    /// 2: 审核时间
    /// </summary>
    dateTimeType: string | number;
    /// <summary>
    /// 開始時間
    /// </summary>
    beginDate: string | number | string[] | undefined;

    /// <summary>
    /// 結束時間
    /// </summary>
    endDate: string | number | string[] | undefined;

}