using Amazon;

namespace JxBackendService.Model.Param.OSS
{
    public interface IOSSSetting
    {
        string AccessKeyId { get; }

        string AccessKeySecret { get; }

        string BucketName { get; }

        string Endpoint { get; }
    }

    public interface IAmazonS3Setting : IOSSSetting
    {
        RegionEndpoint RegionEndpoint { get; }
    }

    public class OSSSetting : IOSSSetting
    {
        public string AccessKeyId { get; set; }

        public string AccessKeySecret { get; set; }

        public string Endpoint { get; set; }

        public string BucketName { get; set; }
    }

    public class AmazonS3Setting : OSSSetting, IAmazonS3Setting
    {
        public RegionEndpoint RegionEndpoint { get; set; }
    }
}