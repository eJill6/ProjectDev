namespace ShuffleString
{
    [TestClass]
    public class TestCase
    {
        [TestMethod]
        public void Case1()
        {
            string s = "techcheer";
            int[] indices = new int[] { 5, 3, 0, 1, 7, 8, 6, 2, 4 };

            string answer = Solution.RestoreString(s, indices);

            Assert.AreEqual("cheertech", answer);
        }

        [TestMethod]
        public void Case2()
        {
            string s = "abc";
            int[] indices = new int[] { 0, 1, 2 };

            string answer = Solution.RestoreString(s, indices);

            Assert.AreEqual("abc", answer);
        }

        [TestMethod]
        public void TestCaseFromFile()
        {
            for (int i = 0; i < GenerateTestCase.CaseCount; i++)
            {
                string[] inputLines = File.ReadAllLines(GenerateTestCase.InputFilePath(i));

                string s = inputLines[0];
                int[] indices = inputLines[1].Split(' ').Select(int.Parse).ToArray();

                string answer = Solution.RestoreString(s, indices);

                string expectedAnswer = File.ReadAllText(GenerateTestCase.OutputFilePath(i));

                Assert.AreEqual(expectedAnswer, answer);
            }
        }
    }
}