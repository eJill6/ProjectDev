using SLPolyGame.Web.Model;
using System;
using System.Collections.Generic;

namespace Web.Services
{
    public interface IPlayInfoService
    {
        /// <summary>
        /// 时时彩下单请求
        /// </summary>
        /// <param name="playInfo">PalyInfo实体</param>
        /// <returns>返回信息是一个palyinfo实体，可通过实体参数UserName判断下单情况</returns>
        PalyInfo InsertPlayInfo(PalyInfo playInfo);

        /// <summary>
        /// 获取单个时时彩单的信息
        /// </summary>
        /// <param name="playId">单编号</param>
        /// <returns>PalyInfo实体，里边包含单个单的信息</returns>
        PalyInfo GetPlayBet(string playId);

        string CancerOrder(int userId, string payId);

        DateTime GetServerCurrentTime();

        List<WinningListItem> GetLatestWinningListItems(string period);

        List<string> GetLatestWinningList();

        List<string> GetLatestWinningWeekList();

        List<string> GetLatestWinningMonthList();

        SLPolyGame.Web.Model.CursorPagination<PalyInfo> GetSpecifyOrderList(int userId, int lotteryId, string status, DateTime searchDate, string cursor, int pageSize);

        SLPolyGame.Web.Model.CursorPagination<PalyInfo> GetFollowBet(string palyId, int lottertId);

        List<PalyInfo> GetPlayBetsByAnonymous(string startTime, string endTime, string gameId);

        PalyInfo GetPlayBetByAnonymous(string playId);
    }
}