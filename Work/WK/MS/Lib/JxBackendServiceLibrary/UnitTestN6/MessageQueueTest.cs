using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.Util;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.Queue;
using JxBackendService.Model.Param.Chat;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnitTestN6;
using JxBackendService.Interface.Service.Config;
using JxBackendService.Model.MessageQueue;
using System.Collections.Generic;
using JxBackendService.Model.Common;
using System.Linq;

namespace UnitTest.Util
{
    [TestClass]
    public class MessageQueueTest : BaseUnitTest
    {
        private readonly Lazy<IMessageQueueService> _messageQueueService;

        private readonly Lazy<IInternalMessageQueueService> _internalMessageQueueService;

        private readonly Lazy<ILogUtilService> _logUtilService;

        private readonly Lazy<IConfigUtilService> _configUtilService;
        

        public MessageQueueTest()
        {
            _messageQueueService = DependencyUtil.ResolveService<IMessageQueueService>();
            _internalMessageQueueService = DependencyUtil.ResolveService<IInternalMessageQueueService>();
            _logUtilService = DependencyUtil.ResolveService<ILogUtilService>();
            _configUtilService = DependencyUtil.ResolveService<IConfigUtilService>();
        }

        [TestMethod]
        public void TestInternalMessageQueue()
        {
            //Task.Run(() =>
            //{
            //    int seq = 0;

            //    var stringBuilder = new StringBuilder();

            //    for (int i = 1; i <= 20; i++)
            //    {
            //        stringBuilder.Append("Aasd;flkjasd;lfkjas;dlfkjas;");
            //    }

            //    string content = stringBuilder.ToString();

            //    while (true)
            //    {
            //        _internalmessageQueueService.Value.EnqueueUnitTestMessage(new { content });
            //        seq++;
            //        //Task.Delay(100).Wait();
            //    }
            //});

            //var internalMessageQueueService = DependencyUtil.ResolveService<IInternalMessageQueueService>();

            //for (int i = 1; i <= 10; i++)
            //{
            //    int seq = i;
            //    _internalMessageQueueService.Value.EnqueueUnitTestMessage(new { seq });
            //}

            for (int i = 1; i <= 1; i++)
            {
                _internalMessageQueueService.Value.StartNewDequeueJob(TaskQueueName.UnitTest, (doDequeueJobAfterReceivedParam) =>
                {
                    Console.WriteLine(doDequeueJobAfterReceivedParam.ToJsonString());

                    return Convert.ToBoolean(doDequeueJobAfterReceivedParam.Message);
                });
            }

            Task.Delay(-1).Wait();
        }

        [TestMethod]
        public void DeleteMessage()
        {
            var deleteMessage = new DeleteMessageQueueParam
            {
                OwnerUserID = 888,
                RoomID = "2607",
                SmallEqualThanTimestamp = DateTime.UtcNow.ToUnixOfTime()
            };

            string jsonString = deleteMessage.ToJsonString();
        }

        [TestMethod]
        public async Task MassConnectionsWithExchangeFanout()
        {
            var appSettingService = DependencyUtil.ResolveKeyed<IAppSettingService>(Application, SharedAppSettings.PlatformMerchant).Value;
            RabbitMQSetting rabbitMQSetting = appSettingService.GetEndUserRabbitMQSettings().First();

            var factory = new ConnectionFactory()
            {
                HostName = rabbitMQSetting.HostName,
                Port = rabbitMQSetting.Port,
                UserName = rabbitMQSetting.UserName,
                Password = rabbitMQSetting.Password
            };

            var tasks = new List<Task>();

            for (int i = 0; i < 500; i++)
            {
                tasks.Add(ReceiveAsync(factory));
            }

            await Task.WhenAll(tasks);
        }

        [TestMethod]
        public void InternalMessageQueueTest()
        {
        }

        private async Task ReceiveAsync(ConnectionFactory factory)
        {
            try
            {
                using (var connection = factory.CreateConnection())
                {
                    var eventingBasicConsumers = new List<EventingBasicConsumer>();
                    var channels = new List<IModel>();

                    for (int i = 1; i <= 100; i++)
                    {
                        var channel = connection.CreateModel();
                        channels.Add(channel);

                        var queueName = $"XiaoRenTest_{i}_{Guid.NewGuid()}";
                        channel.QueueDeclare(queueName, durable: false, exclusive: false, autoDelete: true);

                        channel.QueueBind(
                            queue: queueName,
                            exchange: "HECBET_REFRESHLOTTERY_FANOUT",
                            routingKey: string.Empty);

                        Console.WriteLine($"開始接收訊息 (Queue: {queueName})...");

                        var consumer = new EventingBasicConsumer(channel);
                        eventingBasicConsumers.Add(consumer);

                        consumer.Received += (model, ea) =>
                        {
                            var body = ea.Body.ToArray();
                            var message = Encoding.UTF8.GetString(body);
                            Console.WriteLine($"收到訊息 (Queue: {queueName}): {message}");
                            channel.BasicAck(ea.DeliveryTag, multiple: false);
                        };

                        channel.BasicConsume(
                            queue: queueName,
                            autoAck: false,
                            consumer: consumer);
                    }

                    await Task.Delay(Timeout.Infinite); // 保持連線
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}