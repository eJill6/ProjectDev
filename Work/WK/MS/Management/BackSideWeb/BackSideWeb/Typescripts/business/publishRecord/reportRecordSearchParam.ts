class reportRecordSearchParam extends PagingRequestParam implements ISearchGridParam {
    /// <summary>
    /// 投诉单ID
    /// </summary>
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
    /// 投诉原因
    /// </summary>
    reportType: string | number;

    /// <summary>
    /// 狀態
    /// </summary>
    status: string | number;

    /// <summary>
    /// 開始時間
    /// </summary>
    beginDate: string | number | string[] | undefined;

    /// <summary>
    /// 結束時間
    /// </summary>
    endDate: string | number | string[] | undefined;
}