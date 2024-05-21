using Microsoft.Extensions.Logging;
using MS.Core.Infrastructure.OSS;
using MS.Core.MM.Repos.interfaces;
using MS.Core.MM.Services.interfaces;
using MS.Core.MMModel.Models.Post.Enums;

namespace MS.Core.MM.Services
{
    /// <inheritdoc cref="SourceType.Report"/>
    public class ReportImageService : PostImageService, IMediaService
    {
        /// <summary>
        /// 上傳路徑
        /// </summary>
        private static readonly string _uploadOssPath = "/Upload/Report/";

        /// <inheritdoc/>
        public override SourceType SourceType => SourceType.Report;

        protected override string UploadOssPath => _uploadOssPath;

        /// <inheritdoc/>
        public ReportImageService(IObjectStorageService oos,
            IMediaRepo repo,
            ILogger<ReportImageService> logger) : base(oos, repo, logger)
        {
        }
    }
}
