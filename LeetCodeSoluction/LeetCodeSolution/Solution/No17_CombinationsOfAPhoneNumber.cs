using System.Collections.Generic;

namespace LeetCodeSolution.Solution
{
    public class No17_CombinationsOfAPhoneNumber : BaseSolution
    {
        public override object GetResult()
        {
            return new Solution().LetterCombinations("237");
        }

        public class Solution
        {
            private List<string> finalResult = new List<string>();

            private readonly Dictionary<int, List<string>> keyValuePairs = new Dictionary<int, List<string>>
            {
                //{0, new List<string>{" " } },
                //{1, new List<string>{"" } },
                {2,  new List<string>{ "a", "b", "c" }},
                {3,  new List<string>{"d","e","f" } },
                {4,  new List<string>{"g", "h", "i" } },
                {5,  new List<string>{ "j", "k", "l" } },
                {6,  new List<string>{ "m", "n", "o" } },
                {7,  new List<string>{ "p", "q", "r", "s" } },
                {8,  new List<string>{ "t", "u", "v" } },
                {9,  new List<string>{ "w", "x", "y", "z" } },
            };

            public IList<string> LetterCombinations(string digits)
            {
                if (digits == null || digits.Length == 0)
                {
                    return finalResult;
                }

                var numbers = new List<int>();

                foreach (char ch in digits)
                {
                    if (int.TryParse(ch.ToString(), out int number))
                    {
                        numbers.Add(number);
                    }
                }

                var mapValues = new List<List<string>>();

                foreach (int num in numbers)
                {
                    mapValues.Add(keyValuePairs[num]);
                }

                finalResult.Add(string.Empty);

                for (int i = 0; i < mapValues.Count; i++)
                {
                    var list = new List<string>();

                    foreach (string content in finalResult)
                    {
                        for (int j = 0; j < mapValues[i].Count; j++)
                        {
                            string temp = content + mapValues[i][j];
                            list.Add(temp);
                        }
                    }
                    finalResult = list;
                }

                return finalResult;
            }
        }
    }
}