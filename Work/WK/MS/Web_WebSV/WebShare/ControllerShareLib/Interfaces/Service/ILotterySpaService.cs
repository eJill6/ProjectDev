using ControllerShareLib.Infrastructure.Jobs;
using ControllerShareLib.Interfaces.Model;
using ControllerShareLib.Models;
using ControllerShareLib.Models.Api;
using ControllerShareLib.Models.Base;
using ControllerShareLib.Models.LongResult;
using ControllerShareLib.Models.LotteryPlan;
using ControllerShareLib.Models.SpaPlayConfig;
using JxLottery.Models.Lottery.Bet;
using JxLottery.Services.Adapter;
using SLPolyGame.Web.Model;

namespace ControllerShareLib.Interfaces.Service
{
    public interface ILotterySpaService
    {
        string CancerOrder(int userId, string orderId);

        AfterDetailsModel GetAfterDetails(string id, AfterDetailsSearchModel search, out string resultMsg);

        object GetExtIssueData(string id, string issueNo, out string resultMsg);

        FollowBetResponse GetFollowBet(string palyId, int lotteryId);

        string GetLotteryCountdownTime(int lotteryId);

        LotteryPlayTypeInfo GetLotteryInfo(string lotteryId, ILotteryUserData lotteryUserData);

        LotteryInfo GetLotteryInfoMethod(string lotteryCode);

        Task<CurrentIssueNoViewModel> GetNextIssueNo(int lotteryId);

        Task<CurrentIssueNoViewModel[]> GetNextIssueNos();

        Task<IList<PlayMode<PlayTypeInfoApiResult>>> GetPlayConfig(IBonusAdapter adapter);

        List<string> GetRankList(int days);

        List<WinningListItem> GetRankListItems(int days);

        List<RebateSelectItem> GetRebatePro(UserInfo userInfo, int lotteryId, int playTypeId, int playTypeRadioId, bool? isSingleRebatePro);

        Task<Dictionary<int, LotteryRebateInfo[]>> GetRebatePros(UserInfo userInfo);

        Task<FollowBetResponse> GetSpecifyOrderList(int userId, int? lotteryId, string status, DateTime? searchDate, string cursor, int pageSize, string roomId);

        object GetTodaySummary(int userId, int lotteryId);

        object IssueHistory(int lotteryId, int count, string nextCursor);

        object OrderDetails(string orderId, out int userId);

        PalyInfo PlaceOrder(PlayInfoPostModel model, UserInfo userInfo);

        /// <summary>
        /// 取得長龍資訊
        /// </summary>
        /// <param name="lotteryId">彩種編號</param>
        /// <returns>長龍資訊</returns>
        LongData GetLongData(int lotteryId);

        /// <summary>
        /// 取得跟單計畫資訊
        /// </summary>
        /// <param name="lotteryId">彩種編號</param>
        /// <param name="planType">計畫類別</param>
        /// <returns>跟單計畫資訊</returns>
        LotteryPlanData GetLotteryPlanData(int lotteryId, int planType);

        /// <summary>
        /// 取得長龍資訊
        /// </summary>
        /// <param name="userId">使用者編號</param>
        /// <returns>長龍資訊</returns>
        LongData[] GetLongDatas();

        /// <summary>
        /// 取得跟單計畫資訊
        /// </summary>
        /// <returns>跟單計畫資訊</returns>
        LotteryPlanData[] GetLotteryPlanData();
        LotteryInfo[] GetAllLotteryInfo();
        int[] GetSpecialRuleIds();

        Task<List<UnawardedSummaryApiResponse>> GetUnawardedSummary(UserInfo userInfo,int lotteryId, string roomId, string currentIssueNo);
        
        Task<bool> AddUnawardedSummaryData(UserInfo userInfo, PlayInfoPostModel model);
    }
}