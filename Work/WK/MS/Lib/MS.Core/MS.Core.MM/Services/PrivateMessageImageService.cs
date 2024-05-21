using Microsoft.Extensions.Logging;
using MS.Core.Infrastructure.OSS;
using MS.Core.MM.Repos.interfaces;
using MS.Core.MM.Services.interfaces;
using MS.Core.MMModel.Models.Post.Enums;

namespace MS.Core.MM.Services
{
    /// <inheritdoc cref="SourceType.Refund"/>
    public class PrivateMessageImageService : PostImageService, IMediaService
    {
        /// <summary>
        /// 上傳路徑
        /// </summary>
        private static readonly string _uploadOssPath = "/Upload/PrivateMessage/";

        /// <inheritdoc/>
        public override SourceType SourceType => SourceType.PrivateMessage;

        protected override string UploadOssPath => _uploadOssPath;

        /// <inheritdoc/>
        public PrivateMessageImageService(IObjectStorageService oos,
            IMediaRepo repo,
            ILogger<PrivateMessageImageService> logger) : base(oos, repo, logger)
        {
        }
    }
}