using Microsoft.Extensions.Logging;
using MS.Core.Infrastructure.OSS;
using MS.Core.MM.Repos.interfaces;
using MS.Core.MM.Services.interfaces;
using MS.Core.MMModel.Models.Post.Enums;

namespace MS.Core.MM.Services
{
    /// <inheritdoc cref="SourceType.Comment"/>
    public class CommentImageService : PostImageService, IMediaService
    {
        /// <summary>
        /// 上傳路徑
        /// </summary>
        private static readonly string _uploadOssPath = "/Upload/Comment/";

        /// <inheritdoc/>
        public override SourceType SourceType => SourceType.Comment;

        protected override string UploadOssPath => _uploadOssPath;

        /// <inheritdoc/>
        public CommentImageService(IObjectStorageService oos,
            IMediaRepo repo,
            ILogger<CommentImageService> logger) : base(oos, repo, logger)
        {
        }
    }
}
