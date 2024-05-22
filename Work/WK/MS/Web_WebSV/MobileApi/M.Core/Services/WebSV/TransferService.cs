using ControllerShareLib.Services.WebSV.Base;
using JxBackendService.Common.Util;
using JxBackendService.Model.MiseLive.Response;
using JxBackendService.Model.ReturnModel;
using M.Core.Interface.Services.WebSV;
using System.Web;

namespace M.Core.Services.WebSV
{
    public class TransferService : BaseWebSVService, ITransferService
    {
        protected override string RemoteControllerName => "Transfer";

        public AppResponseModel TransferOutAllTPGameBalance(int userId)
        {
            BaseMiseLiveResponse result = TransferOutAllTPGameBalanceAsync(userId).GetAwaiterAndResult();                

            return new AppResponseModel
            {
                Success = result.Success,
                Message = result.Error
            };
        }

        private async Task<BaseMiseLiveResponse> TransferOutAllTPGameBalanceAsync(int userId)
        {
            return await Task.FromResult(GetHttpGetResponse<BaseMiseLiveResponse>(nameof(TransferOutAllTPGameBalance), $"{nameof(userId)}={HttpUtility.UrlEncode(userId.ToString())}"));
        }

        public AppResponseModel TransferOutLastTPGameBalance(int userId)
        {
            BaseMiseLiveResponse result = TransferOutLastTPGameBalanceAsync(userId)
                .ConfigureAwait(false).GetAwaiter().GetResult();

            return new AppResponseModel
            {
                Success = result.Success,
                Message = result.Error
            };
        }

        private async Task<BaseMiseLiveResponse> TransferOutLastTPGameBalanceAsync(int userId)
        {
            return await Task.FromResult(GetHttpGetResponse<BaseMiseLiveResponse>(nameof(TransferOutLastTPGameBalance), $"{nameof(userId)}={HttpUtility.UrlEncode(userId.ToString())}"));
        }
    }
}