using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.Enums;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.Queue;
using JxBackendService.Model.MessageQueue;
using RabbitMQ.Client;
using System.Collections.Generic;
using System.Linq;

namespace JxBackendService.Service.MessageQueue.Init
{
    public class InternalRabbitMqInitService : BaseRabbitMqInitService, IInternalRabbitMqInitService
    {
        protected override List<RabbitMQSetting> GetRabbitMQSettings() => AppSettingService.GetInternalRabbitMQSettings();

        protected override void InitExchanges(RabbitMQClientManager rabbitMQClientManager)
        {
            //do nothing
        }

        protected override void InitQueues(RabbitMQClientManager rabbitMQClientManager)
        {
            using (IModel channel = rabbitMQClientManager.GetConnection().CreateModel())
            {
                List<TaskQueueName> taskQueueNames = TaskQueueName
                    .GetAll()
                    .Where(w => !w.IsAutoDelete && w.MQBusinessType == MQBusinessTypes.Internal)
                    .ToList();

                var platformProductService = DependencyUtil.ResolveKeyed<IPlatformProductService>(
                    EnvironmentService.Application,
                    SharedAppSettings.PlatformMerchant);

                foreach (PlatformProduct platformProduct in platformProductService.Value.GetAll())
                {
                    taskQueueNames.Add(TaskQueueName.TransferAllOut(platformProduct));
                    taskQueueNames.Add(TaskQueueName.UpdateTPGameUserScore(platformProduct));
                }

                foreach (TaskQueueName taskQueueName in taskQueueNames)
                {
                    QueueDeclare(channel, taskQueueName);
                }
            }
        }
    }
}