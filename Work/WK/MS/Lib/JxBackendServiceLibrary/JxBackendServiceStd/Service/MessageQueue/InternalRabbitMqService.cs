using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.Queue;
using JxBackendService.Model.MessageQueue;
using JxBackendService.Model.Param.Chat;
using JxBackendService.Model.Param.Finance;
using JxBackendService.Model.Param.SMS;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel.ThirdParty;
using System.Collections.Concurrent;

namespace JxBackendService.Service.MessageQueue
{
    public class InternalRabbitMqService : BaseRabbitMqService, IInternalMessageQueueService
    {
        private static readonly ConcurrentDictionary<string, RabbitMQClientManager> s_rabbitMQClientManagerMap;

        static InternalRabbitMqService()
        {
            var internalRabbitMqInitService = DependencyUtil.ResolveService<IInternalRabbitMqInitService>().Value;
            s_rabbitMQClientManagerMap = internalRabbitMqInitService.Init();
        }

        public InternalRabbitMqService()
        {
        }

        public BaseReturnModel EnqueueTransferToMiseLiveMessage(TransferToMiseLiveParam param)
        {
            return Enqueue(TaskQueueName.TransferToMiseLive, param);
        }

        public BaseReturnModel EnqueueTransferAllOutMessage(PlatformProduct platformProduct, TransferOutUserDetail param)
        {
            return Enqueue(TaskQueueName.TransferAllOut(platformProduct), param);
        }

        public BaseReturnModel EnqueueChatMessage(SendAddMessageQueueParam param)
        {
            return Enqueue(TaskQueueName.AddChatMessage, param);
        }

        public void EnqueueSendSMS(SendUserSMSParam param)
        {
            Enqueue(TaskQueueName.SendSMS, param);
        }

        public BaseReturnModel EnqueueUpdateTPGameUserScoreMessage(UpdateTPGameUserScoreParam param)
        {
            return Enqueue(TaskQueueName.UpdateTPGameUserScore(param.Product), param.UserID);
        }

        public BaseReturnModel EnqueueUnitTestMessage<T>(T param)
        {
            return Enqueue(TaskQueueName.UnitTest, param);
        }

        protected override ConcurrentDictionary<string, RabbitMQClientManager> GetRabbitMQClientManagerMap()
        {
            return s_rabbitMQClientManagerMap;
        }
    }
}