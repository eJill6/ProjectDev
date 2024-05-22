using JxBackendService.DependencyInjection;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.Security;
using M.Core.Controllers.Base;
using M.Core.Interface.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace M.Core.Controllers
{
    public class PublicController : BaseApiController
    {
        private readonly Lazy<IPublicKeyService> _publicKeyService;

        public PublicController()
        {
            _publicKeyService = DependencyUtil.ResolveService<IPublicKeyService>();
        }

        /// <summary>取得公鑰，座標用,隔開</summary>
        [HttpGet]
        [AllowAnonymous]
        [ApiExplorerSettings(IgnoreApi = true)]
        public AppResponseModel<string> GetCoordinate()
        {
            Coordinate publicKey = _publicKeyService.Value.GetPublicKeyInfo();

            return new AppResponseModel<string>
            {
                Success = true,
                Data = $"{publicKey.X},{publicKey.Y}"
            };
        }
    }
}