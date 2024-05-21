using System.Web.Http;
using Web.Infrastructure.Attributes.Api;

namespace Web.App_Start
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            config.Filters.Add(new WebAuthorizeAttribute());

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}"
            );
        }
    }
}