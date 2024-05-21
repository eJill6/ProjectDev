using System.Collections.Generic;
using System.IO;
using System.Linq;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using JxBackendService.Interface.Service.OSS;
using JxBackendService.Model.Param.OSS;

namespace JxBackendService.Service.OSS
{
    public class AWSObjectStorageService : IObjectStorageService
    {
        private static readonly int s_listObjectsMaxKey = 1000;

        private readonly IAmazonS3 _ossClient;

        private readonly IAmazonS3Setting _ossSetting;

        public AWSObjectStorageService(IOSSSetting ossSetting)
        {
            _ossSetting = ossSetting as IAmazonS3Setting;
            _ossClient = new AmazonS3Client(ossSetting.AccessKeyId, ossSetting.AccessKeySecret, _ossSetting.RegionEndpoint);
        }

        public void UploadObject(byte[] fileByteData, string fullFileName)
        {
            var requestContent = new MemoryStream(fileByteData);

            var putObjectRequest = new PutObjectRequest
            {
                Key = fullFileName,
                BucketName = _ossSetting.BucketName,
                InputStream = requestContent,
            };

            _ossClient.PutObject(putObjectRequest);
        }

        public void UploadObjectByFilePath(string fullFileName, string fileToUpload)
        {
            var putObjectRequest = new PutObjectRequest
            {
                BucketName = _ossSetting.BucketName,
                Key = fullFileName,
                FilePath = fileToUpload,
            };

            _ossClient.PutObject(putObjectRequest);
        }

        public void DeleteObject(string fullFileName)
        {
            _ossClient.DeleteObject(_ossSetting.BucketName, fullFileName);
        }

        public void DeleteObjects(List<string> fullFileNames)
        {
            var deleteObjectsRequest = new DeleteObjectsRequest()
            {
                BucketName = _ossSetting.BucketName,
                Objects = fullFileNames.Select(s => new KeyVersion() { Key = s }).ToList()
            };

            _ossClient.DeleteObjects(deleteObjectsRequest);
        }

        public byte[] GetObject(string fullFileName)
        {
            GetObjectResponse ossObject = _ossClient.GetObject(_ossSetting.BucketName, fullFileName);
            var memoryStream = new MemoryStream();
            ossObject.ResponseStream.CopyTo(memoryStream);

            return memoryStream.ToArray();
        }

        public List<string> GetFullFileNames(string filter)
        {
            var listObjectsRequest = new ListObjectsRequest
            {
                MaxKeys = s_listObjectsMaxKey,
                Prefix = filter,
                BucketName = _ossSetting.BucketName
            };

            ListObjectsResponse listObjectsResponse = _ossClient.ListObjects(listObjectsRequest);

            return listObjectsResponse.S3Objects.Select(s => s.Key).ToList();
        }

        //public void CopyObject(string sourceFullFileName, string targetFullFileName)
        //{
        //    var copyObjectRequest = new CopyObjectRequest
        //    {
        //        SourceBucket = _ossSetting.BucketName,
        //        SourceKey = sourceFullFileName,
        //        DestinationBucket = _ossSetting.BucketName,
        //        DestinationKey = targetFullFileName
        //    };

        //    _ossClient.CopyObject(copyObjectRequest);
        //}
    }
}