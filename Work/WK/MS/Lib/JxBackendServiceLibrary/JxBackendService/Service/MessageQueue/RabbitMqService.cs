using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.Queue;
using JxBackendService.Model.MessageQueue;
using JxBackendService.Model.Param.Finance;
using JxBackendService.Model.ViewModel.ThirdParty;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMqUtil;
using System;
using System.Text;
using System.Threading.Tasks;

namespace JxBackendService.Service.MessageQueue
{
    public class RabbitMqService : IMessageQueueService
    {
        private static readonly string s_taskExpiration = (60 * 60 * 1000).ToString();

        private static readonly int s_retryDequeueWaitSeconds = 5;

        private static CommonRabbitmqManager s_commonRabbitmqManager;

        public RabbitMqService(JxApplication jxApplication)
        {
            var appSettingService = DependencyUtil.ResolveKeyedForModel<IAppSettingService>(jxApplication, SharedAppSettings.PlatformMerchant);
            RabbitMQSetting rabbitMQSetting = appSettingService.GetRabbitMQSetting();

            s_commonRabbitmqManager = AssignValueOnceUtil.GetAssignValueOnce(
                s_commonRabbitmqManager,
                () =>
                {
                    return new CommonRabbitmqManager(
                        rabbitMQSetting.HostName,
                        rabbitMQSetting.Port,
                        rabbitMQSetting.UserName,
                        rabbitMQSetting.Password);
                });
        }

        public void EnqueueTransferToMiseLiveMessage(TransferToMiseLiveParam param)
        {
            Enqueue(TaskQueueName.TransferToMiseLive, param);
        }

        public void EnqueueTransferAllOutMessage(PlatformProduct platformProduct, TransferOutUserDetail param)
        {
            Enqueue(TaskQueueName.TransferAllOut(platformProduct), param);
        }

        public void StartNewDequeueJob(TaskQueueName taskQueueName, Func<string, bool> doJobAfterReceived)
        {
            Task.Run(() =>
            {
                while (true)
                {
                    try
                    {
                        DoActionByTaskQueueChannel(
                            taskQueueName,
                            (taskQueueChannel) =>
                            {
                                var queueingBasicConsumer = new QueueingBasicConsumer(taskQueueChannel.Channel);
                                taskQueueChannel.Channel.BasicConsume(taskQueueName.Value, noAck: false, queueingBasicConsumer);

                                BasicDeliverEventArgs basicDeliverEventArgs = queueingBasicConsumer.Queue.Dequeue();

                                byte[] body = basicDeliverEventArgs.Body;
                                string message = Encoding.UTF8.GetString(body);

                                if (doJobAfterReceived.Invoke(message))
                                {
                                    taskQueueChannel.Channel.BasicAck(basicDeliverEventArgs.DeliveryTag, multiple: false);
                                }
                            });
                    }
                    catch (Exception ex)
                    {
                        LogUtil.Error(ex);
                        Task.Delay(s_retryDequeueWaitSeconds * 1000).Wait();
                    }
                }
            });
        }

        private void Enqueue<T>(TaskQueueName taskQueueName, T model)
        {
            DoActionByTaskQueueChannel(
                taskQueueName,
                (taskQueueChannel) =>
                {
                    byte[] body = Encoding.UTF8.GetBytes(model.ToJsonString());
                    taskQueueChannel.Channel.BasicPublish(string.Empty, taskQueueName.Value, taskQueueChannel.BasicProperties, body); //開始傳遞
                });
        }

        private void DoActionByTaskQueueChannel(TaskQueueName taskQueueName, Action<TaskQueueChannel> action)
        {
            using (IModel channel = s_commonRabbitmqManager.GetConnection().CreateModel())
            {
                channel.QueueDeclare(taskQueueName);
                IBasicProperties basicProperties = channel.CreateBasicPropertiesForTaskQueue();
                basicProperties.Expiration = s_taskExpiration;

                var taskQueueChannel = new TaskQueueChannel()
                {
                    Channel = channel,
                    BasicProperties = basicProperties
                };

                action.Invoke(taskQueueChannel);
            }
        }
    }
}