using Microsoft.Extensions.Logging;
using MS.Core.Infrastructure.OSS;
using MS.Core.MM.Repos.interfaces;
using MS.Core.MM.Services.interfaces;
using MS.Core.MMModel.Models.Post.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS.Core.MM.Services
{
    public class BossApplyCoverPhotoImageService : PostImageService, IMediaService
    {
        /// <summary>
        /// 上傳路徑
        /// </summary>
        private static readonly string _uploadOssPath = "/Upload/BusinessPhoto/";

        /// <inheritdoc/>
        public override SourceType SourceType => SourceType.BossApply;

        protected override string UploadOssPath => _uploadOssPath;

        /// <inheritdoc/>
        public BossApplyCoverPhotoImageService(IObjectStorageService oos,
            IMediaRepo repo,
            ILogger<BossApplyCoverPhotoImageService> logger) : base(oos, repo, logger)
        {
        }
    }
}
