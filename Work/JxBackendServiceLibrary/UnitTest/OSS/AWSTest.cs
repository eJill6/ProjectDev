using Amazon.S3;
using Amazon.S3.Model;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.Config;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Param.OSS;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTest.Base;
using System.Collections.Generic;
using System.IO;
using System;

namespace UnitTest.OSS
{
    [TestClass]
    public class AWSTest : BaseTest
    {
        private readonly IOSSSettingService _ossSettingService;
        private readonly IAmazonS3 _ossClient;

        private static readonly string s_testBucketName = "youqu.msl.development";

        public static readonly List<string> s_awsBucketNames = new List<string>
        {
            "youqu.msl.development",
            "youqu.msl.sit",
            "youqu.msl.uat",
            "youqu.msl.production"
        };

        public AWSTest()
        {
            _ossSettingService = DependencyUtil.ResolveJxBackendService<IOSSSettingService>(EnvLoginUser, DbConnectionTypes.Slave);
            var amazonS3Setting = _ossSettingService.GetOSSClientSetting() as IAmazonS3Setting;

            _ossClient = new AmazonS3Client(amazonS3Setting.AccessKeyId, amazonS3Setting.AccessKeySecret, amazonS3Setting.RegionEndpoint);
        }

        [TestMethod]
        public void CreateBucketObject()
        {
            foreach (string bucketName in s_awsBucketNames)
            {
                var putBucketRequest = new PutBucketRequest
                {
                    BucketName = bucketName,
                };

                PutBucketResponse putBucketResponse = _ossClient.PutBucket(putBucketRequest);

                string policyText = @"{
                    ""Version"": ""2012-10-17"",
                    ""Statement"": [
                        {
                            ""Sid"": ""PublicReadGetObject"",
                            ""Effect"": ""Allow"",
                            ""Principal"": ""*"",
                            ""Action"": ""s3:GetObject"",
                            ""Resource"": ""arn:aws:s3:::" + bucketName + @"/*""
                        }
                    ]
                }";

                _ossClient.PutBucketPolicy(new PutBucketPolicyRequest
                {
                    BucketName = bucketName,
                    Policy = policyText
                });
            }
        }

        [TestMethod]
        public void ListObjects()
        {
            foreach (string bucketName in s_awsBucketNames)
            {
                ListObjectsResponse result = _ossClient.ListObjects(bucketName);
            }
        }

        [TestMethod]
        public void ByteUploadObject()
        {
            try
            {
                foreach (string bucketName in s_awsBucketNames)
                {
                    string[] imageFiles = Directory.GetFiles(@"C:\Users\hugo.eng\Desktop\hot", "*.jpg");

                    foreach (string imageFile in imageFiles)
                    {
                        byte[] imageData = File.ReadAllBytes(imageFile);

                        string[] parts = imageFile.Split('\\');
                        string fileName = parts[parts.Length - 1];

                        MemoryStream requestContent = new MemoryStream(imageData);

                        var request = new PutObjectRequest
                        {
                            BucketName = bucketName,
                            Key = $"BackSideWeb/Upload/HotGames/{fileName}",
                            InputStream = requestContent
                        };

                        var repsonse = _ossClient.PutObject(request);

                        Console.WriteLine($"Put image {fileName} object succeeded");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Put object failed, {0}", ex.Message);
                throw ex;
            }
        }

        [TestMethod]
        public void DeleteObject()
        {
            string fileName = $"aa/bb/test123.png";

            DeleteObjectResponse result = _ossClient.DeleteObject(s_testBucketName, fileName);
        }
    }
}
