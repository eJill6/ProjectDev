using System.Text;
using System.Text.RegularExpressions;

namespace TestLeet
{
    [TestClass]
    public class ValidPalindrome
    {
        [TestMethod]
        public void TestMethod1()
        {
            string s = "A man, a plan, a canal: Panama";

            object result = new Solution().IsPalindrome(s);

            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void TestMethod2()
        {
            string s = "race a car";

            object result = new Solution().IsPalindrome(s);

            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void TestMethod3()
        {
            string s = " ";

            object result = new Solution().IsPalindrome(s);

            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void TestMethod4()
        {
            string s = "0P";

            object result = new Solution().IsPalindrome(s);

            Assert.AreEqual(false, result);
        }

        public class Solution
        {
            public bool IsPalindrome(string s)
            {
                string pattern = "[a-zA-Z0-9]+";
                var stringBuilder = new StringBuilder();

                MatchCollection matches = Regex.Matches(s.ToLower(), pattern);

                foreach (Match match in matches)
                {
                    stringBuilder.Append(match.Value);
                }

                string word = stringBuilder.ToString();

                if (string.IsNullOrEmpty(word))
                {
                    return true;
                }

                return new string(word.Reverse().ToArray()) == word;
            }
        }
    }
}