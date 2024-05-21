using BackSideWeb.Models;
using System.ComponentModel;

namespace BackSideWeb.Helpers
{
    public class NuiNuiGame : PokerBase
    {
        public class VictoryConditions
        {
            public int MaxPokerNumber { get; set; }
            public int MaxOriginalNumber { get; set; }
            public NuiNuiWeight Weight { get; set; }

            public VictoryConditions(int maxPokerNumber, int maxOriginalNumber, NuiNuiWeight weight)
            {
                MaxPokerNumber = maxPokerNumber;
                MaxOriginalNumber = maxOriginalNumber;
                Weight = weight;
            }
        }

        public class PokerResult
        {
            public List<PokerCard> Cards { get; set; }
            public bool IsWin { get; set; }
            public string ImageType { get; set; }
            public VictoryConditions VictoryConditions { get; set; }

            public PokerResult(List<PokerCard> cards, bool isWin, string imageType, VictoryConditions victoryConditions)
            {
                Cards = cards;
                IsWin = isWin;
                ImageType = imageType;
                VictoryConditions = victoryConditions;
            }
        }

        public enum NuiNuiWeight
        {
            [Description("无牛")]
            NoNui = 0,
            [Description("牛一")]
            Nui1,
            [Description("牛二")]
            Nui2,
            [Description("牛三")]
            Nui3,
            [Description("牛四")]
            Nui4,
            [Description("牛五")]
            Nui5,
            [Description("牛六")]
            Nui6,
            [Description("牛七")]
            Nui7,
            [Description("牛八")]
            Nui8,
            [Description("牛九")]
            Nui9,
            [Description("牛牛")]
            NuiNui,
            [Description("花色牛")]
            SuitNui
        }

        public class NuiNui
        {
            private int numberOfSheet = 13; // One set of cards
            private int total = 10; // Basic total number of cards
            private int areaTotal = 5; // Basic number of cards per area
            private int baseCalculateCombo = 3; // Basic combination calculation; if a number is less than 10, select three cards to check if they sum to 10
            private int baseToNuiNuiParameter = 10; // Base requirement for having a NuiNui (a low card or a combination that sums to 10)

            public List<PokerResult> ConfirmResult(List<string> pokers, bool isShowResult = true)
            {
                if (pokers.Count != total)
                {
                    throw new Exception("总牌数错误");
                }

                var blueArea = SortCardType(pokers.GetRange(0, areaTotal));
                var redArea = SortCardType(pokers.GetRange(areaTotal, areaTotal));

                if (isShowResult)
                {
                    var blueScore = (int)blueArea.VictoryConditions.Weight;
                    var redScore = (int)redArea.VictoryConditions.Weight;

                    if (blueScore == redScore)
                    {
                        var blueMaxPokerNumber = blueArea.VictoryConditions.MaxPokerNumber;
                        var redMaxPokerNumber = redArea.VictoryConditions.MaxPokerNumber;

                        blueScore = (blueMaxPokerNumber == redMaxPokerNumber) ? blueArea.VictoryConditions.MaxOriginalNumber : blueMaxPokerNumber;
                        redScore = (blueMaxPokerNumber == redMaxPokerNumber) ? redArea.VictoryConditions.MaxOriginalNumber : redMaxPokerNumber;
                    }

                    blueArea.IsWin = blueScore > redScore;
                    redArea.IsWin = blueScore < redScore;
                }

                return new List<PokerResult> { blueArea, redArea };
            }

            public PokerResult SortCardType(List<string> pokers)
            {
                if (pokers.Count != areaTotal)
                {
                    throw new Exception("Card count for one side is incorrect");
                }

                var result = new PokerResult(
                    new List<PokerCard>(),
                    false,
                    "0",
                    new VictoryConditions(0, 0, NuiNuiWeight.NoNui)
                );

                result.Cards = GetPokerCards(pokers);

                result.VictoryConditions = GetNuiNuiConditions(result.Cards);

                var imageType = "q";

                if (result.VictoryConditions.Weight == NuiNuiWeight.NuiNui)
                {
                    imageType = "s";
                }
                else if ((int)result.VictoryConditions.Weight < (int)NuiNuiWeight.NuiNui)
                {
                    imageType = ((int)result.VictoryConditions.Weight).ToString();
                }

                result.ImageType = result.Cards[0].OriginalNumber == "0" ? "" : imageType;

                return result;
            }

            public VictoryConditions GetNuiNuiConditions(List<PokerCard> cards)
            {
                var maxNumber = cards.Max(item => item.PokerNumber);
                var maxNumberElements = cards.Where(item => item.PokerNumber == maxNumber).ToList();
                var maxOriginalNumber = maxNumberElements.Max(item => int.Parse(item.OriginalNumber));

                var condition = new VictoryConditions(maxNumber, maxOriginalNumber, NuiNuiWeight.NoNui);

                var isSuitNui = cards.All(item => item.PokerNumber > baseToNuiNuiParameter);

                if (isSuitNui)
                {
                    condition.Weight = NuiNuiWeight.SuitNui;
                    return condition;
                }

                var copyCards = cards.ToList();

                for (int i = 0; i < copyCards.Count - 2; i++)
                {
                    for (int j = i + 1; j < copyCards.Count - 1; j++)
                    {
                        for (int k = j + 1; k < copyCards.Count; k++)
                        {
                            var item1 = (copyCards[i].PokerNumber > baseToNuiNuiParameter) ? baseToNuiNuiParameter : copyCards[i].PokerNumber;
                            var item2 = (copyCards[j].PokerNumber > baseToNuiNuiParameter) ? baseToNuiNuiParameter : copyCards[j].PokerNumber;
                            var item3 = (copyCards[k].PokerNumber > baseToNuiNuiParameter) ? baseToNuiNuiParameter : copyCards[k].PokerNumber;

                            var isNui = (item1 + item2 + item3) % 10 == 0;

                            if (isNui)
                            {
                                copyCards.RemoveAt(k);
                                copyCards.RemoveAt(j);
                                copyCards.RemoveAt(i);

                                var result = copyCards.Sum(item => (item.PokerNumber > baseToNuiNuiParameter) ? 10 : item.PokerNumber);
                                condition.Weight = (result % 10 == 0) ? NuiNuiWeight.NuiNui : (NuiNuiWeight)(result % 10);
                                return condition;
                            }
                        }
                    }
                }

                condition.Weight = NuiNuiWeight.NoNui;
                return condition;
            }
        }
    }
}