using System.Web.Mvc;

namespace JxBackendService.Model.Enums.Route
{
    public class RouteName : BaseStringValueModel<RouteName>
    {
        public string Url { get; private set; }

        public object Defaults { get; private set; }

        private RouteName() { }

        public static readonly RouteName Test = new RouteName()
        {
            Value = "Test",
            Url = "Test",
            Defaults = new { controller = "Account", action = "Test" }
        };

        public static readonly RouteName LoginGuide = new RouteName()
        {
            Value = "LoginGuide",
            Url = "LoginGuide",
            Defaults = new { controller = "Account", action = "LogOnGuide" }
        };

        public static readonly RouteName LoginValidate = new RouteName()
        {
            Value = "LoginValidate",
            Url = "LoginValidate",
            Defaults = new { controller = "Account", action = "LogOnValidate" }
        };

        public static readonly RouteName Login = new RouteName()
        {
            Value = "Login",
            Url = "Login",
            Defaults = new { controller = "Account", action = "LogOn" }
        };

        public static readonly RouteName MobileLogin = new RouteName()
        {
            Value = "MobileLogin",
            Url = "MobileLogin",
            Defaults = new { controller = "Account", action = "MobileLogOn" }
        };

        public static readonly RouteName Maintain = new RouteName()
        {
            Value = "Maintain",
            Url = "Maintain",
            Defaults = new { controller = "Account", action = "Maintain" }
        };

        public static readonly RouteName LogOff = new RouteName()
        {
            Value = "LogOff",
            Url = "LogOff",
            Defaults = new { controller = "Account", action = "LogOff" }
        };

        public static readonly RouteName Forbidden = new RouteName()
        {
            Value = "Forbidden",
            Url = "Forbidden",
            Defaults = new { controller = "Account", action = "Forbidden" }
        };

        public static readonly RouteName Realtime = new RouteName()
        {
            Value = "Realtime",
            Url = "Realtime",
            Defaults = new { controller = "Chongqing", action = "Index" }
        };

        public static readonly RouteName SelectFive = new RouteName()
        {
            Value = "SelectFive",
            Url = "SelectFive",
            Defaults = new { controller = "GDSelectFive", action = "Index" }
        };

        public static readonly RouteName FFC = new RouteName()
        {
            Value = "FFC",
            Url = "FFC",
            Defaults = new { controller = "Heji", action = "Index" }
        };

        public static readonly RouteName Bulletins = new RouteName()
        {
            Value = "Bulletins",
            Url = "Bulletins",
            Defaults = new { controller = "Home", action = "Bulletins" }
        };

        public static readonly RouteName Sales = new RouteName()
        {
            Value = "Sales",
            Url = "Sales",
            Defaults = new { controller = "Home", action = "Sales" }
        };

        /// <summary>兼容以前老的注册地址</summary>
        public static readonly RouteName Register = new RouteName()
        {
            Value = "Register",
            Url = "Reg/Register.aspx",
            Defaults = new { controller = "Account", action = "Register" }
        };

        public static readonly RouteName HjHistory = new RouteName()
        {
            Value = "HjHistory",
            Url = "Heji/History",
            Defaults = new { controller = "Heji", action = "Trend" }
        };

        public static readonly RouteName HjSelectFiveHistory = new RouteName()
        {
            Value = "HjSelectFiveHistory",
            Url = "HjSelectFive/History",
            Defaults = new { controller = "HjSelectFive", action = "Trend" }
        };

        public static readonly RouteName CheckHealth = new RouteName()
        {
            Value = "CheckHealth",
            Url = "CheckHealth",
            Defaults = new { controller = "Home", action = "CheckHealth" }
        };

        public static readonly RouteName GameCenterOther = new RouteName()
        {
            Value = "GameCenterOther",
            Url = "GameCenter/T/{action}",
            Defaults = new { controller = "GameCenter", action = "Index" }
        };

        public static readonly RouteName GameCenter = new RouteName()
        {
            Value = "GameCenter",
            Url = "GameCenter/{productCode}",
            Defaults = new { controller = "GameCenter", action = "Index" }
        };

        public static readonly RouteName Resource = new RouteName()
        {
            Value = "Resource",
            Url = "Resource",
            Defaults = new { controller = "Resource", action = "Index" }
        };

        public static readonly RouteName ResourceOther = new RouteName()
        {
            Value = "ResourceOther",
            Url = "Resource/{directory}/{resourceName}",
            Defaults = new { controller = "Resource", action = "Index" }
        };

        public static readonly RouteName Default = new RouteName()
        {
            Value = "Default",
            Url = "{controller}/{action}/{id}/{id2}",
            Defaults = new { controller = "Home", action = "Index", id = UrlParameter.Optional, id2 = UrlParameter.Optional }
        };
    }
}
