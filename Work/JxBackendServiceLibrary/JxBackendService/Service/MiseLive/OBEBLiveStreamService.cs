using JxBackendService.Common.Util;
using JxBackendService.Common.Util.Cache;
using JxBackendService.Interface.Model.MiseLive.ViewModel;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.MiseLive;
using JxBackendService.Interface.Service.ThirdPartyTransfer.OBEB;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ThirdParty.OB.OBEB;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.ThirdParty.MiseLive;
using JxBackendService.Service.MiseLive.Base;
using System.Collections.Generic;
using System.Linq;

namespace JxBackendService.Service.MiseLive
{
    public class OBEBLiveStreamService : BaseLiveStreamService, ITPLiveStreamService
    {
        private readonly ITPGameOBEBApiService _tpGameOBEBApiService;

        protected override PlatformProduct Product => PlatformProduct.OBEB;

        public OBEBLiveStreamService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _tpGameOBEBApiService = ResolveJxBackendService<ITPGameOBEBApiService>();
        }

        protected override List<IMiseLiveAnchor> GetRemoteAnchors()
        {
            OBEBBaseResponseWtihDataModel<List<OBEBAnchor>> anchorsResult = _tpGameOBEBApiService.GetAnchorsResult();

            if (!anchorsResult.IsSuccess)
            {
                return null;
            }

            //轉換為平台自己定義的model, 這裡是第一家串, 所以欄位是一樣的, 可以直接cast過去
            return anchorsResult.data.Select(s => s.CastByJson<MiseLiveAnchor>() as IMiseLiveAnchor).Where(a => a.LiveStatus == 1).ToList();
        }
    }
}