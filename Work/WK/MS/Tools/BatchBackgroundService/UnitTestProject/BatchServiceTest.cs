using Autofac;
using BatchService.Interface;
using BatchService.Model.Enum;
using BatchService.Service;
using JxBackendService.Common.Extensions;
using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Model.MiseLive.ViewModel;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.MiseLive;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.Chat;
using JxBackendService.Model.MessageQueue;
using JxBackendService.Model.Param.Chat;
using JxBackendService.Model.Param.SMS;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using JxBackendServiceN6.Common.Util;
using JxMsgEntities;
using ProductTransferService;
using RabbitMQ.Client;
using System.Text;
using TencentCloud.Trp.V20210515.Models;
using UnitTestN6;

namespace UnitTestProject
{
    [TestClass]
    public class BatchServiceTest : BaseUnitTest
    {
        protected override EnvironmentUser EnvironmentUser => new EnvironmentUser()
        {
            Application = Application,
            LoginUser = new BasicUserInfo()
        };

        protected override void AppendServiceToContainerBuilder(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<EnvironmentService>().AsImplementedInterfaces();
        }

        public BatchServiceTest()
        {
        }

        [TestMethod]
        public async Task TestBatchMainMockServiceAsync()
        {
            await RunHostedServiceAsync<BatchMockService>();
        }

        [TestMethod]
        public void TestJobSettingMSService()
        {
            var appSettingService = DependencyUtil.ResolveKeyed<IAppSettingService>(Application, SharedAppSettings.PlatformMerchant);
            var jobSettingMSService = DependencyUtil.ResolveKeyed<IJobSettingService>(PlatformMerchant.MiseLiveStream).Value;
            List<JobSetting> actual = jobSettingMSService.GetAll();
            List<JobSetting> expected = new List<JobSetting>()
            {
                JobSetting.StoredProcedureErrorNotice,
            };

            string actualJsonString = actual.ToJsonString();
            string expectedJsonString = expected.ToJsonString();
            Assert.AreEqual(expectedJsonString, actualJsonString);
        }

        [TestMethod]
        public void CreateSendAddMessageQueueParam()
        {
            string json = string.Empty;

            var send = new SendAddMessageQueueParam
            {
                PublishUserID = 588,
                OwnerUserID = 888,
                Message = "TEST",
                MessageTypeValue = MessageType.Text.Value,
                MessageID = 1179672187716505620,
                PublishTimestamp = DateTime.UtcNow.ToUnixOfTime(),
                RoomID = 588.ToString(),
            };

            json = send.ToJsonString();

            send = new SendAddMessageQueueParam
            {
                PublishUserID = 588,
                OwnerUserID = 888,
                Message = "TEST1",
                MessageTypeValue = MessageType.Text.Value,
                MessageID = 1179672187716505621,
                PublishTimestamp = DateTime.UtcNow.ToUnixOfTime(),
                RoomID = 588.ToString(),
            };

            json = send.ToJsonString();

            send = new SendAddMessageQueueParam
            {
                PublishUserID = 888,
                OwnerUserID = 2222,
                Message = "TEST2",
                MessageTypeValue = MessageType.Text.Value,
                MessageID = 1179672187716505622,
                PublishTimestamp = DateTime.UtcNow.ToUnixOfTime(),
                RoomID = 2222.ToString(),
            };

            json = send.ToJsonString();
        }

        [TestMethod]
        public void CreateSMSParam()
        {
            var send = new SendUserSMSParam
            {
                ContentParamInfo = "TEST",
                PhoneNo = "1566999999",
                Usage = SMSUsages.ValidateCode,
                CountryCode = CountryCode.China.Value
            };

            string json = send.ToJsonString();
        }

        [TestMethod]
        public void TestCrawlOBEBAnchors()
        {
            var service = DependencyUtil.ResolveJxBackendService<ITPLiveStreamService>(EnvironmentUser, DbConnectionTypes.Master).Value;
            BaseReturnModel baseReturnDataModel = service.CrawlAnchors();

            LogUtil.ForcedDebug(baseReturnDataModel.ToJsonString());

            List<IMiseLiveAnchor> anchorsResult = service.GetAnchors();

            Assert.IsTrue(anchorsResult.AnyAndNotNull());
        }

        [TestMethod]
        public void TestSendMessage()
        {
            var appSettingService = DependencyUtil.ResolveKeyed<IAppSettingService>(Application, SharedAppSettings.PlatformMerchant).Value;
            RabbitMQSetting rabbitMQSetting = appSettingService.GetEndUserRabbitMQSettings().First();

            var connectionFactory = new ConnectionFactory
            {
                HostName = rabbitMQSetting.HostName,
                Port = rabbitMQSetting.Port,
                UserName = rabbitMQSetting.UserName,
                Password = rabbitMQSetting.Password,
                VirtualHost = rabbitMQSetting.VirtualHost,
                RequestedHeartbeat = TimeSpan.FromSeconds(60.0),
                RequestedConnectionTimeout = TimeSpan.FromSeconds(3.0)
            };

            var messageEntity = new MessageEntity()
            {
                MessageType = RabbitMessageType.Heartbeat,
                SendExchange = RQSettings.RQ_WCF_EXCHANGE,
                SendRoutKey = "9999",
                SendTime = DateTime.Now.ToFormatDateString(),
                SendContent = new { test = "test" },
            };

            using (IConnection connection = connectionFactory.CreateConnection())
            {
                while (true)
                {
                    using (IModel sendMessageChannel = connection.CreateModel())
                    {
                        IBasicProperties basicProperties = sendMessageChannel.CreateBasicProperties();
                        basicProperties.Expiration = "500000";
                        string jsonString = messageEntity.ToJsonString();
                        byte[] bytes = Encoding.UTF8.GetBytes(jsonString);
                        sendMessageChannel.BasicPublish(messageEntity.SendExchange, messageEntity.SendRoutKey, basicProperties, bytes);
                    }

                    Task.Delay(10000).Wait();
                }
            }
        }

        [TestMethod]
        public void TestSendChatNotification()
        {
            var messageQueueService = DependencyUtil.ResolveService<IMessageQueueService>().Value;

            while (true)
            {
                messageQueueService.SendChatNotification(new ChatNotificationParam()
                {
                    ChatNotificationInfo = new ChatNotificationInfo()
                    {
                        ChatNotificationType = ChatNotificationTypes.NewMessage,
                        Message = "test",
                        MessageID = 1,
                        PublishTimestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                        RoomID = "1",
                    },
                    RoutingKey = "9999"
                });

                TaskUtil.DelayAndWait(10 * 1000);
            }
        }

        [TestMethod]
        public void TestEnqueueUnitTest()
        {
            var internalMessageQueueService = DependencyUtil.ResolveService<IInternalMessageQueueService>().Value;

            while (true)
            {
                internalMessageQueueService.EnqueueUnitTestMessage(new { test = "test" });

                TaskUtil.DelayAndWait(10 * 1000);
            }
        }
    }
}