using EasyNetQ;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MS.Core.Infrastructures.ZeroOne.Models;
using MS.Core.Infrastructures.ZeroOne.Models.Responses;
using MS.Core.MM.Repos.interfaces;
using MS.Core.Utils;
using Newtonsoft.Json;
using System.Text;

namespace MMBatch.Infrastructure.HostedService
{
    public class VideoEventService : IHostedService
    {
        private readonly ILogger<VideoEventService> _logger;
        private readonly IOptions<ZeroOneSettings> _zeroOneSettingsOptions;
        private IBus _bus = null;
        private readonly IMediaRepo _repo = null;

        public VideoEventService(IOptions<ZeroOneSettings> zeroOneSettingsOptions,
            IMediaRepo repo,
            ILogger<VideoEventService> logger)
        {
            _logger = logger;
            _repo = repo;
            _zeroOneSettingsOptions = zeroOneSettingsOptions;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            if (_bus == null)
            {
                _bus = RabbitHutch.CreateBus(_zeroOneSettingsOptions.Value.RabbitMqConnection);
                const string queueKey = "mmservice.recive.process.event";
                const string exchange = "shark.topic";
                const string routing_key = "media.video.convert_finish";
                var queue = await _bus.Advanced.QueueDeclareAsync(queueKey);
                _bus.Advanced.Bind(new EasyNetQ.Topology.Exchange(exchange, "topic"), queue, routing_key);
                _bus.Advanced.Consume(queue: queue, handler: Handle);
            }
            await Task.CompletedTask;
        }

        private async Task Handle(ReadOnlyMemory<byte> body, MessageProperties properties, MessageReceivedInfo info)
        {
            var message = Encoding.UTF8.GetString(body.ToArray());
            _logger.LogInformation($"Handle Got message: {message}");
            try
            {
                if (!string.IsNullOrEmpty(message))
                {
                    var data = JsonConvert.DeserializeObject<ZOMQFinishEvent>(message);
                    if (data.operation == "task_over")
                    {
                        _logger.LogInformation($"Handle finish event, message: {message}");
                        var result = await _repo.UpdateUrl(data.id.ToString(), data.converted_path);
                        if (!result.IsSuccess)
                        {
                            _logger.LogError($"Handle Update fail, result: {JsonUtil.ToJsonString(result)}, data: {JsonUtil.ToJsonString(data)}");
                        }
                    }
                    else
                    {
                        _logger.LogInformation($"Handle got event, message: {message}");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Handle Message fail, message: {message}");
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            if (_bus != null)
            {
                _bus.Dispose();
                _bus = null;
            }
            await Task.CompletedTask;
        }
    }
}
