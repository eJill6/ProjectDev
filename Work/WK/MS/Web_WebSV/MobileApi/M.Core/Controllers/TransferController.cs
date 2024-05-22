using JxBackendService.DependencyInjection;
using JxBackendService.Model.ReturnModel;
using M.Core.Controllers.Base;
using M.Core.Interface.Services.WebSV;
using Microsoft.AspNetCore.Mvc;

namespace M.Core.Controllers
{
    public class TransferController : BaseAuthApiController
    {
        private readonly Lazy<ITransferService> _transferService;

        public TransferController()
        {
            _transferService = DependencyUtil.ResolveService<ITransferService>();
        }

        /// <summary>轉回所有第三方遊戲餘額</summary>
        [HttpGet]
        public AppResponseModel TransferOutAllTPGameBalance()
        {
            return _transferService.Value.TransferOutAllTPGameBalance(EnvLoginUser.LoginUser.UserId);
        }

        /// <summary>轉回上一次進入的第三方遊戲餘額</summary>
        [HttpGet]
        public AppResponseModel TransferOutLastTPGameBalance()
        {
            return _transferService.Value.TransferOutLastTPGameBalance(EnvLoginUser.LoginUser.UserId);
        }
    }
}