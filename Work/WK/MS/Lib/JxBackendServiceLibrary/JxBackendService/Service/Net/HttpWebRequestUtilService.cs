﻿using Flurl;
using JxBackendService.Common.Util;
using JxBackendService.Interface.Service.Net;
using JxBackendService.Model.Attributes;
using System.Net;

namespace JxBackendService.Service.Net
{
    public class HttpWebRequestUtilService : IHttpWebRequestUtilService
    {
        public string CombineUrl(params string[] parts)
        {
            return Url.Combine(parts);
        }

        public virtual string GetResponse(WebRequestParam webRequestParam, out HttpStatusCode httpStatusCode)
        {
            return HttpWebRequestUtil.GetResponse(webRequestParam, out httpStatusCode);
        }

    }

    [MockService]
    public class HttpWebRequestUtilMockService : HttpWebRequestUtilService
    {
        public override string GetResponse(WebRequestParam webRequestParam, out HttpStatusCode httpStatusCode)
        {
            httpStatusCode = HttpStatusCode.OK;
            string content = string.Empty;

            return content;
        }

        //public string GetResponse(WebRequestParam webRequestParam, out HttpStatusCode httpStatusCode)
        //{
        //    httpStatusCode = HttpStatusCode.OK;

        //    return new ABBetLogResponseModel().ToJsonString();
        //}
    }
}