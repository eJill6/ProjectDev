using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.Queue;
using JxBackendService.Model.MessageQueue;
using JxBackendService.Model.Param.Chat;
using JxBackendService.Model.Param.Finance;
using JxBackendService.Model.Param.SMS;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxMsgEntities;
using System;
using System.Collections.Generic;

namespace JxBackendService.Interface.Service
{
    public interface IBaseMessageQueueService
    {
        ICollection<string> GetClientProvidedNames();

        void StartNewDequeueJob(TaskQueueName taskQueueName, Func<DoDequeueJobAfterReceivedParam, bool> doJobAfterReceived);

        void StartNewDequeueJob(string queryClientProvidedName, TaskQueueName taskQueueName, Func<DoDequeueJobAfterReceivedParam, bool> doJobAfterReceived);
    }

    public interface IMessageQueueService : IBaseMessageQueueService
    {
        SendResult SendBackSideWebTransferMessage(string routingKey, TransferMessage transferMessage);

        SendResult SendBackSideWebUserLogoutMessage(BWUserLogoutMessage bwUserLogoutMessage);

        SendResult SendBackSideWebUserChangePasswordMessage(BWUserChangePasswordMessage bwUserLogoutMessage);

        SendResult SendChatNotification(ChatNotificationParam newMessageNotificationParam);
    }

    public interface IInternalMessageQueueService : IBaseMessageQueueService
    {
        BaseReturnModel EnqueueTransferToMiseLiveMessage(TransferToMiseLiveParam param);

        BaseReturnModel EnqueueTransferAllOutMessage(PlatformProduct platformProduct, TransferOutUserDetail transferOutUserDetail);

        BaseReturnModel EnqueueUnitTestMessage<T>(T param);

        BaseReturnModel EnqueueChatMessage(SendAddMessageQueueParam param);

        void EnqueueSendSMS(SendUserSMSParam param);

        BaseReturnModel EnqueueUpdateTPGameUserScoreMessage(UpdateTPGameUserScoreParam param);
    }
}