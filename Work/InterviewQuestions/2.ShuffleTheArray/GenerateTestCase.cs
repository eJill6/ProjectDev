using ShuffleString;

namespace ShuffleTheArray
{
    [TestClass]
    public class GenerateTestCase
    {
        private static readonly int _caseCount = 10;

        public static int CaseCount => _caseCount;

        private static readonly string _dirPath = "C:\\LiveCodingTestCases\\ShuffleTheArray";

        public static string InputFilePath(int index) => $"{_dirPath}\\in_{index}.txt";

        public static string OutputFilePath(int index) => $"{_dirPath}\\out_{index}.txt";

        [TestMethod]
        public void Generate()
        {
            GenerateInput();
            GenerateOutput();
        }

        [TestMethod]
        public void GenerateInput()
        {
            Random random = new Random();
            Directory.CreateDirectory(_dirPath);

            for (int i = 0; i < _caseCount; i++)
            {
                int n = random.Next(10, 500);

                if (i == _caseCount - 1)
                {
                    n = 500;
                }

                int[] randomIntegers = new int[n * 2];

                for (int j = 0; j < n * 2; j++)
                {
                    randomIntegers[j] = random.Next(1, 1000 + 1);
                }

                string filePath = InputFilePath(i);
                File.WriteAllText(filePath, string.Join(" ", randomIntegers) + "\n" + n);
            }
        }

        [TestMethod]
        public void GenerateOutput()
        {
            for (int i = 0; i < _caseCount; i++)
            {
                string inputFile = InputFilePath(i);
                string outputFile = OutputFilePath(i);

                string[] lines = File.ReadAllLines(inputFile);

                if (lines.Length >= 2)
                {
                    int[] nums = lines[0].Split(' ').Select(int.Parse).ToArray();
                    int n = int.Parse(lines[1]);

                    int[] answer = Solution.Shuffle(nums, n);

                    File.WriteAllText(outputFile, string.Join(" ", answer));
                }
                else
                {
                    throw new Exception("Input invalid!");
                }
            }
        }
    }
}