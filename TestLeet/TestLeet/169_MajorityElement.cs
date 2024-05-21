namespace TestLeet
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            int[] nums = { 3, 2, 3 };

            object result = new Solution().MajorityElement(nums);

            Assert.AreEqual(3, result);
        }

        [TestMethod]
        public void TestMethod2()
        {
            int[] nums = { 2, 2, 1, 1, 1, 2, 2 };

            object result = new Solution().MajorityElement(nums);

            Assert.AreEqual(2, result);
        }

        public class Solution
        {
            public int MajorityElement(int[] nums)
            {
                int maxValue = 0;
                int half = nums.Length / 2;

                Dictionary<int, int> valuePairs = new Dictionary<int, int>();

                foreach (int num in nums)
                {
                    if (valuePairs.TryGetValue(num, out int times))
                    {
                        times++;
                        valuePairs[num] = times;
                    }
                    else
                    {
                        times = 1;
                        valuePairs.Add(num, times);
                    }

                    if (times > half)
                    {
                        maxValue = num;
                    }
                }

                return maxValue;
            }
        }
    }
}