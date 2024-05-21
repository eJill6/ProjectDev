using EncryptImagesTool;

class Program
{
    private static readonly string s_aesExtension = ".aes";
    private static readonly string s_pngExtension = ".png";
    private static readonly string s_jpgExtension = ".jpg";
    private static readonly string s_svgExtension = ".svg";

    private static readonly HashSet<string> s_fileExtension = new HashSet<string>
    {
        s_pngExtension,
        s_jpgExtension,
        s_svgExtension
    };
    static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("Please provide a directory path as argument.");
            return;
        }

        string directoryPath = args[0];

        if (Directory.Exists(directoryPath))
        {
            Folders(directoryPath);
            Console.WriteLine("處理完成請按任意鍵結束");
            Console.ReadKey();
        }
        else if (File.Exists(directoryPath))
        {
            CreateAesImage(directoryPath);
            Console.WriteLine("處理完成請按任意鍵結束");
            Console.ReadKey();
        }
        else
        {
            Console.WriteLine("Directory does or File not exist.");
        }
        
        
    }
    private static void Folders(string directoryPath)
    {
        string[] files = Directory.GetFiles(directoryPath);

        foreach (string filePath in files)
        {
            CreateAesImage(filePath);
        }

        string[] folders = Directory.GetDirectories(directoryPath);

        foreach (var folderPath in folders)
        {
            Folders(folderPath);
        }
    }

    private static void CreateAesImage(string filePath)
    {
        FileInfo fileInfo = new(filePath);
        if (!s_fileExtension.Contains(fileInfo.Extension.ToLower()))
        {
            return;
        }

        Console.WriteLine(filePath);

        byte[] fileContent = File.ReadAllBytes(fileInfo.FullName);
        byte[] encryptContent = AESUtil.Encrypt(fileContent);
        string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileInfo.FullName);
        string newFullFilePath = Path.Combine(fileInfo.DirectoryName, $"{fileNameWithoutExtension}{s_aesExtension}");

        File.WriteAllBytes(newFullFilePath, encryptContent);
    }
}