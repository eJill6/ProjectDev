using System.Collections.Generic;

namespace LeetCodeSolution.Solution
{
    public class No70_ClimbingStairs : BaseSolution
    {
        public override object GetResult()
        {
            int val = 2;

            return new Solution().ClimbStairs(val);
        }

        public class Solution
        {
            public int ClimbStairs(int n)
            {
                List<int> steps = new List<int>() { 1, 2 };
                int result = 0;

                foreach (int step in steps)
                {
                }
            }
        }
    }
}