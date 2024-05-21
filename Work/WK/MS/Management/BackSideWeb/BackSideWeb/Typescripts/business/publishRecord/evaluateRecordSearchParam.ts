class evaluateRecordSearchParam extends PagingRequestParam implements ISearchGridParam {
    id: string | number | string[] | undefined;

    /// <summary>
    /// 帖子ID
    /// </summary>
    postId: string | number | string[] | undefined;

    /// <summary>
    /// 会员ID
    /// </summary>
    userId: string | number | string[] | undefined;

    /// <summary>
    /// 帖子區域
    /// </summary>
    postType: string | number;

    /// <summary>
    /// 狀態
    /// </summary>
    status: string | number;

    /// <summary>
    /// 1. 首次送审时间、
    /// 2. 再次送审时间、
    /// 3. 审核时间、
    /// 4. 评价时间
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