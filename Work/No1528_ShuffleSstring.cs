using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LeetCodeSoluction.Solution
{
    public class No1528_ShuffleSstring : BaseSolution
    {
        public override object GetResult()
        {
            string s = "codeleet";
            List<int> indices = new List<int>() { 4, 5, 6, 7, 0, 2, 1, 3 };

            return new Solution().RestoreString(s, indices.ToArray());
        }

        public class Solution
        {
            public string RestoreString(string s, int[] indices)
            {
                var sortText = new string[s.Length];

                for (int i = 0; i < indices.Length; i++)
                {
                    int textIndex = indices[i];
                    sortText[textIndex] = s[i].ToString();
                }

                return string.Join(string.Empty, sortText);
            }
        }
    }
}