using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LeetCodeSolution.Solution
{
    public class No11_ContainerWithMostWater  : BaseSolution
    {
        public override object GetResult()
        {
            int[] input = { 1, 8, 6, 2, 5, 4, 8, 3, 7 };
            return new Solution().MaxArea(input);
        }

        public class Solution
        {
            public int MaxArea(int[] height)
            {
                if (height.Count() == 0)
                {
                    return 0;
                }

                int index = 0;
                int lastIndex = height.Count()-1;
                int cheight = 0;
                int width = 0;
                int maxArea = 0;
                

                while (index < lastIndex)
                {
                    width = lastIndex - index;
                    cheight = Math.Min(height[index], height[lastIndex]);
                    maxArea = Math.Max(maxArea, cheight * width);

                    if (height[index] < height[lastIndex])
                    {
                        index++;
                    }
                    else
                    {
                        lastIndex--;
                    }

                }

                return maxArea;

            }
        }
    }
}
