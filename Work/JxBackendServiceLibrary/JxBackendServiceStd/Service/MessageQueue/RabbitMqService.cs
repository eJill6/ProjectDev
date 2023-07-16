using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.Queue;
using JxBackendService.Model.MessageQueue;
using JxBackendService.Model.Param.Finance;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxMsgEntities;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMqUtil;
using System;
using System.Text;
using System.Threading;

namespace JxBackendService.Service.MessageQueue
{
    public class RabbitMqService : IMessageQueueService
    {
        private static readonly string s_taskExpiration = (60 * 60 * 1000).ToString();

        private static CommonRabbitmqManager s_commonRabbitmqManager;

        private static JxApplication _jxApplication;

        public RabbitMqService(JxApplication jxApplication)
        {
            _jxApplication = jxApplication;

            var appSettingService = DependencyUtil.ResolveKeyedForModel<IAppSettingService>(_jxApplication, SharedAppSettings.PlatformMerchant);
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

        public void EnqueueUnitTestMessage<T>(T param)
        {
            Enqueue(TaskQueueName.UnitTest, param);
        }

        public void StartNewDequeueJob(TaskQueueName taskQueueName, Func<string, bool> doJobAfterReceived)
        {
            IModel channel = s_commonRabbitmqManager.GetConnection().CreateModel();

            channel.QueueDeclare(taskQueueName);
            IBasicProperties basicProperties = channel.CreateBasicPropertiesForTaskQueue();
            basicProperties.Expiration = s_taskExpiration;

            channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: true);
            var eventingBasicConsumer = new EventingBasicConsumer(channel);

            eventingBasicConsumer.Received += (object sender, BasicDeliverEventArgs basicDeliverEventArgs) =>
            {
                try
                {
                    ReadOnlyMemory<byte> body = basicDeliverEventArgs.Body;
                    string message = Encoding.UTF8.GetString(body.ToArray());

                    if (doJobAfterReceived.Invoke(message))
                    {
                        channel.BasicAck(basicDeliverEventArgs.DeliveryTag, multiple: false);
                    }
                }
                catch (Exception ex)
                {
                    ErrorMsgUtil.ErrorHandle(ex, new EnvironmentUser
                    {
                        Application = _jxApplication,
                        LoginUser = new BasicUserInfo { }
                    });

                    TaskUtil.DelayAndWait(millisecondsDelay: 5 * 1000);
                }
            };

            channel.BasicConsume(taskQueueName.Value, autoAck: false, eventingBasicConsumer);
        }

        public void SendTransferMessage(TransferMessage transferMessage)
        {
            s_commonRabbitmqManager.SendMessage(new MessageEntity
            {
                MessageType = RabbitMessageType.TransferNotice,
                SendTime = DateTime.Now.ToFormatDateTimeString(),
                SendExchange = RQSettings.RQ_WCF_EXCHANGE,
                SendRoutKey = transferMessage.RoutingKey,
                SendContent = transferMessage.ToJsonString()
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