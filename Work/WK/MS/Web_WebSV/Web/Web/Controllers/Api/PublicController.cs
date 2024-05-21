using JxBackendService.Common.Util;
using SLPolyGame.Web.Interface;
using System.Web.Http;
using Web.Helpers.Security;

namespace Web.Controllers.Api
{
    public class PublicController : ApiController
    {
        protected readonly ISLPolyGameWebSVService _polyGameServiceClient = null;

        public PublicController(ISLPolyGameWebSVService polyGameServiceClient)
        {
            _polyGameServiceClient = polyGameServiceClient;
        }

        [HttpPost]
        public void ReportStatus()
        {
            string userKey = AuthenticationUtil.GetTokenModel().UserKey;

            if (userKey.IsNullOrEmpty())
            {
                return;
            }

            //_polyGameServiceClient.GetStatus();
        }
    }
}