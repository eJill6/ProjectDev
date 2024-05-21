using BackSideWeb.Models;
using System.Text;

namespace BackSideWeb.Helpers
{
    public class JssgGame : PokerBase
    {
        private static int digitCount = 6;
        public static string GetPokerString(string poker)
        {
            if (string.IsNullOrEmpty(poker))
            {
                return string.Empty;
            }

            List<string> pokers = poker.Split(',').ToList();

            if (pokers.Count != digitCount)
            {
                return poker;
            }

            List<PokerCard> pokerCardList = GetPokerCards(pokers);
            var resultBuilder = new StringBuilder();

            void AppendPlayerCards(List<PokerCard> cards, string playerLabel)
            {
                resultBuilder.Append($"{playerLabel}：");
                AppendCards(cards, resultBuilder);
                // 判断是否是公
                bool isTripleKingQueenJack = cards.All(card => card.PokerNumber >= 11 && card.PokerNumber <= 13);
                bool isDoubleKingQueenJack = cards.Count(card => card.PokerNumber >= 11 && card.PokerNumber <= 13) == 2;
                bool isSingleKingQueenJack = cards.Count(card => card.PokerNumber >= 11 && card.PokerNumber <= 13) == 1;

                // 将点数加起来并取点数
                int sum = cards.Sum(card => card.Point) % 10;

                // 根据不同情况呈现结果
                if (isTripleKingQueenJack)
                {
                    resultBuilder.Append("三公");
                }
                else if (isDoubleKingQueenJack)
                {
                    resultBuilder.Append("双公");
                    resultBuilder.Append(sum);
                }
                else if (isSingleKingQueenJack)
                {
                    resultBuilder.Append("单公");
                    resultBuilder.Append(sum);
                }
                else
                {
                    resultBuilder.Append($"{sum}点"); ;
                }
                resultBuilder.Append("<br>");
            }
            AppendPlayerCards(pokerCardList.Take(3).ToList(), "庄家");
            AppendPlayerCards(pokerCardList.Skip(Math.Max(0, pokerCardList.Count - 3)).ToList(), "闲家");

            return resultBuilder.ToString();
        }
    }
}
