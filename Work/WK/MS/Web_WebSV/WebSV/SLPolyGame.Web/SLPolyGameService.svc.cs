using PolyDataBase.Helpers;
using System.ServiceModel;
using System.ServiceModel.Activation;
using WebApiImpl;

namespace SLPolyGame.Web
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“SLPolyGameService”。
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    public class SLPolyGameService : BaseSLPolyGameService, ISLPolyGameService
    {
        public SLPolyGameService()
        {
        }

        /// <summary>获取缓存用户信息</summary>
        protected override Model.UserInfoToken GetUserInfoToken()
        {
            return MessageContextHelper.GetUserInfoToken();
        }
    }
}