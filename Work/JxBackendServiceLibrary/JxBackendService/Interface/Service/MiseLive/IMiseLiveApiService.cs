using JxBackendService.Interface.Model.MiseLive.Request;
using JxBackendService.Model.MiseLive.Response;

namespace JxBackendService.Interface.Service.MiseLive
{
    public interface IMiseLiveApiService
    {
        MiseLiveResponse<MiseLiveTransferOrder> GetTransferOrderResult(IMiseLiveTransferOrderRequestParam requestParam);

        MiseLiveResponse<MiseLiveBalance> GetUserBalance(IMiseLiveUserBalanceRequestParam request);

        MiseLiveResponse<MiseLiveBalance> TransferIn(IMiseLiveTransferRequestParam requestParam);

        MiseLiveResponse<MiseLiveBalance> TransferOut(IMiseLiveTransferRequestParam requestParam);
    }
}