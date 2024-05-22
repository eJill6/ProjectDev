using Amazon;
using Autofac;
using Microsoft.Extensions.Configuration;

namespace QATBetLogTransferApp
{
    public class AmazonS3SettingService
    {
        private static readonly string s_bucketNamePrefix = "youqu";

        private static readonly string s_coreBucketNameInfix = "core";

        private static readonly string s_awsAccessKeyId = "AKIATEFZOYA6N5EAUGHI";

        private static readonly string s_awsSecretAccessKey = "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";

        private readonly IConfiguration _configuration;

        public AmazonS3SettingService()
        {
            _configuration = AutofacUtil.Container.Resolve<IConfiguration>();
        }

        public AmazonS3Setting GetProductCoreS3Setting()
        {
            return new AmazonS3Setting()
            {
                AccessKeyId = s_awsAccessKeyId,
                AccessKeySecret = s_awsSecretAccessKey,
                BucketName = $"{s_bucketNamePrefix}.{s_coreBucketNameInfix}.production".ToLower(),
                Endpoint = nameof(RegionEndpoint.APNortheast1),
                RegionEndpoint = RegionEndpoint.APNortheast1,
            };
        }

        public AmazonS3Setting GetUploadCoreS3Setting(string environmentCode)
        {
            return new AmazonS3Setting()
            {
                AccessKeyId = s_awsAccessKeyId,
                AccessKeySecret = s_awsSecretAccessKey,
                BucketName = $"{s_bucketNamePrefix}.{s_coreBucketNameInfix}.{environmentCode}".ToLower(),                
                Endpoint = nameof(RegionEndpoint.APNortheast1),
                RegionEndpoint = RegionEndpoint.APNortheast1,
            };
        }
    }
}