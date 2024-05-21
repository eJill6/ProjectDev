using Amazon;

namespace MS.Core.Infrastructure.OSS.Model.Param.OSS
{
    public class OssSettings
    {
        public string AccessKeyId { get; set; }

        public string AccessKeySecret { get; set; }

        public string BucketName { get; set; }

        public string SystemName { get; set; }

        public RegionEndpoint RegionEndpoint => RegionEndpoint.GetBySystemName(SystemName);

        public string BucketCdnDomain { get; set; }
    }
}