using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using SLPolyGame.Web.Filters;
using System.Web.Http;

namespace SLPolyGame.Web.Controllers.Base
{
    /// <summary>底層Controller</summary>
    [ApiExceptionFilter]
    public class BaseApiController : ApiController
    {
        /// <summary>ctor</summary>
        public BaseApiController()
        {
        }

        private static EnvironmentUser _environmentUser = new EnvironmentUser()
        {
            Application = JxApplication.FrontSideWeb,
            LoginUser = new BasicUserInfo()
            {
                UserId = 0
            }
        };

        /// <summary>
        /// 登入用戶
        /// </summary>
        protected static EnvironmentUser EnvLoginUser { get => _environmentUser; set => _environmentUser = value; }

        protected EnvironmentUser CreateEnvironmentUser(int userId)
        {
            return new EnvironmentUser()
            {
                Application = JxApplication.FrontSideWeb,
                LoginUser = new BasicUserInfo()
                {
                    UserId = userId
                }
            };
        }

        protected EnvironmentUser EnvironmentUserOfRequest
        {
            get
            {
                return Request.Properties[nameof(EnvironmentUserOfRequest)] as EnvironmentUser;
            }
            set
            {
                Request.Properties[nameof(EnvironmentUserOfRequest)] = value;
            }
        }
    }
}