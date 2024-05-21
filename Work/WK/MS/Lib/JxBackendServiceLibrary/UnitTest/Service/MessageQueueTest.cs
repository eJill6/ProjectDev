using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.Queue;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;
using UnitTest.Base;

namespace UnitTest.Util
{
    [TestClass]
    public class MessageQueueTest : BaseTest
    {
        private readonly Lazy<IMessageQueueService> _messageQueueService;

        private readonly Lazy<IInternalMessageQueueService> _internalMessageQueueService;

        public MessageQueueTest()
        {
            _messageQueueService = DependencyUtil.ResolveService<IMessageQueueService>();
            _internalMessageQueueService = DependencyUtil.ResolveService<IInternalMessageQueueService>();
        }

        [TestMethod]
        public void TestMessageQueue()
        {
            for (int i = 1; i <= 30; i++)
            {
                int seq = i;
                _internalMessageQueueService.Value.EnqueueUnitTestMessage(new { seq });
            }

            //for (int i = 1; i <= 2; i++)
            //{
            //    _internalMessageQueueService.Value.StartNewDequeueJob(TaskQueueName.UnitTest, (message) =>
            //    {
            //        //logUtilService.ForcedDebug(message);
            //        Task.Delay(1000).Wait();
            //        return true;
            //    });
            //}

            Task.Delay(-1).Wait();
        }
    }
}