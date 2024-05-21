using JxBackendService.Common.Util.Route;
using System.Web.Mvc;

namespace JxBackendService.Model.Enums.Route
{
    public class RouteName : BaseStringValueModel<RouteName>
    {
        public string Url { get; private set; }

        public object Defaults { get; private set; }

        private RouteName()
        { }

        public static readonly RouteName Error = new RouteName()
        {
            Value = nameof(Error),
            Url = "Error",
            Defaults = new { controller = "Public", action = "Error" }
        };

        public static readonly RouteName ReconnectTips = new RouteName()
        {
            Value = nameof(ReconnectTips),
            Url = "ReconnectTips",
            Defaults = new { controller = "Public", action = "ReconnectTips" }
        };

        public static readonly RouteName MiseWebToken = new RouteName()
        {
            Value = nameof(MiseWebToken),
            Url = $"mwt/{"{" + RouteUtil.RouteMiseWebTokenName + "}"}/{"{" + RouteUtil.RouteControllerName + "}"}/{"{" + RouteUtil.RouteActionName + "}"}",
        };

        public static readonly RouteName Default = new RouteName()
        {
            Value = nameof(Default),
            Url = "{controller}/{action}/{id}",
            Defaults = new { controller = "Account", action = "LogOn", id = UrlParameter.Optional }
        };
    }
}