using System.Text;

namespace TestLeet
{
    [TestClass]
    public class LongestCommonPrefix
    {
        [TestMethod]
        public void TestMethod1()
        {
            string[] strs = new string[] { "flower", "flow", "flight" };

            object result = new Solution().LongestCommonPrefix(strs);

            Assert.AreEqual("fl", result);
        }

        [TestMethod]
        public void TestMethod2()
        {
            string[] strs = new string[] { "dog", "racecar", "car" };

            object result = new Solution().LongestCommonPrefix(strs);

            Assert.AreEqual("", result);
        }

        [TestMethod]
        public void TestMethod3()
        {
            string[] strs = new string[] { "flower", "flower", "flower", "flower" };

            object result = new Solution().LongestCommonPrefix(strs);

            Assert.AreEqual("flower", result);
        }

        public class Solution
        {
            public string LongestCommonPrefix(string[] strs)
            {
                string fixWorld = strs[0];

                if (strs.Length == 1)
                {
                    return fixWorld;
                }

                string result = string.Empty;
                int indx = 0;

                while (indx < fixWorld.Length)
                {
                    string words = fixWorld.Substring(0, indx + 1);
                    int match = 0;

                    for (int i = 0; i < strs.Length; i++)
                    {
                        if (strs[i].StartsWith(words))
                        {
                            match++;
                        }

                        if (match == strs.Length)
                        {
                            result = words;
                        }
                    }

                    indx++;
                }

                return result;
            }
        }
    }
}