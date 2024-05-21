using ImageMagick;
using Microsoft.Extensions.Logging;
using MS.Core.Infrastructure.OSS;
using MS.Core.MM.Model.Media;
using MS.Core.MM.Repos.interfaces;
using MS.Core.MM.Services;
using MS.Core.MM.Services.interfaces;
using MS.Core.MMModel.Models.Post.Enums;
using MS.Core.Models;
using System.Drawing;

namespace MS.Core.MM.Service
{
    /// <summary>
    /// Banner圖片服務
    /// </summary>
    public class BannerImageService : PostImageService, IMediaService
    {
        /// <summary>
        /// 上傳路徑
        /// </summary>
        private static readonly string _uploadOssPath = "/Upload/Banner/";

        /// <summary>
        /// 圖片副檔名
        /// </summary>
        private static readonly string[] _imageFileExtension = new string[] { ".jpg", ".png" };

        /// <summary>
        /// 圖片大小
        /// </summary>
        private static readonly Size _imageSize = new Size { Width = 710, Height = 180 };

        /// <summary>
        /// 圖片檔案大小(MB)
        /// </summary>
        private static readonly int _imageMBLimit = 1;

        /// <summary>
        /// 圖片檔案大小
        /// </summary>
        protected override int ImageSizeLimit => ImageMBLimit * 1024 * 1024;

        /// <inheritdoc/>
        public override SourceType SourceType => SourceType.Banner;

        /// <inheritdoc/>
        protected override string[] ImageFileExtension => _imageFileExtension;

        /// <inheritdoc/>
        protected virtual Size ImageSize => _imageSize;

        /// <inheritdoc/>
        protected override int ImageMBLimit => _imageMBLimit;

        protected override string UploadOssPath => _uploadOssPath;

        /// <inheritdoc/>
        public BannerImageService(IObjectStorageService oos,
            IMediaRepo repo,
            ILogger<BannerImageService> logger) : base(oos, repo, logger)
        {
        }

        /// <inheritdoc/>
        protected override async Task<BaseReturnModel> ChildCheckParam(SaveMediaToOssParam param)
        {
            if (!IsValidImageSize(param.Bytes, ImageSize))
            {
                return new BaseReturnModel(ReturnCode.ParameterIsInvalid);
            }

            return await Task.FromResult(new BaseReturnModel(ReturnCode.Success));
        }


        /// <summary>
        /// 驗證圖片大小
        /// </summary>
        /// <param name="imageBytes"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        protected virtual bool IsValidImageSize(byte[] imageBytes, Size size)
        {
            using (var image = new MagickImage(new MemoryStream(imageBytes)))
            {
                if (size.Width == image.Width && size.Height == image.Height)
                {
                    return true;
                }
            }

            return false;
        }
    }
}