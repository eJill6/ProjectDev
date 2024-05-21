using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MS.Core.Infrastructure.OSS.Model.Param.OSS;

namespace MS.Core.Infrastructure.OSS.Service.OSS
{
    public class AWSObjectStorageService : IObjectStorageService
    {
        private static readonly int _listObjectsMaxKey = 1000;

        /// <inheritdoc cref="IOptionsMonitor{AmazonS3Setting}"/>
        private readonly IOptionsMonitor<OssSettings> _setting = null;

        private readonly ILogger<AWSObjectStorageService> _logger = null;

        public AWSObjectStorageService(IOptionsMonitor<OssSettings> setting,
            ILogger<AWSObjectStorageService> logger)
        {
            _setting = setting;
            _logger = logger;
        }

        public async Task<bool> UploadObject(byte[] fileByteData, string fullFileName)
        {
            try
            {
                var requestContent = new MemoryStream(fileByteData);

                var putObjectRequest = new PutObjectRequest
                {
                    Key = fullFileName,
                    BucketName = _setting.CurrentValue.BucketName,
                    InputStream = requestContent,
                };

                using (var client = await GetClient())
                {
                    var resp = await client.PutObjectAsync(putObjectRequest);
                    if (resp.HttpStatusCode == System.Net.HttpStatusCode.OK)
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                var method = System.Reflection.MethodBase.GetCurrentMethod();
                _logger.LogError(ex, $"{method?.Name} fails");
            }
            return false;
        }

        public async Task UploadObjectByFilePath(string fullFileName, string fileToUpload)
        {
            try
            {
                var putObjectRequest = new PutObjectRequest
                {
                    BucketName = _setting.CurrentValue.BucketName,
                    Key = fullFileName,
                    FilePath = fileToUpload,
                };

                using (var client = await GetClient())
                {
                    await client.PutObjectAsync(putObjectRequest);
                }
            }
            catch (Exception ex)
            {
                var method = System.Reflection.MethodBase.GetCurrentMethod();
                _logger.LogError(ex, $"{method?.Name} fails");
            }
        }

        public async Task<bool> DeleteObject(string fullFileName)
        {
            try
            {
                using (var client = await GetClient())
                {
                    var deleteObjectRequest = new DeleteObjectRequest
                    {
                        BucketName = _setting.CurrentValue.BucketName,
                        Key = fullFileName,
                    };
                    var resp = await client.DeleteObjectAsync(deleteObjectRequest);
                    _logger.LogInformation("DeletObject success, key:{key}, result:{resp}", fullFileName, resp.HttpStatusCode);
                    return true;
                }
            }
            catch (Exception ex)
            {
                var method = System.Reflection.MethodBase.GetCurrentMethod();
                _logger.LogError(ex, $"{method?.Name} fails");
            }

            return false;
        }

        public async Task<bool> DeleteObjects(List<string> fullFileNames)
        {
            try
            {
                var deleteObjectsRequest = new DeleteObjectsRequest()
                {
                    BucketName = _setting.CurrentValue.BucketName,
                    Objects = fullFileNames.Select(s => new KeyVersion() { Key = s }).ToList()
                };


                using (var client = await GetClient())
                {
                    var resp = await client.DeleteObjectsAsync(deleteObjectsRequest);

                    if (resp.HttpStatusCode == System.Net.HttpStatusCode.OK)
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                var method = System.Reflection.MethodBase.GetCurrentMethod();
                _logger.LogError(ex, $"{method?.Name} fails");
            }

            return false;
        }

        public async Task<byte[]> GetObject(string fullFileName)
        {
            var result = new byte[0];
            try
            {
                using (var client = await GetClient())
                {
                    var ossObject = await client.GetObjectAsync(_setting.CurrentValue.BucketName, fullFileName);
                    var memoryStream = new MemoryStream();
                    ossObject.ResponseStream.CopyTo(memoryStream);

                    result = memoryStream.ToArray();
                }

            }
            catch (Exception ex)
            {
                var method = System.Reflection.MethodBase.GetCurrentMethod();
                _logger.LogError(ex, $"{method?.Name} fails");
            }

            return result;
        }

        public async Task<bool> IsObjectExist(string fullFileName)
        {
            var result = false;
            try
            {
                using (var client = await GetClient())
                {
                    try
                    {
                        var response = await client.GetObjectMetadataAsync(_setting.CurrentValue.BucketName, fullFileName);
                        if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
                        {
                            result = true;
                        }
                        else
                        {
                            var method = System.Reflection.MethodBase.GetCurrentMethod();
                            _logger.LogError("{methodName} fileName: not exist", method?.Name, fullFileName);
                        }
                    }
                    catch (Exception ex)
                    {
                        var method = System.Reflection.MethodBase.GetCurrentMethod();
                        _logger.LogError(ex, $"{method?.Name} fails");
                    }
                }

            }
            catch (Exception ex)
            {
                var method = System.Reflection.MethodBase.GetCurrentMethod();
                _logger.LogError(ex, $"{method?.Name} fails");
            }

            return result;
        }

        public async Task<string[]> GetFullFileNames(string filter)
        {
            var result = new string[0];
            try
            {
                using (var client = await GetClient())
                {
                    var listObjectsRequest = new ListObjectsRequest
                    {
                        MaxKeys = _listObjectsMaxKey,
                        Prefix = filter,
                        BucketName = _setting.CurrentValue.BucketName
                    };

                    ListObjectsResponse listObjectsResponse = await client.ListObjectsAsync(listObjectsRequest);

                    result = listObjectsResponse.S3Objects.Select(s => s.Key).ToArray();
                }

            }
            catch (Exception ex)
            {
                var method = System.Reflection.MethodBase.GetCurrentMethod();
                _logger.LogError(ex, $"{method?.Name} fails");
            }

            return result;
        }

        /// <inheritdoc/>
        public async Task ExcutePaginators(Func<string, Task> action)
        {
            using (var client = await GetClient())
            {
                var listObjectsV2Paginator = client.Paginators.ListObjectsV2(new ListObjectsV2Request
                {
                    BucketName = _setting.CurrentValue.BucketName,
                });

                await foreach (var response in listObjectsV2Paginator.Responses)
                {
                    _logger.LogInformation("ExcutePaginators HttpStatusCode:{statusCode}, Number of Keys:{keys}", response.HttpStatusCode, response.KeyCount);
                    foreach (var entry in response.S3Objects)
                    {
                        await action(entry.Key);
                    }
                }
            }
        }

        private async Task<IAmazonS3> GetClient()
        {
            return await Task.FromResult(new AmazonS3Client(_setting.CurrentValue.AccessKeyId,
                _setting.CurrentValue.AccessKeySecret,
                _setting.CurrentValue.RegionEndpoint));
        }

        public async Task<string> GetCdnPath(string path)
        {
            var uri = new Uri(new Uri(_setting.CurrentValue.BucketCdnDomain), path);
            return await Task.FromResult(uri.AbsoluteUri);
        }
    }
}