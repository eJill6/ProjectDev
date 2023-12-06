namespace FindCommonCharacters
{
    [TestClass]
    public class GenerateTestCase
    {
        private static readonly int _caseCount = 10;
        public static int CaseCount => _caseCount;

        private static readonly string _dirPath = "C:\\LiveCodingTestCases\\FindCommonCharacters";
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
            Directory.CreateDirectory(_dirPath);

            for (int i = 0; i < _caseCount; i++)
            {
                int wordCount = 100;
                int wordLength = 100;
                string alphabet = "abcdefghijklm";
                Random random = new Random();

                string[] words = new string[wordCount];

                for (int j = 0; j < wordCount; j++)
                {
                    char[] wordChars = new char[wordLength];

                    for (int k = 0; k < wordLength; k++)
                    {
                        wordChars[k] = alphabet[random.Next(alphabet.Length)];
                    }

                    words[j] = new string(wordChars);
                }

                string filePath = InputFilePath(i);
                File.WriteAllText(filePath, string.Join(" ", words));
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

                if (lines.Length == 1)
                {
                    string[] s = lines[0].Split(' ').ToArray();

                    List<string> answer = Solution.CommonChars(s).ToList();

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