using JxBackendService.Interface.Service;

namespace JxBackendService.Service.Setting
{
    /// <summary>
    /// BatchService設定檔
    /// </summary>
    public class BatchServiceAppSettingService : NewTransferServiceAppSettingService, IBatchServiceAppSettingService
    {
        public override int? MinWorkerThreads => 100;
    }
}