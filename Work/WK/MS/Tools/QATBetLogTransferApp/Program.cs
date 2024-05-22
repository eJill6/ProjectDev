using Autofac;
using Microsoft.Extensions.Configuration;
using QATBetLogTransferApp;
using System.Text.Json;

const int executionSeconds = 60;

Console.WriteLine("Start Initialize");
CancellationToken cancellationToken = InitializeCancelEvent();
BuildAutofacContainer();

int totalWaitSeconds = int.MaxValue;

while (!cancellationToken.IsCancellationRequested)
{
    if (totalWaitSeconds < executionSeconds)
    {
        Thread.Sleep(1000);
        totalWaitSeconds++;

        continue;
    }

    totalWaitSeconds = 0;
    Console.WriteLine("Start DownloadProductQATBetLogs");
    List<string> tempFullFileNames =  DownloadProductQATBetLogs();

    if (!tempFullFileNames.Any())
    {
        Console.WriteLine("No files are downloaded ");
        Thread.Sleep(1000);
        totalWaitSeconds++;

        continue;
    }

    List<UploadSetting> uploadSettings = GetUploadSettings();

    foreach (UploadSetting uploadSetting in uploadSettings)
    {
        Console.WriteLine($"setting={JsonSerializer.Serialize(uploadSetting)}");
        Console.WriteLine("Start CopyAndReplaceBetLogContents");
        List<string> fullFilePaths =  CopyAndReplaceBetLogContents(uploadSetting, tempFullFileNames);

        Console.WriteLine("Start UploadMerchantBetLogs");
        UploadMerchantBetLogs(uploadSetting.EnvironmentCode, fullFilePaths);
    }

    Console.WriteLine("Start BackupLocalBetLogs");
    BackupLocalBetLogs();

    Console.WriteLine("Jobs are done");
}

Environment.Exit(0);

static string S3RootPath() => "UploadBetLogService/Upload";

static string TempRootPath() => "Temp";

static string MerchatQAT() => "QAT";

static CancellationToken InitializeCancelEvent()
{
    var cancellationTokenSource = new CancellationTokenSource();
    CancellationToken cancellationToken = cancellationTokenSource.Token;

    Console.CancelKeyPress += (object? sender, ConsoleCancelEventArgs eventArgs) =>
    {
        cancellationTokenSource.Cancel();
        eventArgs.Cancel = true;
    };

    return cancellationToken;
}

static void BuildAutofacContainer()
{
    var containerBuilder = new ContainerBuilder();
    containerBuilder.RegisterInstance(CreateConfiguration()).As<IConfiguration>();
    IContainer container = containerBuilder.Build();
    AutofacUtil.SetContainer(container);
}

static List<string> DownloadProductQATBetLogs()
{
    var amazonS3SettingService = new AmazonS3SettingService();
    AmazonS3Setting productAmazonS3Setting = amazonS3SettingService.GetProductCoreS3Setting();
    var awsObjectStorageService = new AWSObjectStorageService(productAmazonS3Setting);
    List<string> fullFileNames = awsObjectStorageService.GetFullFileNames($"{S3RootPath()}/{MerchatQAT()}");
    string merchant = GetUploadMerchant();

    var tempFullFileNames = new List<string>();

    for (int i = 0; i < fullFileNames.Count; i++)
    {
        string fullFileName = fullFileNames[i];
        //下載到暫存區
        string writeToPath = Path.Combine(Environment.CurrentDirectory, TempRootPath(), fullFileName.Replace($"/{MerchatQAT()}/", $"/{merchant}/"));
        string directoryName = Path.GetDirectoryName(writeToPath);

        CreateDirectoryNX(directoryName);

        Console.WriteLine($"Donwload {i + 1}/{fullFileNames.Count} {fullFileName}");
        awsObjectStorageService.WriteResponseStreamToFile(fullFileName, writeToPath);
        tempFullFileNames.Add(writeToPath);
    }

    if (fullFileNames.Any())
    {
        Console.WriteLine("Delete S3 Objects");
        awsObjectStorageService.DeleteObjects(fullFileNames);
    }

    return tempFullFileNames;
}

static List<string> CopyAndReplaceBetLogContents(UploadSetting uploadSetting, List<string> tempFullFileNames)
{
    List<string> fullFilePaths = CopyTempFileToWorkDirectory(tempFullFileNames);

    foreach (string fullFilePath in fullFilePaths)
    {
        string fileContent = File.ReadAllText(fullFilePath);
        string replacedContent = fileContent.Replace(uploadSetting.FindWhat, uploadSetting.ReplaceWith);
        File.WriteAllText(fullFilePath, replacedContent);
    }

    return fullFilePaths;
}

static List<string> CopyTempFileToWorkDirectory(List<string> tempFullFileNames)
{
    string readPath = Path.Combine(Environment.CurrentDirectory, S3RootPath());

    if (Directory.Exists(readPath))
    {
        Directory.Delete(readPath, recursive: true);
    }

    CreateDirectoryNX(readPath);
    var destFileNames = new List<string>();

    long newTimestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

    for (int i = 0; i < tempFullFileNames.Count; i++)
    {
        string tempFullFilePath = tempFullFileNames[i];
        FileInfo destFileInfo = new FileInfo(tempFullFilePath.Replace($"{TempRootPath()}{Path.DirectorySeparatorChar}", string.Empty));
        string destFileName = destFileInfo.FullName.Replace(destFileInfo.Name, $"{newTimestamp + i}{destFileInfo.Extension}");

        CreateDirectoryNX(destFileInfo.DirectoryName);
        File.Copy(tempFullFilePath, destFileName);
        destFileNames.Add(destFileName);
    }

    return destFileNames;
}

static void UploadMerchantBetLogs(string environmentCode, List<string> filePathsToUpload)
{
    var amazonS3SettingService = new AmazonS3SettingService();
    AmazonS3Setting merchantAmazonS3Setting = amazonS3SettingService.GetUploadCoreS3Setting(environmentCode);
    var awsObjectStorageService = new AWSObjectStorageService(merchantAmazonS3Setting);
    string s3RootPath = S3RootPath();

    for (int i = 0; i < filePathsToUpload.Count; i++)
    {
        string filePathToUpload = filePathsToUpload[i];
        int index = filePathToUpload.IndexOf(s3RootPath.Replace("/", Path.DirectorySeparatorChar.ToString()));
        string fullFileName = filePathToUpload.Substring(index).Replace("\\", "/");

        Console.WriteLine($"Upload {i + 1}/{filePathsToUpload.Count()} {fullFileName}");
        awsObjectStorageService.UploadObjectByFilePath(fullFileName, filePathToUpload);
    }
}

static void BackupLocalBetLogs()
{
    string readPath = Path.Combine(Environment.CurrentDirectory, TempRootPath(), S3RootPath());

    if (!Directory.Exists(readPath))
    {
        return;
    }

    string backupDiretory = Path.Combine(Environment.CurrentDirectory, "Backup", DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString());

    CreateDirectoryNX(backupDiretory);

    string destDirName = Path.Combine(backupDiretory, "Upload");
    Directory.Move(readPath, destDirName);
    Directory.Delete(Path.Combine(Environment.CurrentDirectory, S3RootPath()), recursive: true);
}

static IConfiguration CreateConfiguration()
{
    return new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: true)
        .Build();
}

static string GetUploadMerchant()
{
    var configuration = AutofacUtil.Container.Resolve<IConfiguration>();
    string uploadMerchant = configuration.GetValue<string>("UploadMerchant");

    return uploadMerchant;
}

static List<UploadSetting> GetUploadSettings()
{
    var configuration = AutofacUtil.Container.Resolve<IConfiguration>();
    var uploadSettings = configuration.GetSection("UploadSettings").Get<List<UploadSetting>>();

    return uploadSettings;
}

static void CreateDirectoryNX(string path)
{
    if (!Directory.Exists(path))
    {
        Directory.CreateDirectory(path);
    }
}

public class AutofacUtil
{
    public static IComponentContext Container { get; private set; }

    public static void SetContainer(IComponentContext container)
    {
        Container = container;
    }
}