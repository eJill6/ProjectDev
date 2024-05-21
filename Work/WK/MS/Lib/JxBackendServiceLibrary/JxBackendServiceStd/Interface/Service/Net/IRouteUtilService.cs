using JxBackendService.Model.Enums.Route;

namespace JxBackendService.Interface.Service.Net
{
    public interface IRouteUtilService
    {
        string GetActionName();

        string GetControllerName();

        string GetMiseWebTokenName();

        string GetMiseWebTokenUrl(string action, string controller, object routeParams);

        string GetRouteUrl(RouteName routeName, string action);

        string GetRouteUrl(RouteName routeName, string action, string controller);

        string GetRouteUrl(RouteName routeName, string action, string controller, object routeParams);

        string GetRouteUrl(string action, string controller, object routeParams);
    }
}