namespace MinimumStringLengthAfterRemovingSubstrings
{
    [TestClass]
    public class TestCase
    {
        [TestMethod]
        public void Case1()
        {
            string s = "ABFCACDB";
            int answer = Solution.MinLength(s);

            Assert.AreEqual(2, answer);
        }

        [TestMethod]
        public void Case2()
        {
            string s = "ACBBD";
            int answer = Solution.MinLength(s);

            Assert.AreEqual(5, answer);
        }

        [TestMethod]
        public void TestCaseFromFile()
        {
            for (int i = 0; i < GenerateTestCase.CaseCount; i++)
            {
                string[] inputLines = File.ReadAllLines(GenerateTestCase.InputFilePath(i));

                string s = inputLines[0];

                int answer = Solution.MinLength(s);

                string expectedAnswer = File.ReadAllText(GenerateTestCase.OutputFilePath(i));

                Assert.AreEqual(expectedAnswer, answer.ToString());
            }
        }
    }
}