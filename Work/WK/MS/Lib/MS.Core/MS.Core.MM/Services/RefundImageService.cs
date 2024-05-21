﻿using Microsoft.Extensions.Logging;
using MS.Core.Infrastructure.OSS;
using MS.Core.MM.Repos.interfaces;
using MS.Core.MM.Services.interfaces;
using MS.Core.MMModel.Models.Post.Enums;

namespace MS.Core.MM.Services
{
    /// <inheritdoc cref="SourceType.Refund"/>
    public class RefundImageService : PostImageService, IMediaService
    {
        /// <summary>
        /// 上傳路徑
        /// </summary>
        private static readonly string _uploadOssPath = "/Upload/Refund/";

        /// <inheritdoc/>
        public override SourceType SourceType => SourceType.Refund;

        protected override string UploadOssPath => _uploadOssPath;

        /// <inheritdoc/>
        public RefundImageService(IObjectStorageService oos,
            IMediaRepo repo,
            ILogger<RefundImageService> logger) : base(oos, repo, logger)
        {
        }
    }
}