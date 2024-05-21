using System;
using System.Collections.Generic;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.Chat;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.Chat;
using JxBackendService.Model.Param.Chat;
using JxBackendService.Common.Util;
using Autofac;

namespace UnitTestN6
{
    [TestClass]
    public class ChatTest : BaseUnitTest
    {
        private readonly Lazy<IIdGeneratorService> _idGeneratorService;

        private readonly Lazy<IChatMessageService> _chatMessageService;

        protected override void AppendServiceToContainerBuilder(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<UnitTestBatchEnvironmentService>().AsImplementedInterfaces();
        }

        private readonly List<int> randomUserIDs = new List<int>() { 888, 2384, 4646, 6251 };

        public ChatTest()
        {
            _idGeneratorService = DependencyUtil.ResolveJxBackendService<IIdGeneratorService>(EnvironmentUser, DbConnectionTypes.Master);
            _chatMessageService = DependencyUtil.ResolveJxBackendService<IChatMessageService>(EnvironmentUser, DbConnectionTypes.IMsgMaster);
        }

        [TestMethod]
        public void CreateChatMessage()
        {
            for (int i = 0; i < 10000; i++)
            {
                int randomIndex = new Random().Next(0, randomUserIDs.Count);

                var param = new SendAddMessageQueueParam()
                {
                    OwnerUserID = 2607,
                    Message = $"hello{i}_{randomUserIDs[randomIndex]}",
                    MessageTypeValue = MessageType.Text.Value,
                    MessageID = _idGeneratorService.Value.CreateId(),
                    PublishUserID = randomUserIDs[randomIndex],
                    RoomID = randomUserIDs[randomIndex].ToString(),
                    PublishTimestamp = DateTime.UtcNow.ToUnixOfTime()
                };

                _chatMessageService.Value.AddChatMessage(param);
            }
        }
    }
}