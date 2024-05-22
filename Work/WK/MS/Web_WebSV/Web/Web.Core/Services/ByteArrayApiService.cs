using System.Web;
using ControllerShareLib.Helpers.Security;
using ControllerShareLib.Services;
using JxBackendService.Common;
using JxBackendService.Common.Util;

namespace Web.Core.Services;

public class ByteArrayApiService : BaseByteArrayApiService
{
    private static readonly string s_encodingPathPropertyName = "ep";

    /// <summary>
    /// Web's javascript method: btoa() can't support Chinese
    /// so use url encode body before send request
    /// </summary>
    /// <param name="json"></param>
    /// <returns></returns>
    protected override string ProcessRequestBodyJson(string json) => HttpUtility.UrlDecode(json);

    protected override string ProcessRequestBody(string requestBody)
    {
        //TODO: Articor: axios的新版本issue：使用content-type: application-json後傳入base64String，會造成傳入的值多了雙引號
        //目前先在後端處理將前後雙引號(")移除，後續要Survey一下如何在前端將正確的值傳進來
        char[] symbols = { '"' };
        return requestBody.Trim(symbols);
    }

    public override bool CheckAllowUnencryptedRequest(HttpRequest httpRequest)
    {
        //do nothing
        return true;
    }

    protected override string GetDecodePath(HttpRequest httpRequest)
    {
        var encodePath = GetEncodePathFromQueryString(httpRequest);

        if (!encodePath.IsNullOrEmpty())
        {
            //Step1:encodePath是從queryString來，所以一定有urlEncode，先Decode一次
            //Step2:拿到Decode後的base64String，做XOR解密拿到結果
            //Step3:本來的明碼可能也會做UrlEncode(因為他是{Path}/{QueryString})，所以要再Decode一次
            return HttpUtility.UrlDecode(
                XorEncryptTool.XorDecryptToString(
                    HttpUtility.UrlDecode(encodePath)));
        }
        
        return base.GetDecodePath(httpRequest);
    }

    public override bool IsEncodingRequired(HttpRequest httpRequest)
    {
        return IsEncodingPathFromQueryString(httpRequest) || base.IsEncodingRequired(httpRequest);
    }

    public override bool IsEncodingPathFromQueryString(HttpRequest httpRequest)
    {
        return !GetEncodePathFromQueryString(httpRequest).IsNullOrEmpty();
    }

    private static string GetEncodePathFromQueryString(HttpRequest httpRequest)
    {
        if (httpRequest.Query.TryGetValue(s_encodingPathPropertyName, out var values))
        {
            return values.ToString();
        }

        return string.Empty;
    }
}