using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LeetCodeSoluction.Solution
{
    public class No22_GenerateParentheses : BaseSolution
    {
        public override object GetResult()
        {

            return new Solution().GenerateParenthesis(3);
            //return null;
        }

        public class Solution
        {
            public IList<string> GenerateParenthesis(int n)
            {
                return new List<string>();
            }
        }
    }
}
