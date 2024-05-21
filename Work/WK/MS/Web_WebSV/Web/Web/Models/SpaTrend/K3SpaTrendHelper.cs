using SLPolyGame.Web.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Web.Models.SpaTrend
{
    public class K3SpaTrendHelper : SpaTrendHelper
    {
        public override int GameTypeId => (int)JxLottery.Models.Lottery.GameTypeId.K3;

        public override IEnumerable<dynamic> PrepareTrend(IEnumerable<CurrentLotteryInfo> source)
        {
            if (source == null)
            {
                return new List<dynamic>();
            }

            return source.Select(item =>
            {
                string[] numbers = item.CurrentLotteryNum.Split(',');
                int[] integerNumbers = numbers.Select(int.Parse).ToArray();
                int sum = integerNumbers.Sum(x => Convert.ToInt32(x));
                bool isXiao = (3 <= sum && sum <= 10);
                bool isShuang = (sum % 2) == 0;

                return new K3TrendDetailModel()
                {
                    Issue = item.IssueNo,
                    Numbers = numbers,
                    Ge = numbers[2],
                    Shi = numbers[1],
                    Bai = numbers[0],
                    DaXiao = isXiao ? "小" : "大",
                    IsDa = !isXiao,
                    DanShuang = isShuang ? "双" : "单",
                    IsDan = !isShuang,
                    HeZhiFenBu = sum.ToString(),
                    KuaWei = (integerNumbers.Max() - integerNumbers.Min()).ToString()
                };
            }).Cast<dynamic>().ToList();
        }
    }
}