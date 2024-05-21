using BackSideWeb.Models.Enums;

namespace BackSideWeb.Models
{
    public class PokerCard
    {
        public string OriginalNumber { get; }
        public int PokerNumber { get; set; }
        public string Type { get; }
        public CardSuit Suit { get; }
        public string Symbol { get; }
        public bool IsBlack { get; }
        public int Point { get; }

        public PokerCard(string originalNumber, int pokerNumber, string type, CardSuit suit, string symbol, bool isBlack, int point)
        {
            OriginalNumber = originalNumber;
            PokerNumber = pokerNumber;
            Type = type;
            Suit = suit;
            Symbol = symbol;
            IsBlack = isBlack;
            Point = point;
        }
    }
}
