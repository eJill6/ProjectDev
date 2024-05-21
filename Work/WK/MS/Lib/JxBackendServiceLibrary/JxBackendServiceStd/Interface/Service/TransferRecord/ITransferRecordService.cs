using JxBackendService.Model.Common;
using JxBackendService.Model.Paging;
using JxBackendService.Model.Param.TransferRecord;
using JxBackendService.Model.ViewModel.TransferRecord;
using System.Collections.Generic;

namespace JxBackendService.Interface.Service.TransferRecord
{
    public interface ITransferRecordService
    {
        PagedResultModel<TransferRecordViewModel> GetTransferRecord(SearchTransferRecordParam param);

        List<JxBackendSelectListItem> GetProductSelectListItems();
    }
}