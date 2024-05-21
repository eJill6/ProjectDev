using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.Config;

namespace BackSideWeb.Helpers
{
    public class MMApiHelper
    {
        public static string ApiUrl
        {
            get
            {
                if (_configUtilService == null)
                {
                    _configUtilService = DependencyUtil.ResolveService<IConfigUtilService>().Value;
                }
                return _configUtilService.Get("MMServiceUrl");
            }
        }

        private static IConfigUtilService _configUtilService;
    }
}