using Microsoft.Extensions.Logging;
using MS.Core.Extensions;
using MS.Core.Infrastructure.OSS;
using MS.Core.MM.Model.Media;
using MS.Core.MM.Models;
using MS.Core.MM.Repos.interfaces;
using MS.Core.MM.Service;
using MS.Core.MM.Services.interfaces;
using MS.Core.MMModel.Models.Post.Enums;
using MS.Core.Models;

namespace MS.Core.MM.Services
{
    /// <inheritdoc cref="SourceType.Post"/>
    public class PostImageService : BaseImageService, IMediaService
    {
        /// <summary>
        /// 上傳路徑
        /// </summary>
        private static readonly string _uploadOssPath = "/Upload/Post/";


        /// <summary>
        /// ios要特殊轉檔
        /// </summary>
        private static readonly string[] _iosExtension = new string[] { ".heic", ".heif" };

        /// <summary>
        /// 圖片副檔名
        /// </summary>
        private static readonly string[] _imageFileExtension = new string[] { ".jpeg", ".jpg", ".png", ".heic", ".heif" };

        /// <summary>
        /// 圖片檔案大小(MB)
        /// </summary>
        private static readonly int _imageMBLimit = 5;

        /// <summary>
        /// 圖片檔案大小
        /// </summary>
        protected virtual int ImageSizeLimit => ImageMBLimit * 1024 * 1024;

        /// <inheritdoc/>
        public override SourceType SourceType => SourceType.Post;

        /// <inheritdoc/>
        protected virtual string[] ImageFileExtension => _imageFileExtension;

        /// <inheritdoc/>
        protected virtual int ImageMBLimit => _imageMBLimit;

        protected override string UploadOssPath => _uploadOssPath;

        protected override string ApplicationSoruce => "MMService";

        /// <inheritdoc/>
        public PostImageService(IObjectStorageService oos,
            IMediaRepo repo,
            ILogger<PostImageService> logger) : base(oos, repo, logger)
        {
        }

        /// <inheritdoc/>
        protected override async Task<BaseReturnModel> ChildCheckParam(SaveMediaToOssParam param)
        {
            var extRaw = Path.GetExtension(param.FileName);
            var ext = extRaw.ToLower();
            if (!ImageFileExtension.Contains(ext) ||
                !param.Bytes.AnyAndNotNull())
            {
                _logger.LogInformation($"{param.FileName} extension not allow");
                return new BaseReturnModel(ReturnCode.ParameterIsInvalid);
            }

            if (_iosExtension.Contains(ext))
            {
                var convert = await ConvertImage(param.Bytes);
                if (convert.Item1)
                {
                    param.Bytes = convert.Item2;
                    param.FileName = string.Concat(param.FileName.Replace(extRaw, string.Empty), ".jpg");
                }
                else
                {
                    return new BaseReturnModel(ReturnCode.ParameterIsInvalid);
                }
            }

            if (param.Bytes.AnyAndNotNull() && param.Bytes.Length > ImageSizeLimit)
            {
                return new BaseReturnModel(MMReturnCode.ImageSizeMoreThanLimit);
            }

            return await Task.FromResult(new BaseReturnModel(ReturnCode.Success));
        }
    }
}
