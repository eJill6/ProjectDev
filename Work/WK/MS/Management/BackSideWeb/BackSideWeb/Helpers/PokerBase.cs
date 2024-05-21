using BackSideWeb.Models;
using BackSideWeb.Models.Enums;
using System.Text;

namespace BackSideWeb.Helpers
{
    public class PokerBase
    {
        private static int numberOfSheet = 13;
        private static int totalPerDeck = 52;
        public static string GetCardType(string pokerNumber)
        {
            var number = int.Parse(pokerNumber);
            if (number == 0)
            {
                return "0";
            }
            int cardType = number % numberOfSheet;
            cardType = (cardType != 0) ? cardType : numberOfSheet;

            return cardType switch
            {
                13 => "K",
                12 => "Q",
                11 => "J",
                1 => "A",
                _ => cardType.ToString(),
            };
        }
        public static int GetCardNumber(string pokerNumber)
        {
            var number = int.Parse(pokerNumber);
            return (number == 0) ? 0 : (number % numberOfSheet != 0) ? number % numberOfSheet : numberOfSheet;
        }
        public static string GetSymbol(CardSuit suit)
        {
            return suit switch
            {
                CardSuit.Diamond => "♦",
                CardSuit.Club => "♣",
                CardSuit.Heart => "♥",
                CardSuit.Spades => "♠",
                _ => string.Empty,
            };
        }
        public static CardSuit GetCardSuit(string pokerNumber)
        {
            var number = int.Parse(pokerNumber);
            return (number == 0) ? CardSuit.Zero : (CardSuit)((number - 1) / numberOfSheet + 1);
        }
        public static void AppendCards(List<PokerCard> cards, StringBuilder resultBuilder)
        {
            foreach (var card in cards)
            {
                resultBuilder.Append("<span style=\"font-size:16px;");
                resultBuilder.Append(card.IsBlack ? "\">" : " color:red;\">");
                resultBuilder.Append($"{card.Symbol}{card.Type}</span>,");
            }
        }
        public static List<PokerCard> GetPokerCards(List<string> pokers)
        {
            return pokers.Select(pokerNo =>
            {
                var normalizedPokerNo = ((int.Parse(pokerNo) - 1) % totalPerDeck + 1).ToString();
                var suit = GetCardSuit(normalizedPokerNo);
                var cardNumber = GetCardNumber(normalizedPokerNo);
                return new PokerCard(
                    normalizedPokerNo,
                    cardNumber,
                    GetCardType(normalizedPokerNo),
                    suit,
                    GetSymbol(suit),
                    suit == CardSuit.Spades || suit == CardSuit.Club || suit == CardSuit.Zero,
                    cardNumber > 10 ? 10 : cardNumber
                );
            }).ToList();
        }
    }
}