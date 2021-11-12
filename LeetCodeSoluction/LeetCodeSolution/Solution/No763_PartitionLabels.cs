using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LeetCodeSoluction.Solution
{
    public class No763_PartitionLabels : BaseSolution
    {
        public override object GetResult()
        {
            string inputValue = "eccbbbbdec";
            return new Solution().PartitionLabels(inputValue);
        }

        public class Solution
        {
            public IList<int> PartitionLabels(string s)
            {
                Dictionary<char, List<int>> keyValuePairs = new Dictionary<char, List<int>>();

                for (int i = 0; i < s.Length; i++)
                {
                    char tmp = s[i];

                    if (!keyValuePairs.ContainsKey(s[i]))
                    {
                        keyValuePairs.Add(tmp, new List<int> { i });
                    }
                    else
                    {
                        keyValuePairs[tmp].Add(i);
                    }                    
                }

                int min = keyValuePairs.ElementAt(0).Value.Min();
                int max = keyValuePairs.ElementAt(0).Value.Max();
                List<int> result = new List<int>();
                int total = keyValuePairs.Count;

                for (int i = 1; i < total; i++)
                {
                    var tmp = keyValuePairs.ElementAt(i);
                    List<int> tmpList = tmp.Value;
                    int tmpMin = tmpList.First();
                    int tmpMax = tmpList.Last();

                    if (max < tmpMin)
                    {
                        result.Add(max - min + 1);
                        min = tmpMin;
                        max = tmpMax;
                    }
                    else if(max < tmpMax)
                    {
                        max = tmpMax;
                    }

                    if (i == total - 1)
                    {
                        result.Add(max - min + 1);
                    }
                }

                return result;
            }
        }
    }
}
