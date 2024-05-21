using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.Enums;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums.Queue;
using JxBackendService.Model.MessageQueue;
using JxMsgEntities;
using RabbitMQ.Client;
using System.Collections.Generic;
using System.Linq;

namespace JxBackendService.Service.MessageQueue.Init
{
    public class EndUserRabbitMqInitService : BaseRabbitMqInitService, IEndUserRabbitMqInitService
    {
        protected override List<RabbitMQSetting> GetRabbitMQSettings() => AppSettingService.GetEndUserRabbitMQSettings();

        protected override void InitExchanges(RabbitMQClientManager rabbitMQClientManager)
        {
            using (IModel channel = rabbitMQClientManager.GetConnection().CreateModel())
            {
                const string exchangeTypeDirect = "direct";
                const string exchangeTypeFanout = "fanout";

                var exchanges = new Dictionary<string, string>
                {
                    { RQSettings.RQ_WCF_EXCHANGE, exchangeTypeDirect },
                    { RQSettings.RQ_BACKSIDEWEB_EXCHANGE, exchangeTypeDirect },
                    { RQSettings.RQ_MISELIVE_CHAT_EXCHANGE, exchangeTypeDirect },
                    { RQSettings.RQ_HECBET_REFRESHLOTTERY_FANOUT, exchangeTypeFanout }
                };

                foreach (KeyValuePair<string, string> exchange in exchanges)
                {
                    channel.ExchangeDeclare(
                        exchange: exchange.Key,
                        type: exchange.Value,
                        durable: true,
                        autoDelete: false);
                }
            }
        }

        protected override void InitQueues(RabbitMQClientManager rabbitMQClientManager)
        {
            using (IModel channel = rabbitMQClientManager.GetConnection().CreateModel())
            {
                List<TaskQueueName> taskQueueNames = TaskQueueName
                    .GetAll()
                    .Where(w => !w.IsAutoDelete && w.MQBusinessType == MQBusinessTypes.EndUser)
                    .ToList();

                var platformProductService = DependencyUtil.ResolveKeyed<IPlatformProductService>(
                    EnvironmentService.Application,
                    SharedAppSettings.PlatformMerchant);

                foreach (TaskQueueName taskQueueName in taskQueueNames)
                {
                    QueueDeclare(channel, taskQueueName);
                }
            }
        }
    }
}