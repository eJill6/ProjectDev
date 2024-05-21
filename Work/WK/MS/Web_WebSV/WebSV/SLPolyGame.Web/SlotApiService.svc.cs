using JxBackendService.Common.Util;
using JxBackendService.Interface.Service;
using JxBackendService.Model.ViewModel;
using JxBackendService.Service.ThirdPartyTransfer.Base;
using JxBackendServiceNF.Service.ThirdPartyTransfer.Base;
using SLPolyGame.Web.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;

namespace SLPolyGame.Web
{
    // 注意: 您可以使用 [重構] 功能表上的 [重新命名] 命令同時變更程式碼、svc 和組態檔中的類別名稱 "IMOneSlotApiService"。
    // 注意: 若要啟動 WCF 測試用戶端以便測試此服務，請在 [方案總管] 中選取 IMOneSlotApiService.svc 或 IMOneSlotApiService.svc.cs，然後開始偵錯。
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    public class SlotApiService : BaseSlotApiWCFService, ISlotApiService
    {
        private EnvironmentUser _envLoginUser;

        public SlotApiService()
        { }

        #region override

        public override EnvironmentUser EnvLoginUser
        {
            get
            {
                _envLoginUser = AssignValueOnceUtil.GetAssignValueOnce(_envLoginUser, () => new BaseFrontSideWebService().EnvLoginUser);
                return _envLoginUser;
            }
        }

        #endregion override
    }
}