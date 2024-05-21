namespace TestLeet
{
    [TestClass]
    public class LengthofLastWord
    {
        [TestMethod]
        public void TestMethod1()
        {
            string sentence = "Hello World";

            object result = new Solution().LengthOfLastWord(sentence);

            Assert.AreEqual(5, result);
        }

        [TestMethod]
        public void TestMethod2()
        {
            string sentence = "   fly me   to   the moon  ";

            object result = new Solution().LengthOfLastWord(sentence);

            Assert.AreEqual(4, result);
        }

        [TestMethod]
        public void TestMethod3()
        {
            string sentence = "luffy is still joyboy";

            object result = new Solution().LengthOfLastWord(sentence);

            Assert.AreEqual(6, result);
        }

        public class Solution
        {
            public int LengthOfLastWord(string s)
            {
                string[] splits = s.Split(' ').Where(w => !string.IsNullOrWhiteSpace(w)).ToArray();

                return splits[splits.Length - 1].Length;
            }
        }
    }
}