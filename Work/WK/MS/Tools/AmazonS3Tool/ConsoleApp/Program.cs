using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using ClassLibrary;
using ConsoleApp.Models;
using System.Text.Json;

namespace ConsoleApp
{
    public class Program
    {
        private static IAmazonS3 _ossClient;

        private static IAmazonS3Setting _ossSetting;

        private static Environments environment = Environments.DEV;

        public static async Task Main(string[] args)
        {
            Console.WriteLine("開始執行");

            while (true)
            {
                Console.WriteLine("請輸入環境: DEV/SIT/UAT/LIVE");
                bool isFail = false;
                var environmentInput = Console.ReadLine();
                switch (environmentInput)
                {
                    case "DEV":
                        environment = Environments.DEV;
                        _ossSetting = new AmazonS3Setting()
                        {
                            AccessKeyId = "AKIATEFZOYA6N5EAUGHI",
                            AccessKeySecret = "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",
                            BucketName = "youqu.msl.development",
                            Endpoint = "APNortheast1",
                            RegionEndpoint = RegionEndpoint.APNortheast1
                        };
                        break;

                    case "SIT":
                        environment = Environments.SIT;
                        _ossSetting = new AmazonS3Setting()
                        {
                            AccessKeyId = "AKIATEFZOYA6N5EAUGHI",
                            AccessKeySecret = "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",
                            BucketName = "youqu.msl.sit",
                            Endpoint = "APNortheast1",
                            RegionEndpoint = RegionEndpoint.APNortheast1
                        };
                        break;

                    case "UAT":
                        environment = Environments.UAT;
                        _ossSetting = new AmazonS3Setting()
                        {
                            AccessKeyId = "AKIATEFZOYA6N5EAUGHI",
                            AccessKeySecret = "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",
                            BucketName = "youqu.msl.uat",
                            Endpoint = "APNortheast1",
                            RegionEndpoint = RegionEndpoint.APNortheast1
                        };
                        break;

                    case "LIVE":
                        environment = Environments.LIVE;
                        _ossSetting = new AmazonS3Setting()
                        {
                            AccessKeyId = "AKIATEFZOYA6N5EAUGHI",
                            AccessKeySecret = "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",
                            BucketName = "youqu.msl.development",
                            Endpoint = "APNortheast1",
                            RegionEndpoint = RegionEndpoint.APNortheast1
                        };
                        isFail = true;
                        Console.WriteLine("不可選擇LIVE");
                        break;

                    default:
                        Console.WriteLine("輸入錯誤");
                        isFail = true;
                        break;
                }
                if (isFail)
                {
                    continue;
                }
                break;
            }

            #region 前置

            _ossClient = new AmazonS3Client(_ossSetting.AccessKeyId, _ossSetting.AccessKeySecret, _ossSetting.RegionEndpoint);

            string path = Environment.CurrentDirectory;
            // 獲取文件的完整路徑
            string directory = Path.Combine(path, "Resources", environment.ToString());

            Console.WriteLine("文件的路徑是：" + directory);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            string jsonPath = Path.Combine(directory, "ImageUrls.json");
            Console.WriteLine("json的路徑是：" + jsonPath);
            if (!File.Exists(jsonPath))
            {
                File.Create(jsonPath);
            }

            var jsonString = await File.ReadAllTextAsync(jsonPath);
            List<string> imageUrls = JsonSerializer.Deserialize<List<string>>(jsonString)!;

            if (!imageUrls.Any())
            {
                Console.WriteLine($"{jsonPath}沒有內容");
            }
            // 使用LINQ選擇並轉換URL
            List<string> aesUrls = imageUrls.Select(url =>
            {
                int lastDotIndex = url.LastIndexOf('.');
                if (lastDotIndex != -1)
                {
                    return url.Substring(0, lastDotIndex) + ".aes";
                }
                throw new Exception("URL格式不正確");
            }).ToList();

            var keyUrls = imageUrls.Concat(aesUrls).ToList();
            keyUrls = keyUrls.Select(url => url.StartsWith("/") ? url.Substring(1) : url).ToList();

            #endregion 前置

            while (true)
            {
                Actions action = Actions.Default;
                while (true)
                {
                    var isFail = false;
                    Console.WriteLine("請輸入動作: 備份1 刪除2 復原3");
                    var input = Console.ReadLine();
                    switch (input)
                    {
                        case "1":
                            action = Actions.Backup;
                            break;

                        case "2":
                            action = Actions.Delete;
                            break;

                        case "3":
                            action = Actions.Restore;
                            break;

                        default:
                            Console.WriteLine("輸入錯誤");
                            isFail = true;
                            break;
                    }
                    if (isFail)
                    {
                        continue;
                    }
                    break;
                }

                switch (action)
                {
                    case Actions.Backup:
                        foreach (var fullFileName in keyUrls)
                        {
                            var FileName = fullFileName.Split('/').Last();
                            var filePath = Path.Combine(directory, FileName);
                            GetObjectResponse ossObject = null;
                            try
                            {
                                ossObject = await _ossClient.GetObjectAsync(_ossSetting.BucketName, fullFileName);
                                if (ossObject.HttpStatusCode != System.Net.HttpStatusCode.OK)
                                {
                                    Console.WriteLine($"請求失敗: {fullFileName},HttpStatusCode: {ossObject.HttpStatusCode}");
                                    continue;
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"請求失敗: {fullFileName},ex.Message: {ex.Message}");
                                continue;
                            }
                            var memoryStream = new MemoryStream();
                            ossObject.ResponseStream.CopyTo(memoryStream);
                            // 將 MemoryStream 指標重置到開頭
                            memoryStream.Position = 0;

                            // 使用 FileStream 來保存檔案
                            using (FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                            {
                                memoryStream.CopyTo(fileStream);
                            }

                            Console.WriteLine($"檔案已保存到: {filePath}");
                        }
                        break;

                    case Actions.Delete:
                        foreach (var fullFileName in keyUrls)
                        {
                            try
                            {
                                DeleteObjectResponse ossObject = await _ossClient.DeleteObjectAsync(_ossSetting.BucketName, fullFileName);
                                if (ossObject.HttpStatusCode != System.Net.HttpStatusCode.NoContent)
                                {
                                    Console.WriteLine($"請求失敗: {fullFileName},HttpStatusCode: {ossObject.HttpStatusCode}");
                                }
                                else
                                {
                                    Console.WriteLine($"檔案已刪除: {fullFileName}");
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"請求失敗: {fullFileName},ex.Message: {ex.Message}");
                            }
                        }
                        break;

                    case Actions.Restore:
                        foreach (var fullFileName in keyUrls)
                        {
                            var FileName = fullFileName.Split('/').Last();
                            var filePath = Path.Combine(directory, FileName);
                            if (!File.Exists(filePath))
                            {
                                Console.WriteLine($"檔案不存在: {fullFileName}");
                                continue;
                            }
                            // 使用FileStream打開文件
                            using FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                            // 創建一個MemoryStream對象
                            using MemoryStream memoryStream = new MemoryStream();
                            // 將FileStream的內容複製到MemoryStream
                            fileStream.CopyTo(memoryStream);

                            var putObjectRequest = new PutObjectRequest
                            {
                                Key = fullFileName,
                                BucketName = _ossSetting.BucketName,
                                InputStream = memoryStream,
                            };
                            try
                            {
                                PutObjectResponse ossObject = await _ossClient.PutObjectAsync(putObjectRequest);
                                if (ossObject.HttpStatusCode != System.Net.HttpStatusCode.OK)
                                {
                                    Console.WriteLine($"請求失敗: {fullFileName},HttpStatusCode: {ossObject.HttpStatusCode}");
                                    continue;
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"請求失敗: {fullFileName},ex.Message: {ex.Message}");
                                continue;
                            }
                            Console.WriteLine($"檔案已恢復: {fullFileName}");
                        }
                        break;

                    default:
                        break;
                }

                Console.WriteLine("執行完畢");
            }
        }
    }
}