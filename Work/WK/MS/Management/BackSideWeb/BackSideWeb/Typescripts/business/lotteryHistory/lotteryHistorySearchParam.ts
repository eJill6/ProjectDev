class lotteryHistorySearchParam extends PagingRequestParam implements ISearchGridParam {
    /// <summary>
    /// 彩种
    /// </summary>
    LotteryID: string | number | string[] | undefined;

    /// <summary>
    /// 期号
    /// </summary>
    IssueNo: string | number | string[] | undefined;

    /// <summary>
    /// 開始時間
    /// </summary>
    startDate: string | number | string[] | undefined;

    /// <summary>
    /// 結束時間
    /// </summary>
    endDate: string | number | string[] | undefined;

    /// <summary>
    /// 开奖状态
    /// </summary>
    IsLottery: string | number | string[] | undefined;
}