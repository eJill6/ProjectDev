using JxBackendService.Model.ReturnModel;

namespace M.Core.Interface.Services.WebSV
{
    public interface ITransferService
    {
        AppResponseModel TransferOutAllTPGameBalance(int userId);

        AppResponseModel TransferOutLastTPGameBalance(int userId);
    }
}