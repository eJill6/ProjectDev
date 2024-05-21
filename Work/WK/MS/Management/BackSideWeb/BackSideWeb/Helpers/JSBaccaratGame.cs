using BackSideWeb.Models;
using System.Text;

namespace BackSideWeb.Helpers
{
    public class JSBaccaratGame : PokerBase
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
                resultBuilder.Append($"{cards.Sum(f => f.Point) % 10}点<br>");
            }

            AppendPlayerCards(pokerCardList.Take(3).ToList(), "庄家");
            AppendPlayerCards(pokerCardList.Skip(Math.Max(0, pokerCardList.Count - 3)).ToList(), "闲家");

            return resultBuilder.ToString();
        }
    }
}