using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.Queue;
using JxBackendService.Model.MessageQueue;
using JxBackendService.Model.Param.Finance;
using JxBackendService.Model.ViewModel.ThirdParty;
using System;

namespace JxBackendService.Interface.Service
{
    public interface IMessageQueueService
    {
        void EnqueueTransferToMiseLiveMessage(TransferToMiseLiveParam param);

        void EnqueueTransferAllOutMessage(PlatformProduct platformProduct, TransferOutUserDetail transferOutUserDetail);

        void StartNewDequeueJob(TaskQueueName taskQueueName, Func<string, bool> doJobAfterReceived);

        void EnqueueUnitTestMessage<T>(T param);

        void SendTransferMessage(TransferMessage transferMessage);
    }
}