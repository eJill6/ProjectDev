using BackSideWeb.Models;
using System.Text;

namespace BackSideWeb.Helpers
{
    public class JslhGame : PokerBase
    {
        public static string GetPokerString(string poker)
        {
            if (string.IsNullOrEmpty(poker))
            {
                return string.Empty;
            }
            List<string> pokers = poker.Split(',').ToList();

            List<PokerCard> pokerCardList = GetPokerCards(pokers);

            var resultBuilder = new StringBuilder();

            void AppendPlayerCards(List<PokerCard> cards, string playerLabel)
            {
                resultBuilder.Append($"{playerLabel}：");
                AppendCards(cards, resultBuilder);
                resultBuilder.Length -= 1;
                resultBuilder.Append("<br>");
            }
            AppendPlayerCards(pokerCardList.Take(1).ToList(), "龙");
            AppendPlayerCards(pokerCardList.Skip(Math.Max(0, pokerCardList.Count - 1)).ToList(), "虎");

            return resultBuilder.ToString();
        }
    }
}
