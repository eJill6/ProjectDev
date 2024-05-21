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
                if (n < 2)
                {
                    return 1;
                }

                List<int> steps = new List<int>() { 1, 2 };
                int prev = 1;
                int result = 1;

                // f(n) = f(n-1) + f(n-2)
                for (int i = 2; i <= steps.Count; i++)
                {
                    int c = result;
                    result = result + prev;
                    prev = c;
                }

                return result;
            }
        }
    }
}