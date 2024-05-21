using SLPolyGame.Web.Model;
using System.Collections.Generic;
using Web.Models.Base;

namespace Web.Models.SpaTrend
{
    public interface ISpaTrendHelper
    {
        int GameTypeId { get; }

        IEnumerable<dynamic> PrepareTrend(IEnumerable<CurrentLotteryInfo> source);

        CurrentLotteryQueryInfo GetQueryInfo(int searchType);
    }
}