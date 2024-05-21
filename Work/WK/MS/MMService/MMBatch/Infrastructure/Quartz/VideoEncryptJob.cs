using Microsoft.Extensions.Logging;
using MS.Core.MM.Services.interfaces;
using MS.Core.MMModel.Models.Media.Enums;
using Quartz;

namespace MMBatch.Infrastructure.Quartz
{
    /// <summary>
    /// 將视频加密的排程
    /// </summary>
    public class VideoEncryptJob : BaseJob
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
        private static bool _isEncrypted = false;

        /// <summary>
        /// 媒體服務
        /// </summary>
        private readonly IEnumerable<IMediaService> _services = null;

        /// <summary>
        /// 日誌
        /// </summary>
        private readonly ILogger<VideoEncryptJob> _logger = null;

        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="services">服務</param>
        /// <param name="logger">日誌</param>
        public VideoEncryptJob(IEnumerable<IMediaService> services,
            ILogger<VideoEncryptJob> logger)
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

            if (!_isEncrypted)
            {
                _logger.LogInformation($"Begin VideoEncrypt, From:{_begin}, To:{_end}");
                var services = _services.Where(x => x.Type == MediaType.Video);
                foreach (var service in services)
                {
                    await service.Encrypt(_begin, _end, _finish);
                    await Task.Delay(TimeSpan.FromSeconds(1));
                }

                if (_begin < _finish)
                {
                    _isEncrypted = true;
                    _logger.LogInformation($"VideoEncrypt Job Finish, _end:{_end}, now:{_finish}");
                }
                else
                {
                    _end = _begin;
                }
            }
        }
    }
}