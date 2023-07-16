using JxBackendService.Common.Util;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace JxBackendService.Interface.Service.Net
{
    public interface IHttpWebRequestUtilService
    {
        string GetResponse(WebRequestParam webRequestParam, out HttpStatusCode httpStatusCode);

        string CombineUrl(params string[] parts);

        Task<ResponseInfo> GetResponseAsync(WebRequestParam webRequestParam);
    }
}