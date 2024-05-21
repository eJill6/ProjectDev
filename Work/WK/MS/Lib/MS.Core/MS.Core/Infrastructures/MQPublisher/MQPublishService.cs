using EasyNetQ;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MS.Core.Extensions;
using MS.Core.Infrastructures.MQPublisher.Models;
using MS.Core.Services;

namespace MS.Core.Infrastructures.MQPublisher
{
    public class MQPublishService : BaseService, IMQPublishService
    {
        public static readonly SemaphoreSlim _lock = new SemaphoreSlim(1);
        public static int _index = 0;
        public IOptionsMonitor<MQSettings> MQSettings { get; }

        public MQPublishService(IOptionsMonitor<MQSettings> mqSettingsOptions, ILogger logger) : base(logger)
        {
            MQSettings = mqSettingsOptions;
        }

        public async Task DeleteChatMessages(int userId, int roomId)
        {
            var param = new DeletemessageParam
            {
                OwnerUserID = userId.ToString(),
                RoomID = roomId.ToString(),
                SmallEqualThanTimestamp = DateTime.Now.ToUnixOfTime()
            };
            try
            {

                var retryCount = MQSettings.CurrentValue.Datas.Length;
                for (var i = 0; i < retryCount; i++)
                {

                    try
                    {
                        var setting = MQSettings.CurrentValue.Datas.Length != 0 ? MQSettings.CurrentValue.Datas[_index] : MQSettings.CurrentValue;
                        if (MQSettings.CurrentValue.Datas.Length != 0)
                        {
                            await _lock.WaitAsync();
                            _index = (_index + 1) % MQSettings.CurrentValue.Datas.Length;
                        }
                        using (var bus = RabbitHutch.CreateBus(setting.GetMQConnection().MQConnectionStr))
                        {
                            await bus.SendReceive.SendAsync("MiseLive.DeleteChatMessage", param);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"DeleteChatMessages publish failed, param:{param}, Exception:{ex.Message}");
                        if (i >= retryCount - 1)
                        {
                            throw;
                        }
                        continue;
                    }
                    finally
                    {
                        _lock.Release();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"DeleteChatMessages retry publish failed, param:{param}, Exception:{ex.Message}");
            }
        }
    }
}
