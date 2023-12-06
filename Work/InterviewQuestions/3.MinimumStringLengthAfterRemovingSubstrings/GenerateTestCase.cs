namespace MinimumStringLengthAfterRemovingSubstrings
{
    [TestClass]
    public class GenerateTestCase
    {
        private static readonly int _caseCount = 10;
        public static int CaseCount => _caseCount;

        private static readonly string _dirPath = "C:\\LiveCodingTestCases\\MinimumStringLengthAfterRemovingSubstrings";
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
                string generateResult;

                if (i < 3)
                {
                    generateResult = GeneratePureRandomString();
                }
                else if (i < 7)
                {
                    generateResult = GeneratePureABString();
                }
                else
                {
                    generateResult = GeneratePureABCDString();
                }

                string filePath = InputFilePath(i);
                File.WriteAllText(filePath, generateResult);
            }
        }

        private string GeneratePureRandomString()
        {
            Random random = new Random();

            string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            int numberOfLetters = random.Next(30, 60);

            string randomString = new string(Enumerable.Range(0, numberOfLetters)
                .Select(_ => alphabet[random.Next(alphabet.Length)])
                .ToArray());

            int numberOfSubstrings = numberOfLetters / 4;

            for (int i = 0; i < numberOfSubstrings; i++)
            {
                int insertIndex = random.Next(randomString.Length + 1);
                string insertString = random.Next(2) == 0 ? "AB" : "CD";
                randomString = randomString.Insert(insertIndex, "AB");
            }

            return randomString;
        }

        private string GeneratePureABString()
        {
            Random random = new Random();

            string alphabet = "AB";
            int numberOfLetters = random.Next(50, 100);

            string randomString = new string(Enumerable.Range(0, numberOfLetters)
                .Select(_ => alphabet[random.Next(alphabet.Length)])
                .ToArray());

            return randomString;
        }

        private string GeneratePureABCDString()
        {
            Random random = new Random();

            string alphabet = "ABCD";
            int numberOfLetters = random.Next(30, 60);

            string randomString = new string(Enumerable.Range(0, numberOfLetters)
                .Select(_ => alphabet[random.Next(alphabet.Length)])
                .ToArray());

            int numberOfSubstrings = numberOfLetters / 4;

            while (randomString.Length < 99)
            {
                int insertIndex = random.Next(randomString.Length + 1);
                string insertString = random.Next(2) == 0 ? "AB" : "CD";
                randomString = randomString.Insert(insertIndex, "AB");
            }

            return randomString;
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
                    string s = lines[0];

                    int answer = Solution.MinLength(s);

                    File.WriteAllText(outputFile, answer.ToString());
                }
                else
                {
                    throw new Exception("Input invalid!");
                }
            }
        }
    }
}