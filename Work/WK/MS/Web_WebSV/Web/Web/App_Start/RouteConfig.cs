using JxBackendService.Model.Enums.Route;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            foreach (RouteName routeName in RouteName.GetAll())
            {
                routes.MapRoute(
                    routeName.Value,
                    routeName.Url,
                    routeName.Defaults);
            }

            //routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}/{id}",
            //    defaults: new { controller = "Account", action = "LogOn", id = UrlParameter.Optional }
            //);
        }
    }
}