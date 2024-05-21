namespace TestLeet
{
    [TestClass]
    public class BestTimetoBuyandSellStock
    {
        [TestMethod]
        public void TestMethod1()
        {
            int[] nums = { 7, 1, 5, 3, 6, 4 };

            object result = new Solution().MaxProfit(nums);

            Assert.AreEqual(5, result);
        }

        [TestMethod]
        public void TestMethod2()
        {
            int[] nums = { 7, 6, 4, 3, 1 };

            object result = new Solution().MaxProfit(nums);

            Assert.AreEqual(0, result);
        }

        public class Solution
        {
            public int MaxProfit(int[] prices)
            {
                int maxValue = 0;
                int minProfit = prices[0];

                for (int i = 0; i < prices.Length; i++)
                {
                    if (prices[i] < minProfit)
                    {
                        minProfit = prices[i];
                    }

                    int miles = prices[i] - minProfit;

                    if (miles > maxValue)
                    {
                        maxValue = miles;
                    }
                }

                return maxValue;
            }
        }

        //public class Solution
        //{
        //    public int MaxProfit(int[] prices)
        //    {
        //        int maxValue = 0;

        //        for (int i = 0; i < prices.Length; i++)
        //        {
        //            for (int j = i + 1; j < prices.Length; j++)
        //            {
        //                int stract = prices[j] - prices[i];

        //                if (stract > 0 && stract > maxValue)
        //                {
        //                    maxValue = stract;
        //                }
        //            }
        //        }

        //        return maxValue;
        //    }
        //}
    }
}