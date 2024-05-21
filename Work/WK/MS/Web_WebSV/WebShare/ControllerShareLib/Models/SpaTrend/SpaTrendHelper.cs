using SLPolyGame.Web.Model;

namespace ControllerShareLib.Models.SpaTrend
{
    public abstract class SpaTrendHelper : ISpaTrendHelper
    {
        public abstract int GameTypeId { get; }

        public virtual CurrentLotteryQueryInfo GetQueryInfo(int searchType)
        {
            var result = new CurrentLotteryQueryInfo
            {
                Count = 10000,
            };
            if (searchType == 0)
            {
                result.Start = DateTime.Now;
                result.End = DateTime.Now;
            }
            else if (searchType == 1)
            {
                result.Count = 30;
            }
            else if (searchType == 2)
            {
                result.Count = 100;
            }
            else if (searchType == 3)
            {
                result.Start = DateTime.Now.AddDays(-2);
                result.End = DateTime.Now;
            }
            else if (searchType == 4)
            {
                result.Start = DateTime.Now.AddDays(-5);
                result.End = DateTime.Now;
            }

            return result;
        }

        public abstract IEnumerable<dynamic> PrepareTrend(IEnumerable<CurrentLotteryInfo> source);
    }
}