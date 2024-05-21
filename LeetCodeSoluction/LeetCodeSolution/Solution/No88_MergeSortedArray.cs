using System;

namespace LeetCodeSolution.Solution
{
    public class No88_MergeSortedArray : BaseSolution
    {
        public override object GetResult()
        {
            int[] nums1 = new int[] { 1, 2, 3, 0, 0, 0 };
            int m = 3;
            int[] nums2 = new int[] { 2, 5, 6 };
            int n = 3;

            //int[] nums1 = new int[] { 1 };
            //int m = 1;
            //int[] nums2 = new int[] { };
            //int n = 0;

            return new Solution().Merge(nums1, m, nums2, n);
        }

        public class Solution
        {
            public int[] Merge(int[] nums1, int m, int[] nums2, int n)
            {
                for (int i = 0; i < m; i++)
                {
                    nums1[i] = nums1[i];
                }

                for (int i = 0; i < n; i++)
                {
                    nums1[m + i] = nums2[i];
                }

                Array.Sort(nums1);

                return nums1;
            }
        }
    }
}