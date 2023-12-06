using ShuffleString;

namespace ShuffleTheArray
{
    [TestClass]
    public class TestCase
    {
        [TestMethod]
        public void Case1()
        {
            int[] nums = new int[] { 2, 5, 1, 3, 4, 7 };
            int n = 3;
            int[] expectedAnswer = new int[] { 2, 3, 5, 4, 1, 7 };

            AssertShuffleAnswerCorrect(nums, n, expectedAnswer);
        }

        [TestMethod]
        public void Case2()
        {
            int[] nums = new int[] { 1, 2, 3, 4, 4, 3, 2, 1 };
            int n = 4;
            int[] expectedAnswer = new int[] { 1, 4, 2, 3, 3, 2, 4, 1 };

            AssertShuffleAnswerCorrect(nums, n, expectedAnswer);
        }

        [TestMethod]
        public void Case3()
        {
            int[] nums = new int[] { 1, 1, 2, 2 };
            int n = 2;
            int[] expectedAnswer = new int[] { 1, 2, 1, 2 };

            AssertShuffleAnswerCorrect(nums, n, expectedAnswer);
        }

        [TestMethod]
        public void TestCaseFromFile()
        {
            for (int i = 0; i < GenerateTestCase.CaseCount; i++)
            {
                string[] inputLines = File.ReadAllLines(GenerateTestCase.InputFilePath(i));

                int[] nums = inputLines[0].Split(' ').Select(int.Parse).ToArray();
                int n = int.Parse(inputLines[1]);

                int[] answer = Solution.Shuffle(nums, n);

                string expectedAnswer = File.ReadAllText(GenerateTestCase.OutputFilePath(i));

                Assert.AreEqual(expectedAnswer, string.Join(" ", answer));
            }
        }

        private void AssertShuffleAnswerCorrect(int[] nums, int n, int[] expectedAnswer)
        {
            int[] actualAnswer = Solution.Shuffle(nums, n);
            Assert.AreEqual(ConvertToString(expectedAnswer), ConvertToString(actualAnswer));
        }

        private string ConvertToString(int[] intArray)
        {
            return string.Join(" ", intArray);
        }
    }
}