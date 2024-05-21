using SLPolyGame.Web.Interface;
using SLPolyGame.Web.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Models.Base;

namespace Web.Services
{
    public interface ILotteryService
    {
        List<LotteryInfo> GetLotteryType();

        List<PlayTypeInfo> GetPlayTypeInfo();

        List<PlayTypeRadio> GetPlayTypeRadio();

        List<LotteryInfo> GetAllLotteryInfo();

        CurrentLotteryInfo GetLotteryInfos(int lotteryID, int userID);

        List<CurrentLotteryInfo> QueryCurrentLotteryInfo(CurrentLotteryQueryInfo query);

        SLPolyGame.Web.Model.CursorPagination<CurrentLotteryInfo> GetCursorPaginationDrawResult(int lotteryId, DateTime start, DateTime end, int count, string cursor);

        TodaySummaryInfo GetTodaySummaryInfo(int lotteryID, int count, bool? LoadType);
    }
}