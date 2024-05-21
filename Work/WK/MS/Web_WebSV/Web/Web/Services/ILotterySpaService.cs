using JxBackendService.Interface.Model.User;
using JxLottery.Models.Lottery.Bet;
using JxLottery.Services.Adapter;
using SLPolyGame.Web.Model;
using System;
using System.Collections.Generic;
using Web.Models;
using Web.Models.Base;
using Web.Models.LongResult;
using Web.Models.LotteryPlan;
using Web.Models.SpaPlayConfig;

namespace Web.Services
{
    public interface ILotterySpaService
    {
        string CancerOrder(int userId, string orderId);

        AfterDetailsModel GetAfterDetails(string id, AfterDetailsSearchModel search, out string resultMsg);

        object GetExtIssueData(string id, string issueNo, out string resultMsg);

        FollowBetResponse GetFollowBet(string palyId, int lotteryId);

        string GetLotteryCountdownTime(int userId, int lotteryId);

        LotteryPlayTypeInfo GetLotteryInfo(string lotteryId, ILotteryUserData lotteryUserData);

        LotteryInfo GetLotteryInfoMethod(string lotteryCode);

        object GetNextIssueNo(int userId, int lotteryId);

        IList<PlayMode<PlayTypeInfoApiResult>> GetPlayConfig(IBonusAdapter adapter, int userId);

        List<string> GetRankList(int days);

        List<WinningListItem> GetRankListItems(int days);

        List<RebateSelectItem> GetRebatePro(UserInfo userInfo, int lotteryId, int playTypeId, int playTypeRadioId, bool? isSingleRebatePro);

        FollowBetResponse GetSpecifyOrderList(int userId, int lotteryId, string status, DateTime? searchDate, string cursor, int pageSize);

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
    }
}