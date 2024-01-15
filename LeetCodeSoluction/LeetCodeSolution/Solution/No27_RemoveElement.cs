using System.Collections.Generic;

namespace LeetCodeSolution.Solution
{
    public class No27_RemoveElement : BaseSolution
    {
        public override object GetResult()
        {
            int[] nums = new int[] { 3, 2, 2, 3 };
            int val = 2;

            return new Solution().RemoveElement(nums, val);
        }

        public class Solution
        {
            public int RemoveElement(int[] nums, int val)
            {
                List<int> result = new List<int>();

                foreach (int i in nums)
                {
                    if (i != val)
                    {
                        result.Add(i);
                    }
                }

                return result.Count;
            }
        }
    }
}