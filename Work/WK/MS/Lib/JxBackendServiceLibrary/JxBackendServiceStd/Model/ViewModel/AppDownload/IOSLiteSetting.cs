namespace JxBackendService.Model.ViewModel.AppDownload
{
    public class IOSLiteSetting
    {
        public string[] LiteAppLandingUrls { get; set; }

        public string TestingImagePath { get; set; }

        public string PayloadDisplayName { get; set; }

        public string PayloadIdentifier { get; set; }

        public string DownloadJumpPayloadDisplayName { get; set; }

        public string DownloadJumpPayloadIdentifier { get; set; }

        public string DownloadJumpProvisionUrl { get; set; }

        public bool IsAppendDisplayVersion { get; set; }

        public string AppDownloadWebProxyUrl { get; set; }
    }
}