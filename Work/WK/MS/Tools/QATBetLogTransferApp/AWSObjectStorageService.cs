using Amazon.S3;
using Amazon.S3.Model;

namespace QATBetLogTransferApp
{
    public class AWSObjectStorageService
    {
        private static readonly int s_listObjectsMaxKey = 1000;

        private readonly AmazonS3Setting _amazonS3Setting;

        private readonly AmazonS3Client _amazonS3Client;

        public AWSObjectStorageService(AmazonS3Setting amazonS3Setting)
        {
            _amazonS3Setting = amazonS3Setting;
            _amazonS3Client = new AmazonS3Client(amazonS3Setting.AccessKeyId, amazonS3Setting.AccessKeySecret, _amazonS3Setting.RegionEndpoint);
        }

        public void UploadObject(byte[] fileByteData, string fullFileName)
        {
            var requestContent = new MemoryStream(fileByteData);

            var putObjectRequest = new PutObjectRequest
            {
                Key = fullFileName,
                BucketName = _amazonS3Setting.BucketName,
                InputStream = requestContent,
            };

            _amazonS3Client.PutObjectAsync(putObjectRequest).Wait();
        }

        public void UploadObjectByFilePath(string fullFileName, string fileToUpload)
        {
            var putObjectRequest = new PutObjectRequest
            {
                BucketName = _amazonS3Setting.BucketName,
                Key = fullFileName,
                FilePath = fileToUpload,
            };

            _amazonS3Client.PutObjectAsync(putObjectRequest).Wait();
        }

        //public void DeleteObject(string fullFileName)
        //{
        //    _amazonS3Client.DeleteObjectAsync(_amazonS3Setting.BucketName, fullFileName).Wait();
        //}

        public void DeleteObjects(List<string> fullFileNames)
        {
            var deleteObjectsRequest = new DeleteObjectsRequest()
            {
                BucketName = _amazonS3Setting.BucketName,
                Objects = fullFileNames.Select(s => new KeyVersion() { Key = s }).ToList()
            };

            _amazonS3Client.DeleteObjectsAsync(deleteObjectsRequest).Wait();
        }

        //public byte[] GetObject(string fullFileName)
        //{
        //    GetObjectResponse ossObject = _amazonS3.GetObjectAsync(_amazonS3Setting.BucketName, fullFileName).Result;
        //    var memoryStream = new MemoryStream();
        //    ossObject.ResponseStream.CopyTo(memoryStream);

        //    return memoryStream.ToArray();
        //}

        public void WriteResponseStreamToFile(string fullFileName, string writeToPath)
        {
            GetObjectResponse objectResponse = _amazonS3Client.GetObjectAsync(_amazonS3Setting.BucketName, fullFileName).Result;
            objectResponse.WriteResponseStreamToFileAsync(writeToPath, append: false, default).Wait();
        }

        public List<string> GetFullFileNames(string filter)
        {
            var listObjectsRequest = new ListObjectsRequest
            {
                MaxKeys = s_listObjectsMaxKey,
                Prefix = filter,
                BucketName = _amazonS3Setting.BucketName
            };

            ListObjectsResponse listObjectsResponse = _amazonS3Client.ListObjectsAsync(listObjectsRequest).Result;

            return listObjectsResponse.S3Objects.Select(s => s.Key).ToList();
        }
    }
}