using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LeetCodeSoluction.Solution
{
    public class No1002_FindCommonCharacters : BaseSolution
    {
        public override object GetResult()
        {
            List<string> words = new List<string>() { "bella", "label", "roller" };

            return new Solution().CommonChars(words.ToArray());
        }

        public class Solution
        {
            public IList<string> CommonChars(string[] words)
            {
                IList<string> result = new List<string>();
                string baseWord = words.First();
                List<Dictionary<char, int>> charPosIndexList = new List<Dictionary<char, int>>();

                for (int i = 1; i < words.Length; i++)
                {
                    charPosIndexList.Add(new Dictionary<char, int>());
                }

                foreach (char letter in baseWord)
                {
                    bool isFound = true;

                    for (int i = 1; i < words.Length; i++)
                    {
                        Dictionary<char, int> charPosIndexMap = charPosIndexList[i - 1];

                        if (!charPosIndexMap.TryGetValue(letter, out int lastIndex))
                        {
                            lastIndex = -1;
                            charPosIndexMap.Add(letter, lastIndex);
                        }

                        int index = words[i].IndexOf(letter, lastIndex + 1);

                        if (index > lastIndex)
                        {
                            charPosIndexMap[letter] = index;

                            continue;
                        }
                        else
                        {
                            isFound = false;
                            break;
                        }
                    }

                    if (isFound)
                    {
                        result.Add(letter.ToString());
                    }
                }

                return result;
            }
        }
    }
}