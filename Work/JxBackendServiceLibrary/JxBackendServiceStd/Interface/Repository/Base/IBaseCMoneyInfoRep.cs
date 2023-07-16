using JxBackendService.Model.Paging;
using JxBackendService.Model.Param.TransferRecord;
using JxBackendService.Model.ViewModel.TransferRecord;
using System.Collections.Generic;

namespace JxBackendService.Interface.Repository.Base
{
    public interface IBaseCMoneyInfoRep<T> : IBaseDbRepository<T>
    {
        string CreateMoneyID();

        List<T> GetProcessingOrders3DaysAgo();

        PagedResultModel<TransferRecordViewModel> GetPlatformTransferRecord(QueryPlatformTransferRecordParam param);
    }
}