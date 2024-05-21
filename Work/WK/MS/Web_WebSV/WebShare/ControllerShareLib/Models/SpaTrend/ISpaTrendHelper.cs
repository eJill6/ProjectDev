using SLPolyGame.Web.Model;

namespace ControllerShareLib.Models.SpaTrend
{
    public interface ISpaTrendHelper
    {
        int GameTypeId { get; }

        IEnumerable<dynamic> PrepareTrend(IEnumerable<CurrentLotteryInfo> source);

        CurrentLotteryQueryInfo GetQueryInfo(int searchType);
    }
}