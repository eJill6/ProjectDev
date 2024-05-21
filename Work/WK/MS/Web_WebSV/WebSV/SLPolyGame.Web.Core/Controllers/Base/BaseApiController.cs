using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.Config;
using JxBackendService.Interface.Service.Util;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.Net;
using JxBackendService.Model.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace SLPolyGame.Web.Core.Controllers.Base
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class BaseApiController : ControllerBase
    {
        private readonly Lazy<ILogUtilService> _logUtilService;

        private readonly Lazy<IConfigUtilService> _configUtilService;

        private static readonly Lazy<IEnvironmentService> s_environmentService = DependencyUtil.ResolveService<IEnvironmentService>();

        protected static PlatformMerchant Merchant => SharedAppSettings.PlatformMerchant;

        protected ILogUtilService LogUtilService => _logUtilService.Value;

        protected IConfigUtilService ConfigUtilService => _configUtilService.Value;

        public BaseApiController()
        {
            _logUtilService = DependencyUtil.ResolveService<ILogUtilService>();
            _configUtilService = DependencyUtil.ResolveService<IConfigUtilService>();
        }

        private readonly EnvironmentUser _environmentUser = new EnvironmentUser()
        {
            Application = s_environmentService.Value.Application,
            LoginUser = new BasicUserInfo()
            {
                UserId = 0
            }
        };

        /// <summary>
        /// 登入用戶
        /// </summary>
        protected virtual EnvironmentUser EnvLoginUser => _environmentUser;

        protected EnvironmentUser CreateEnvironmentUser(int userId)
        {
            var currentUser = new EnvironmentUser()
            {
                Application = s_environmentService.Value.Application,
                LoginUser = new BasicUserInfo()
                {
                    UserId = userId
                }
            };

            HttpContext.SetItemValue(HttpContextItemKey.EnvironmentUser, currentUser);

            return currentUser;
        }

        protected EnvironmentUser EnvironmentUserOfHttpContext
        {
            get
            {
                return HttpContext.GetItemValue<EnvironmentUser>(HttpContextItemKey.EnvironmentUser);
            }
            set
            {
                HttpContext.SetItemValue(HttpContextItemKey.EnvironmentUser, value);
            }
        }

        protected Lazy<T> ResolveJxBackendService<T>(DbConnectionTypes dbConnectionType)
        {
            return DependencyUtil.ResolveJxBackendService<T>(EnvLoginUser, dbConnectionType);
        }

        protected Lazy<object> ResolveJxBackendService(Type type, DbConnectionTypes dbConnectionType)
        {
            return DependencyUtil.ResolveJxBackendService(type, EnvLoginUser, dbConnectionType);
        }

        protected Lazy<T> ResolveJxBackendService<T>(PlatformMerchant merchant, DbConnectionTypes dbConnectionType)
        {
            return DependencyUtil.ResolveJxBackendService<T>(merchant, EnvLoginUser, dbConnectionType);
        }

        protected Lazy<T> ResolveJxBackendService<T>(PlatformProduct product, DbConnectionTypes dbConnectionType)
        {
            return DependencyUtil.ResolveJxBackendService<T>(product, SharedAppSettings.PlatformMerchant, EnvLoginUser, dbConnectionType);
        }

        //protected T ResolveJxBackendService<T>(PlatformProduct product, bool isMockService, DbConnectionTypes dbConnectionType)
        //{
        //    return DependencyUtil.ResolveJxBackendService<T>(product, isMockService, EnvLoginUser, dbConnectionType);
        //}

        protected Lazy<T> ResolveJxBackendService<T>(JxApplication keyModel, DbConnectionTypes dbConnectionType)
            => DependencyUtil.ResolveJxBackendService<T>(keyModel, SharedAppSettings.PlatformMerchant, EnvLoginUser, dbConnectionType);

        protected static Lazy<T> ResolveKeyed<T>(JxApplication keyModel)
        {
            return DependencyUtil.ResolveKeyed<T>(keyModel, Merchant);
        }

        protected static Lazy<T> ResolveKeyed<T>(JxApplication keyModel, PlatformMerchant platformMerchant)
        {
            return DependencyUtil.ResolveKeyed<T>(keyModel, platformMerchant);
        }

        //protected static Lazy<T> ResolveKeyedForModel<T>(JxApplication keyCtorModel)
        //{
        //    return DependencyUtil.ResolveKeyedForModel<T>(keyCtorModel, Merchant);
        //}
    }
}