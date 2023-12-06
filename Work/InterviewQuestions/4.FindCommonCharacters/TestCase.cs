namespace FindCommonCharacters
{
    [TestClass]
    public class TestCase
    {
        [TestMethod]
        public void Case1()
        {
            string[] words = new string[] { "bella", "label", "roller" };
            List<string> answer = Solution.CommonChars(words).ToList();
            List<string> expectedAnswer = new List<string>() { "e", "l", "l" };

            AssertWithSort(expectedAnswer, answer);
        }

        [TestMethod]
        public void Case2()
        {
            string[] words = new string[] { "cool", "lock", "cook" };
            List<string> answer = Solution.CommonChars(words).ToList();
            List<string> expectedAnswer = new List<string>() { "c", "o" };

            AssertWithSort(expectedAnswer, answer);
        }

        [TestMethod]
        public void TestCaseFromFile()
        {
            for (int i = 0; i < GenerateTestCase.CaseCount; i++)
            {
                string[] inputLines = File.ReadAllLines(GenerateTestCase.InputFilePath(i));

                string[] s = inputLines[0].Split(' ').ToArray();

                List<string> answer = Solution.CommonChars(s).ToList();

                string fileText = File.ReadAllText(GenerateTestCase.OutputFilePath(i));
                List<string> expectedAnswer = fileText.Split(' ').ToList();

                AssertWithSort(expectedAnswer, answer);
            }
        }

        private void AssertWithSort(List<string> expectedAnswer, List<string> actualAnswer)
        {
            Assert.AreEqual(Sort(expectedAnswer), Sort(actualAnswer));
        }

        private string Sort(List<string> strings)
        {
            char[] chars = string.Join("", strings).ToCharArray();
            Array.Sort(chars);
            return new string(chars);
        }
    }
}