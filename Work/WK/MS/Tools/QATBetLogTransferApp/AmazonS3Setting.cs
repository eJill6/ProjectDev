using Amazon;

namespace QATBetLogTransferApp
{
    public class AmazonS3Setting
    {
        public string AccessKeyId { get; set; }

        public string AccessKeySecret { get; set; }

        public string Endpoint { get; set; }

        public string BucketName { get; set; }

        public RegionEndpoint RegionEndpoint { get; set; }
    }

    public class UploadSetting
    {        

        public string EnvironmentCode { get; set; }

        public string FindWhat { get; set; }

        public string ReplaceWith { get; set; }
    }
}