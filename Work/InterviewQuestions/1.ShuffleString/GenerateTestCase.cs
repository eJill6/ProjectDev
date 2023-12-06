namespace ShuffleString
{
    [TestClass]
    public class GenerateTestCase
    {
        private static readonly int _caseCount = 10;
        public static int CaseCount => _caseCount;

        private static readonly string _dirPath = "C:\\LiveCodingTestCases\\ShuffleString";
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
            string alphabet = "abcdefghijklmnopqrstuvwxyz";
            Directory.CreateDirectory(_dirPath);

            for (int i = 0; i < _caseCount; i++)
            {
                int numberOfLetters = random.Next(30, 100);

                string randomLetters = new string(Enumerable.Range(0, numberOfLetters)
                    .Select(_ => alphabet[random.Next(alphabet.Length)])
                    .ToArray());

                string randomIntegers = string.Join(" ", Enumerable.Range(0, numberOfLetters)
                    .OrderBy(_ => random.Next())
                    .ToArray());

                string filePath = InputFilePath(i);
                File.WriteAllText(filePath, randomLetters + "\n" + randomIntegers);
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
                    string s = lines[0];
                    int[] indices = lines[1].Split(' ').Select(int.Parse).ToArray();

                    string answer = Solution.RestoreString(s, indices);

                    File.WriteAllText(outputFile, answer);
                }
                else
                {
                    throw new Exception("Input invalid!");
                }
            }
        }
    }
}