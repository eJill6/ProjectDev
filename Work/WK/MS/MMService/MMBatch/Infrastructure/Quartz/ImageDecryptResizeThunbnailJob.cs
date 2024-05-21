using Microsoft.Extensions.Logging;
using MS.Core.MM.Services.interfaces;
using MS.Core.MMModel.Models.Media.Enums;
using Quartz;

namespace MMBatch.Infrastructure.Quartz
{
    /// <summary>
    /// 將圖片加密的排程
    /// </summary>
    public class ImageDecryptResizeThunbnailJob : BaseJob
    {
        /// <summary>
        /// 開始時間
        /// </summary>
        private static DateTime _begin => _end.AddDays(-1);

        /// <summary>
        /// 一次的結束時間
        /// </summary>
        private static DateTime _end = default(DateTime);

        /// <summary>
        /// 批次最後時間
        /// </summary>
        private static DateTime _finish = new DateTime(2023, 05, 01);

        /// <summary>
        /// 是否都已經加密了
        /// </summary>
        private static bool _isDecrypted = false;

        /// <summary>
        /// 媒體服務
        /// </summary>
        private readonly IEnumerable<IMediaService> _services = null;

        /// <summary>
        /// 日誌
        /// </summary>
        private readonly ILogger<ImageDecryptResizeThunbnailJob> _logger = null;

        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="services">服務</param>
        /// <param name="logger">日誌</param>
        public ImageDecryptResizeThunbnailJob(IEnumerable<IMediaService> services,
            ILogger<ImageDecryptResizeThunbnailJob> logger)
        {
            _services = services;
            _logger = logger;
        }

        public override async Task Execute(IJobExecutionContext context)
        {
            if (_end == default(DateTime))
            {
                _end = DateTime.Now.AddDays(1);
            }

            if (!_isDecrypted)
            {
                _logger.LogError($"Begin DecryptResizeThunbnail, From:{_begin}, To:{_end}");
                var services = _services.Where(x => x.Type == MediaType.Image);
                foreach (var service in services)
                {
                    await service.Decrypt(_begin, _end, _finish, true);
                    await Task.Delay(TimeSpan.FromSeconds(1));
                }

                if (_begin < _finish)
                {
                    _isDecrypted = true;
                    _logger.LogError($"DecryptResizeThunbnail Job Finish, _end:{_end}, now:{_finish}");
                }
                else
                {
                    _end = _begin;
                }
            }
        }
    }
}
