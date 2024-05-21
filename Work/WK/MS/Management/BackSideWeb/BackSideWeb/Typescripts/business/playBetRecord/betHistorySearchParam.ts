class betHistorySearchParam extends PagingRequestParam implements ISearchGridParam {
    /// <summary>
    /// 彩种
    /// </summary>
    lotteryID: string | number | string[] | undefined;

    /// <summary>
    /// 开奖期号
    /// </summary>
    palyCurrentNum: string | number | string[] | undefined;

    /// <summary>
    /// 会员ID
    /// </summary>
    userId: string | number | string[] | undefined;

    /// <summary>
    /// 派奖状态
    /// </summary>
    IsFactionAward: string | number;
    /// <summary>
    /// 输赢(0:輸 ,1:贏)
    /// </summary>
    isWin: string | number;

    /// <summary>
    /// 開始時間
    /// </summary>
    startDate: string | number | string[] | undefined;

    /// <summary>
    /// 結束時間
    /// </summary>
    endDate: string | number | string[] | undefined;

    /// <summary>
    /// 房间ID
    /// </summary>
    roomId: string | number | string[] | undefined;
}