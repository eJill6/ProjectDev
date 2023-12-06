using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LeetCodeSoluction.Solution
{
    public class No2696_MinStrLenAfterRemovingSubstrings : BaseSolution
    {
        public override object GetResult()
        {
            string s = "ABFCACDB";

            return new Solution().MinLength(s);
        }

        public class Solution
        {
            public int MinLength(string s)
            {
                var patterns = new List<string>() { "AB", "CD" };

                while (true)
                {
                    bool isNoPatternMatch = true;

                    foreach (string pattern in patterns)
                    {
                        if (s.IndexOf(pattern) >= 0)
                        {
                            isNoPatternMatch = false;
                            s = s.Replace(pattern, string.Empty);
                        }
                    }

                    if (isNoPatternMatch)
                    {
                        break;
                    }
                }

                return s.Length;
            }
        }
    }
}