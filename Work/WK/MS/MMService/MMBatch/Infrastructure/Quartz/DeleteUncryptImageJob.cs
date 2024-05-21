using Microsoft.Extensions.Logging;
using MS.Core.Infrastructure.OSS;
using Quartz;

namespace MMBatch.Infrastructure.Quartz
{
    /// <summary>
    /// 將圖片加密的排程
    /// </summary>
    public class DeleteUncryptImageJob : BaseJob
    {
        /// <inheritdoc cref="IObjectStorageService"/>
        private readonly IObjectStorageService _service;

        /// <summary>
        /// 日誌
        /// </summary>
        private readonly ILogger<DeleteUncryptImageJob> _logger = null;

        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="service">服務</param>
        /// <param name="logger">日誌</param>
        public DeleteUncryptImageJob(IObjectStorageService service,
            ILogger<DeleteUncryptImageJob> logger)
        {
            _service = service;
            _logger = logger;
        }

        public override async Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation("Begin DeleteUncryptImage {now}", DateTime.Now);
            await _service.ExcutePaginators(async (key) =>
            {
                var ext = Path.GetExtension(key).ToLower();
                if (!string.Equals(ext, ".aes"))
                {
                    await _service.DeleteObject(key);
                }
            });

            _logger.LogInformation("End DeleteUncryptImage {now}", DateTime.Now);
        }
    }
}
