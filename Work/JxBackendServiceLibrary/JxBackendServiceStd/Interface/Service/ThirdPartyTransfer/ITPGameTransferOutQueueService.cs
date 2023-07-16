using JxBackendService.Model.Enums;

namespace JxBackendService.Interface.Service.ThirdPartyTransfer
{
    public interface ITPGameTransferOutQueueService
    {
        void StartDequeueTransferAllOutJob(PlatformProduct product);
    }
}