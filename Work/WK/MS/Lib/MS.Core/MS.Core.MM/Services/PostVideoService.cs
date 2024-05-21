using Microsoft.Extensions.Logging;
using MS.Core.Extensions;
using MS.Core.Infrastructures.ZoneOne;
using MS.Core.MM.Model.Media;
using MS.Core.MM.Models;
using MS.Core.MM.Repos;
using MS.Core.MM.Repos.interfaces;
using MS.Core.MM.Services.interfaces;
using MS.Core.MMModel.Models.Post.Enums;
using MS.Core.Models;

namespace MS.Core.MM.Service
{
    /// <summary>
    /// 媒體服務的基礎型別
    /// </summary>
    public class PostVideoService : BaseVideoService, IMediaService
    {
        /// <inheritdoc/>
        public override SourceType SourceType => SourceType.Post;

        /// <summary>
        /// 圖片檔案大小
        /// </summary>
        protected virtual int ImageSizeLimit => 50 * 1024 * 1024;

        /// <summary>
        /// 影片副檔名
        /// </summary>
        protected virtual string[] FileExtension => new string[] { ".mp4" };

        public PostVideoService(
            IZeroOneApiService zeroOne,
            IMediaRepo repo,
            IPostRepo postRepo,
            ILogger<PostVideoService> logger) : base(repo, zeroOne, postRepo, logger)
        {
        }

        /// <summary>
        /// 由子類別來實作相對應的檢查
        /// </summary>
        /// <param name="param">輸入參數</param>
        /// <returns>驗證結果</returns>
        protected override async Task<BaseReturnModel> ChildCheckParam(SaveMediaToOssParam param)
        {
            var extRaw = Path.GetExtension(param.FileName);
            var ext = extRaw.ToLower();
            if (!FileExtension.Contains(ext) ||
                !param.Bytes.AnyAndNotNull())
            {
                _logger.LogInformation($"{param.FileName} extension not allow");
                return new BaseReturnModel(ReturnCode.ParameterIsInvalid);
            }

            if (param.Bytes.AnyAndNotNull() && param.Bytes.Length > ImageSizeLimit)
            {
                return new BaseReturnModel(MMReturnCode.ImageSizeMoreThanLimit);
            }

            return await Task.FromResult(new BaseReturnModel(ReturnCode.Success));
        }
    }
}