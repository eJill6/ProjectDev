using SLPolyGame.Web.Model;
using System.ServiceModel;
using System.ServiceModel.Activation;
using WebApiImpl;

namespace SLPolyGame.Web
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“SLPolyGameService”。
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    public class PublicApiService : BasePublicApiService, IPublicApiService
    {
    }
}