using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using SLPolyGame.Web.Models;
using System.Net.Http;
using System.Web.Http.Filters;

namespace SLPolyGame.Web.Filters.Base
{
    /// <summary>
    /// BaseApiFilterAttribute
    /// </summary>
    public class BaseApiFilterAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// 配合當參數用
        /// </summary>
        private readonly EnvironmentUser _environmentUser = new EnvironmentUser()
        {
            Application = JxApplication.FrontSideWeb,
            LoginUser = new BasicUserInfo()
        };

        /// <summary>EnvironmentUser</summary>
        protected EnvironmentUser EnvironmentUser => _environmentUser;
    }
}