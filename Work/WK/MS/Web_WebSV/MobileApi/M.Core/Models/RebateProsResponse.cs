using ControllerShareLib.Models.Api;

namespace M.Core.Models;

public class RebateProsResponse
{
    public string Hash { get; set; }
    public Dictionary<int, IEnumerable<RebateProResponse>> RebatePros { get; set; }
}

public class RebateProResponse
{
    /// <summary>
    /// 彩種編號
    /// </summary>
    public int LotteryID { get; set; }

    /// <summary>
    /// 玩法編號
    /// </summary>
    public int PlayTypeID { get; set; }

    /// <summary>
    /// 子玩法編號
    /// </summary>
    public int PlayTypeRadioID { get; set; }

    /// <summary>
    /// 投注項已及賠率
    /// </summary>
    public List<NumberOddsModel> NumberOdds { get; set; }
}