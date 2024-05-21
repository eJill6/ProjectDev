using Amazon.S3;
using Amazon.S3.Model;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.Config;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Param.OSS;
using System.Collections.Generic;
using System.IO;
using System;
using Autofac;
using JxBackendService.Interface.Service;

namespace UnitTestN6.AWS
{
    [TestClass]
    public class AWSTest : BaseUnitTest
    {
        private readonly IOSSSettingService _ossSettingService;

        private readonly IAmazonS3 _ossClient;

        private static readonly string s_testBucketName = "youqu.msl.development";

        private static readonly List<string> s_awsBucketNames = new List<string>
        {
            "youqu.msl.development",
            //"youqu.msl.sit",
            //"youqu.msl.uat",
            //"youqu.msl.production"
        };

        private static readonly HashSet<string> s_slotThirdPartyGameCode = new HashSet<string>
        {
            "IMJDB",
            "IMPP",
            "IMPT",
            "IMSE",
            "JDBFI",
            "PMSL",
        };

        protected override void AppendServiceToContainerBuilder(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<AWSTestEnvironmentService>().AsImplementedInterfaces();            
        }

        public AWSTest()
        {
            _ossSettingService = DependencyUtil.ResolveJxBackendService<IOSSSettingService>(EnvironmentUser, DbConnectionTypes.Slave).Value;
            var amazonS3Setting = _ossSettingService.GetOSSClientSetting() as IAmazonS3Setting;

            _ossClient = new AmazonS3Client(amazonS3Setting.AccessKeyId, amazonS3Setting.AccessKeySecret, amazonS3Setting.RegionEndpoint);
        }

        [TestMethod]
        public void ByteUploadObject()
        {
            string[] imageFiles = Directory.GetFiles(@"C:\Users\hugo.eng\Desktop\hot", "*.jpg");

            foreach (string bucketName in s_awsBucketNames)
            {
                UploadObjects(bucketName, imageFiles, "BackSideWeb/Upload/HotGames");
            }
        }

        [TestMethod]
        public void DownloadHotAndSlotGameImages()
        {
            DownloadHotGameImages();
            DownloadSlotGameImages();
        }

        [TestMethod]
        public void UploadAESHotAndSlotGameImages()
        {
            UploadAESHotGameImages();
            UploadAESSlotGameImages();
        }

        [TestMethod]
        public void UploadAESHotGameImages()
        {
            string localFolderPath = "C:\\AllS3Image";

            foreach (string bucketName in s_awsBucketNames)
            {
                string localfullFolderPath = Path.Combine(localFolderPath, bucketName, "HotGames");
                string[] imageFiles = Directory.GetFiles(localfullFolderPath, "*.aes");
                string remoteFolderPath = $"BackSideWeb/Upload/HotGames";

                UploadObjects(bucketName, imageFiles, remoteFolderPath);
            }
        }

        [TestMethod]
        public void UploadAESSlotGameImages()
        {
            string localFolderPath = "C:\\AllS3Image";

            foreach (string bucketName in s_awsBucketNames)
            {
                foreach (string gameCode in s_slotThirdPartyGameCode)
                {
                    string localfullFolderPath = Path.Combine(localFolderPath, bucketName, $"SlotGames\\{gameCode}");
                    string[] imageFiles = Directory.GetFiles(localfullFolderPath, "*.aes");
                    string remoteFolderPath = $"BackSideWeb/Upload/SlotGames/{gameCode}";

                    UploadObjects(bucketName, imageFiles, remoteFolderPath);
                }
            }
        }

        [TestMethod]
        public void DownloadHotGameImages()
        {
            string localFolderPath = "C:\\AllS3Image";

            foreach (string bucketName in s_awsBucketNames)
            {
                string remotefullFolder = $"BackSideWeb/Upload/HotGames";
                string localfullFolderPath = Path.Combine(localFolderPath, bucketName, "HotGames");

                if (!Directory.Exists(localfullFolderPath))
                {
                    Directory.CreateDirectory(localfullFolderPath);
                }

                DownloadRemoteFolderImages(bucketName, remotefullFolder, localfullFolderPath);
            }
        }

        [TestMethod]
        public void DownloadSlotGameImages()
        {
            string localFolderPath = "C:\\AllS3Image";

            foreach (string bucketName in s_awsBucketNames)
            {
                foreach (string gameCode in s_slotThirdPartyGameCode)
                {
                    string remoteFolderPath = $"BackSideWeb/Upload/SlotGames/{gameCode}";

                    string localfullFolderPath = Path.Combine(localFolderPath, bucketName, $"SlotGames\\{gameCode}");

                    if (!Directory.Exists(localfullFolderPath))
                    {
                        Directory.CreateDirectory(localfullFolderPath);
                    }

                    DownloadRemoteFolderImages(bucketName, remoteFolderPath, localfullFolderPath);
                }
            }
        }

        private void DownloadRemoteFolderImages(string bucketName, string remoteFolderPath, string localfullFolderPath)
        {
            var listObjectsRequest = new ListObjectsV2Request
            {
                BucketName = bucketName,
                Prefix = remoteFolderPath
            };

            ListObjectsV2Response objectResponse = _ossClient.ListObjectsV2Async(listObjectsRequest).Result;

            foreach (S3Object s3Object in objectResponse.S3Objects)
            {
                string key = s3Object.Key;
                string localFilePath = Path.Combine(localfullFolderPath, Path.GetFileName(key));

                var request = new GetObjectRequest
                {
                    BucketName = bucketName,
                    Key = key
                };

                using (GetObjectResponse response = _ossClient.GetObjectAsync(request).Result)
                {
                    using (var fileStream = File.Create(localFilePath))
                    {
                        response.ResponseStream.CopyTo(fileStream);
                    }
                }
            }
        }

        private void UploadObjects(string bucketName, string[] imageFiles, string remoteFolderPath)
        {
            try
            {
                foreach (string imageFile in imageFiles)
                {
                    byte[] imageData = File.ReadAllBytes(imageFile);

                    string[] parts = imageFile.Split('\\');
                    string fileName = parts[parts.Length - 1];

                    MemoryStream requestContent = new MemoryStream(imageData);

                    var request = new PutObjectRequest
                    {
                        BucketName = bucketName,
                        Key = $"{remoteFolderPath}/{fileName}",
                        InputStream = requestContent
                    };

                    PutObjectResponse repsonse = _ossClient.PutObjectAsync(request).Result;

                    Console.WriteLine($"Put image {fileName} object succeeded");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Put object failed, {0}", ex.Message);
                throw;
            }
        }
    }

    public class AWSTestEnvironmentService : IEnvironmentService
    {
        public JxApplication Application => JxApplication.BatchService;
    }
}