using JxBackendService.Common.Util;
using JxBackendService.Interface.Service;
using JxBackendService.Model.ViewModel;
using JxBackendService.Service.ThirdPartyTransfer.Base;
using JxBackendServiceNF.Service.ThirdPartyTransfer.Base;
using PolyDataBase.Helpers;
using SLPolyGame.Web.BLL;
using System.ServiceModel;
using System.ServiceModel.Activation;
using WebApiImpl;

namespace SLPolyGame.Web
{
    // 注意: 您可以使用 [重構] 功能表上的 [重新命名] 命令同時變更程式碼、svc 和組態檔中的類別名稱 "ThirdPartyApiService"。
    // 注意: 若要啟動 WCF 測試用戶端以便測試此服務，請在 [方案總管] 中選取 ThirdPartyApiService.svc 或 ThirdPartyApiService.svc.cs，然後開始偵錯。
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    public class ThirdPartyApiService : BaseThirdPartyApiService, IThirdPartyApiWCFService
    {
        private EnvironmentUser _envLoginUser;

        public ThirdPartyApiService()
        { }

        /// <summary>获取缓存用户信息</summary>
        protected override Model.UserInfoToken GetUserInfoToken()
        {
            return MessageContextHelper.GetUserInfoToken();
        }
    }
}