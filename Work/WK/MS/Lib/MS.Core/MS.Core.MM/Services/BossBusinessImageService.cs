using Microsoft.Extensions.Logging;
using MS.Core.Infrastructure.OSS;
using MS.Core.MM.Repos.interfaces;
using MS.Core.MM.Services.interfaces;
using MS.Core.MMModel.Models.Post.Enums;

namespace MS.Core.MM.Services
{
    /// <inheritdoc cref="SourceType.BusinessPhoto"/>
    public class BossBusinessImageService : PostImageService, IMediaService
    {
        /// <summary>
        /// 上傳路徑
        /// </summary>
        private static readonly string _uploadOssPath = "/Upload/BusinessPhoto/";

        /// <inheritdoc/>
        public override SourceType SourceType => SourceType.BusinessPhoto;

        protected override string UploadOssPath => _uploadOssPath;

        /// <inheritdoc/>
        public BossBusinessImageService(IObjectStorageService oos,
            IMediaRepo repo,
            ILogger<BossBusinessImageService> logger) : base(oos, repo, logger)
        {
        }
    }
}