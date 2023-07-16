using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.Queue;
using JxBackendServiceNF.Common.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using System.Threading.Tasks;
using UnitTest.Base;

namespace UnitTest.Util
{
    [TestClass]
    public class MessageQueueTest : BaseTest
    {
        private readonly IMessageQueueService _messageQueueService;

        public MessageQueueTest()
        {
            _messageQueueService = DependencyUtil.ResolveServiceForModel<IMessageQueueService>(JxApplication.BatchService);
        }

        [TestMethod]
        public void TestMessageQueue()
        {
            for (int i = 1; i <= 30; i++)
            {
                int seq = i;
                _messageQueueService.EnqueueUnitTestMessage(new { seq });
            }

            for (int i = 1; i <= 2; i++)
            {
                _messageQueueService.StartNewDequeueJob(TaskQueueName.UnitTest, (message) =>
                {
                    LogUtil.ForcedDebug(message);
                    Task.Delay(1000).Wait();                    
                    return true;
                });
            }

            Task.Delay(-1).Wait();
        }
    }
}