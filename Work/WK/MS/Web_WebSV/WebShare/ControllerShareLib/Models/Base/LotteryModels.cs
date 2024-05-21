using SLPolyGame.Web.Model;
using System.ComponentModel.DataAnnotations;

namespace ControllerShareLib.Models.Base
{
    public class LotteryDrawResultModel
    {
        public string LotteryTypeName { get; set; }

        public int LotteryNumbers { get; set; }

        public int LotteryId { get; set; }

        public LotteryModel LatestTime { get; set; }

        public LotteryModel CurrentTime { get; set; }

        public string OfficialLotteryUrl { get; set; }

        public string NumberTrendUrl { get; set; }

        public decimal LotteryMaxBonusMoney { get; set; } = 0;

        public LotteryType? LotteryCategory { get; set; } = null;

        public string DrawResultLogo { get; set; } = string.Empty;

        public LotteryDrawResultModel()
        {
            LatestTime = new LotteryModel();
            CurrentTime = new LotteryModel();
        }
    }

    public class MMCLotteryDrawResultModel
    {
        public string LotteryTypeName { get; set; }

        public int LotteryNumbers { get; set; }

        public LotteryModel LatestTime { get; set; }

        public LotteryModel CurrentTime { get; set; }

        public string OfficialLotteryUrl { get; set; }

        public string NumberTrendUrl { get; set; }

        public string NextIssueNo { get; set; }

        public string LotteryMoney { get; set; }

        public decimal LotteryMaxBonusMoney { get; set; } = 0;

        public MMCLotteryDrawResultModel()
        {
            LatestTime = new LotteryModel();
            CurrentTime = new LotteryModel();
            NextIssueNo = string.Empty;
            LotteryMoney = "0.0000";
        }
    }

    public class SummaryModel
    {
        public SummaryModel()
        {
            LotteryList = new List<LotteryModel>();
            OrderList = new List<OrderDetailsModel>();
        }

        /// <summary>
        /// 今日投注
        /// </summary>
        public IList<OrderDetailsModel> OrderList { get; set; }

        /// <summary>
        /// 今日开奖
        /// </summary>
        public IList<LotteryModel> LotteryList { get; set; }
    }

    public class TodaySummaryModel : SummaryModel
    {
        public TodaySummaryModel()
            : base()
        {
            SummaryList = new List<PlaySummaryModel>();
        }

        /// <summary>
        /// 盈亏总览
        /// </summary>
        public IList<PlaySummaryModel> SummaryList { get; set; }
    }

    public class PK10SummaryModel : SummaryModel
    {
        public PK10SummaryModel()
            : base()
        {
            LongQueueList = new List<LongQueue>();
        }

        /// <summary>
        /// 盈亏总览
        /// </summary>
        public IList<LongQueue> LongQueueList { get; set; }
    }

    public class LongQueue
    {
        /// <summary>
        /// 车号
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// 面
        /// </summary>
        public string Panel { get; set; }

        /// <summary>
        /// 连续期数
        /// </summary>
        public int Queue { get; set; }
    }

    public class LotteryModel
    {
        public string IssueNo { get; set; }

        public string CurrentLotteryNum { get; set; }

        public DateTime CurrentLotteryTime { get; set; }

        public bool IsLottery { get; set; }

        public DateTime CurrentTime { get; set; }
    }

    public class SelectFiveLotteryModel : LotteryModel
    {
        /// <summary>
        /// 单双
        /// </summary>
        public string OddEven { get; set; }
    }

    public class SportsLotteryModel : LotteryModel
    {
        /// <summary>
        /// 和值
        /// </summary>
        public int SumValue { get; set; }

        /// <summary>
        /// 三星组态
        /// </summary>
        public string ThreeStarConfiguration { get; set; }
    }

    public class RealtimeLotteryModel : LotteryModel
    {
        /// <summary>
        /// 前三组
        /// </summary>
        public string ThreeGroupBefore { get; set; }

        /// <summary>
        /// 后三组
        /// </summary>
        public string ThreeGroupAfter { get; set; }
    }

    public class OrderModel
    {
        public DateTime NoteTime { get; set; }

        public string PlayMethod { get; set; }

        public string IssueNo { get; set; }

        public string PlayNums { get; set; }

        public decimal NoteMoney { get; set; }

        public long PlayID { get; set; }
    }

    public class OrderDetailsModel
    {
        public int? UserID { get; set; }

        public string PlayID { get; set; }

        public int IsFactionAward { get; set; }

        public string CurrentLotteryNum { get; set; }

        public string LotteryType { get; set; }

        public decimal? NoteMoney { get; set; }

        public int? NoteNum { get; set; }

        public DateTime? NoteTime { get; set; }

        public string PalyCurrentNum { get; set; }

        public string PalyNum { get; set; }

        public string StFactionAward { get; set; }

        public decimal? WinMoney { get; set; }

        public int? WinNum { set; get; }

        public decimal WinPossMoney { set; get; }

        public decimal? RebatePro { set; get; }

        public string RebateProMoney { get; set; }

        public decimal? SingleMoney { set; get; }

        public string PlayTypeName { get; set; }

        public string PlayTypeRadioName { get; set; }

        public DateTime? CurrentLotteryTime { get; set; }

        public int? Multiple { get; set; }

        public decimal? PrizeMoney { get; set; }

        public decimal? CurrencyUnit { get; set; }

        public int? Ratio { get; set; }

        public string Odds { get; set; }
    }

    /// <summary>
    /// Play info model.
    /// </summary>
    [Serializable]
    public class PlayInfoModel
    {
        public int LotteryID { get; set; }

        public string LotteryTypeName { get; set; }

        public int PlayTypeId { get; set; }

        public string PlayTypeName { get; set; }

        public int PlayTypeRadioID { get; set; }

        public string PlayTypeRadioName { get; set; }

        public int UserType { get; set; }

        public decimal? RebatePro { get; set; }

        public List<PlayTypeRadio> PlayTypeRadios { get; set; }

        public decimal? AddedRebatePro { get; set; }//额外返点

        public int MinNumber { get; set; }

        public int MaxNumber { get; set; }

        public PlayConfig PlayConfig { get; set; }

        public string WinExample { get; set; }

        public string PlayDescription { get; set; }

        public List<PlayOddsModel> PlayOddsModel { get; set; }

        public bool IsDisableAfter { get; set; }

        public PlayInfoModel()
        {
            PlayTypeRadios = new List<PlayTypeRadio>();
            PlayOddsModel = new List<PlayOddsModel>();
        }
    }

    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public class PlayOddsModel
    {
        public string RebatePro { get; set; }

        public string Number { get; set; }

        public decimal RebateProMoney { get; set; }
    }

    /// <summary>
    /// Play type radio model.
    /// </summary>
    [Serializable]
    public class PlayTypeRadioModel
    {
        public string PlayTypeName { get; set; }

        public int PlayTypeRadioID { get; set; }

        public string PlayTypeRadioName { get; set; }

        public string PlayDescription { get; set; }

        public string WinExample { get; set; }

        public string TypeModel { get; set; }
    }

    [Serializable]
    public class PlayTypeNumModel
    {
        public int PlayTypeId { get; set; }

        public int PlayTypeRadioId { get; set; }

        public string PlayTypeNumName { get; set; }
    }

    /// <summary>
    /// Calculate prize model.
    /// </summary>
    public class PlayInfoPrizeModel
    {
        public int LotteryID { get; set; }

        public int PlayTypeID { get; set; }

        /// <summary>
        /// Play type name.
        /// </summary>
        public string PlayTypeName { get; set; }

        public int PlayTypeRadioID { get; set; }

        /// <summary>
        /// Play type radio name(sub item).
        /// </summary>
        public string PlayTypeRadioName { get; set; }

        public int UserType { get; set; }

        /// <summary>
        /// User RebatePro
        /// </summary>
        public float RebatePro { get; set; }

        /// <summary>
        /// 0 or RebatePro
        /// </summary>
        public float HabitRebatePro { get; set; }

        public decimal? AddedRebatePro { get; set; }//额外返点

        public decimal NewAddedRebatePro { get; set; }//额外返点

        public float? CustomerBonusPct { get; set; }
    }

    /// <summary>
    /// Play info post model.
    /// </summary>
    public class PlayInfoPostModel : PlayInfoPrizeModel
    {
        public int PlayType { get; set; }

        public int PlayTypeRadio { get; set; }

        public string CurrentIssueNo { get; set; }

        public string SelectedNums { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Square Feet must be a positive number")]
        public int Amount { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Square Feet must be a positive number")]
        public decimal Price { get; set; }

        /// <summary>
        /// 元角分厘。1：元、0.1：角、0.01：分、0.001：厘
        /// </summary>
        public decimal? CurrencyUnit { get; set; }

        public int? Ratio { get; set; }

        /// <summary>
        /// 0：滿版、1：半版邊看邊玩、其他整數：半版
        /// </summary>
        public string RoomId { get; set; }
    }

    /// <summary>
    /// After details search model.
    /// </summary>
    public class AfterDetailsSearchModel
    {
        /// <summary>
        /// 当期期号
        /// </summary>
        public string CurrentIssueNo { get; set; }

        /// <summary>
        /// 盈利率
        /// </summary>
        public double ProfitRate { get; set; }

        /// <summary>
        /// 单次投注金额，比如是1元或者0.1元
        /// </summary>
        public double BetMoney { get; set; }

        /// <summary>
        /// 单笔投注金额，是这单的总金额
        /// </summary>
        public double SingleMoney { get; set; }

        /// <summary>
        /// 要追的期数
        /// </summary>
        public int Periods { get; set; }

        /// <summary>
        /// 倍数
        /// </summary>
        public int Multiple { get; set; }

        /// <summary>
        /// 间隔期数
        /// </summary>
        public int Interval { get; set; }

        /// <summary>
        /// 奖金金额
        /// </summary>
        public double PrizeMoney { get; set; }

        /// <summary>
        /// 追号类型0-利润率追号,1-同倍追号,2-翻倍追号
        /// </summary>
        public AfterType AfterType { get; set; }
    }

    /// <summary>
    /// After details model.
    /// </summary>
    public class AfterDetailsModel
    {
        public IList<AfterDetailModel> AfterDetails { get; set; }

        public string msg { get; set; }

        public AfterDetailsModel()
        {
            AfterDetails = new List<AfterDetailModel>();
        }
    }

    /// <summary>
    /// After detail model.
    /// </summary>
    public class AfterDetailModel
    {
        /// <summary>
        /// 序号
        /// </summary>
        public int Num { get; set; }

        /// <summary>
        /// 期号
        /// </summary>
        public string IssueNo { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        public double Money { get; set; }

        /// <summary>
        /// 倍数
        /// </summary>
        public long Multiple { get; set; }
    }

    /// <summary>
    /// After info model.
    /// </summary>
    public class AfterInfoModel
    {
        /// <summary>
        /// 下单号码
        /// </summary>
        public string PlayNum { set; get; }

        /// <summary>
        /// 玩法ID
        /// </summary>
        public int? PlayTypeId { set; get; }

        /// <summary>
        /// 单选玩法ID
        /// </summary>
        public int? PlayTypeRadioId { set; get; }

        /// <summary>
        /// 注数
        /// </summary>
        public int? NoteNum { set; get; }

        /// <summary>
        /// 单注金额
        /// </summary>
        public decimal? OrderMoney { set; get; }

        /// <summary>
        /// 是否中奖停止追号
        /// </summary>
        public bool IsWinStop { set; get; }

        /// <summary>
        /// 总期数
        /// </summary>
        public int? TotalPeriods { set; get; }

        /// <summary>
        /// 返点值
        /// </summary>
        public decimal? RebatePro { set; get; }

        /// <summary>
        /// 奖金
        /// </summary>
        public string RebateProMoney { set; get; }

        /// <summary>
        /// 拼接字符串
        /// </summary>
        public string DetailsStr { get; set; }

        public decimal? CustomerBonusPct { get; set; }

        /// <summary>
        /// Currency Unit (1:元, 0.1:分 以此類推)
        /// </summary>
        public decimal? CurrencyUnit { get; set; }

        /// <summary>
        /// Ratio (倍數)
        /// </summary>
        public int? Ratio { get; set; }
    }

    public class ResultModel<T>
    {
        public int ResultCode { get; set; }

        public string ResultStr { get; set; }

        public T ResultData { get; set; }
    }

    public class LotterySearchModel : PagingModel
    {
        public int? Type { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string IssueNo { get; set; }
    }

    public class CurrentLotteryInfoModel
    {
        public string LotteryType { get; set; }

        public string CurrentLotteryNum { get; set; }

        public string IssueNo { get; set; }

        public DateTime UpdateTime { get; set; }
    }

    public class CurrentLotteryInfoListModel : PagingModel
    {
        public DateTime? Start { get; set; }

        public DateTime? End { get; set; }

        public string IssueNo { get; set; }

        public List<CurrentLotteryInfoModel> CurrentLotteryInfoList { get; set; }
    }

    public class LotteryInfoModel
    {
        public int LotteryID { get; set; }

        public string LotteryType { get; set; }

        public string OfficialLotteryUrl { get; set; }

        public string NumberTrendUrl { get; set; }
    }

    public class PlayTypeModel
    {
        public int PlayTypeID { get; set; }

        public string PlayTypeName { get; set; }

        public int LotteryID { get; set; }
    }

    public class LotteryBasicData
    {
        public LotteryInfoModel LotteryInfo { get; set; }

        public List<PlayTypeModel> PlayTypes { get; set; }

        public int MinNumber { get; set; }

        public int MaxNumber { get; set; }
    }

    public class CurrentLotteryQueryModel
    {
        public int LotteryId { get; set; }

        public int Count { get; set; }

        public DateTime? Start { get; set; }

        public DateTime? End { get; set; }

        public string SortBy { get; set; }

        public string StartIssueNo { get; set; }

        public string EndIssueNo { get; set; }
    }
}