using System.Text;

namespace TestLeet
{
    [TestClass]
    public class FindTheIndexOfTheFirstOccurenceInAString
    {
        [TestMethod]
        public void TestMethod1()
        {
            string haystack = "sadbutsad";
            string needle = "sad";

            object result = new Solution().StrStr(haystack, needle);

            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void TestMethod2()
        {
            string haystack = "leetcode";
            string needle = "leeto";

            object result = new Solution().StrStr(haystack, needle);

            Assert.AreEqual(-1, result);
        }

        public class Solution
        {
            public int StrStr(string haystack, string needle)
            {
                int needLen = needle.Length;

                for (int index = 0; index <= haystack.Length - needLen; index++)
                {
                    if (haystack.Substring(index, needLen).ToLower() == needle.ToLower())
                    {
                        return index;
                    }
                }

                return -1;
            }

            //public int StrStr(string haystack, string needle)
            //{
            //    int match = 0;
            //    int index = 0;
            //    StringBuilder stringBuilder = new StringBuilder();
            //    int startIndex = 0;

            //    while (index < needle.Length)
            //    {
            //        char position = needle[index];
            //        stringBuilder.Append(position);

            //        if (haystack.Contains(stringBuilder.ToString()))
            //        {
            //            startIndex = index;
            //        }

            //        index++;
            //    }

            //    return match;
            //}
        }
    }
}