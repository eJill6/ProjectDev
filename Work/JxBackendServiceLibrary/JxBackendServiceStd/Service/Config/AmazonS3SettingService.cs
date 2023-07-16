using Amazon;
using JxBackendService.Interface.Service.Config;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Param.OSS;
using JxBackendService.Model.ViewModel;
using JxBackendService.Service.Base;

namespace JxBackendService.Service.Config
{
    public class AmazonS3SettingService : BaseService, IOSSSettingService
    {
        private static readonly string s_bucketNamePrefix = "youqu";

        private static readonly string s_coreBucketNameInfix = "core";

        private static readonly string s_awsAccessKeyId = "AKIATEFZOYA6N5EAUGHI";

        private static readonly string s_awsSecretAccessKey = "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";

        public AmazonS3SettingService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
        }

        public IOSSSetting GetCoreOSSClientSetting()
        {
            return new AmazonS3Setting()
            {
                AccessKeyId = s_awsAccessKeyId,
                AccessKeySecret = s_awsSecretAccessKey,
                BucketName = $"{s_bucketNamePrefix}.{s_coreBucketNameInfix}.{EnvLoginUser.EnvironmentCode.Value}".ToLower(),
                Endpoint = nameof(RegionEndpoint.APNortheast1),
                RegionEndpoint = RegionEndpoint.APNortheast1,
            };
        }

        public IOSSSetting GetOSSClientSetting()
        {
            return new AmazonS3Setting()
            {
                AccessKeyId = s_awsAccessKeyId,
                AccessKeySecret = s_awsSecretAccessKey,
                BucketName = $"{s_bucketNamePrefix}.{Merchant.Value}.{EnvLoginUser.EnvironmentCode.Value}".ToLower(),
                Endpoint = nameof(RegionEndpoint.APNortheast1),
                RegionEndpoint = RegionEndpoint.APNortheast1,
            };
        }
    }
}