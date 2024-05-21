using JxBackendService.Model.Paging;
using JxBackendService.Model.Param.ThirdParty;
using JxBackendService.Model.Param.TransferRecord;
using JxBackendService.Model.ViewModel.TransferRecord;
using System.Collections.Generic;

namespace JxBackendService.Interface.Repository.TransferRecord
{
    public interface IAllTPTransferRecordRep
    {
        PagedResultModel<TransferRecordViewModel> GetAllTPTransferRecord(
            QueryTPTransferRecordParam param, List<SearchTransferType> searchTransferTypes, string platformName);
    }
}