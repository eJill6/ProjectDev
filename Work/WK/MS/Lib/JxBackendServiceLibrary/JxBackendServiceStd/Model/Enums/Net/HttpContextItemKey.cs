using Microsoft.AspNetCore.Http;

namespace JxBackendService.Model.Enums.Net
{
    public class HttpContextItemKey : BaseStringValueModel<HttpContextItemKey>
    {
        private HttpContextItemKey()
        { }

        public static readonly HttpContextItemKey BackSideWebUser = new HttpContextItemKey()
        {
            Value = "BackSideWebUser"
        };

        public static readonly HttpContextItemKey EnvironmentUser = new HttpContextItemKey()
        {
            Value = "EnvironmentUser"
        };        

        public static readonly HttpContextItemKey UserInfoToken = new HttpContextItemKey()
        {
            Value = "UserInfoToken"
        };

        public static readonly HttpContextItemKey ApiLogRequestItem = new HttpContextItemKey()
        {
            Value = "ApiLogRequestInfo"
        };        
    }

    public static class HttpContextExtensions
    {
        public static T GetItemValue<T>(this HttpContext httpContext, HttpContextItemKey key) where T : class
        {
            if (httpContext.Items.ContainsKey(key.Value))
            {
                return httpContext.Items[key.Value] as T;
            }

            return default(T);
        }

        public static void SetItemValue<T>(this HttpContext httpContext, HttpContextItemKey key, T value) where T : class
        {
            httpContext.Items[key.Value] = value;
        }
    }
}