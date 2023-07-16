using JxBackendService.Common.Util;

namespace JxBackendService.Model.ViewModel.ThirdParty
{
    public class RequestAndResponse
    {
        public string RequestBody { get; set; }

        public string ResponseContent { get; set; }
    }

    public class DetailRequestAndResponse : RequestAndResponse
    {
        public DetailRequestAndResponse()
        {
        }

        public DetailRequestAndResponse(WebRequestParam webRequestParam, string apiResult)
        {
            RequestUrl = webRequestParam.Url;
            RequestHeader = webRequestParam.Headers.ToJsonString();
            RequestBody = webRequestParam.Body;
            ResponseContent = apiResult;
        }

        public string RequestUrl { get; set; }

        public string RequestHeader { get; set; }
    }
}