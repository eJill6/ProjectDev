using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LeetCodeSoluction.Solution
{
    public class No1470_ShuffleTheArray : BaseSolution
    {
        public override object GetResult()
        {
            int[] nums = { 2, 5, 1, 3, 4, 7 };
            int n = 3;

            return new Solution().Shuffle(nums, n);
        }

        public class Solution
        {
            public int[] Shuffle(int[] nums, int n)
            {
                int xStartIndex = 0;
                int yStartIndex = nums.Length / 2;
                int[] shuffleResult = new int[n * 2];

                for (int i = 0; i < nums.Length; i++)
                {
                    shuffleResult[i] = nums[xStartIndex];
                    i++;
                    shuffleResult[i] = nums[yStartIndex];

                    xStartIndex++;
                    yStartIndex++;
                }

                return shuffleResult;
            }
        }
    }
}