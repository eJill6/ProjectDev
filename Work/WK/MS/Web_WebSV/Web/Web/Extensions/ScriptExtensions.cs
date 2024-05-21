using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.Net;
using JxBackendService.Model.Enums;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Web.Extensions
{
    public static class ScriptExtensions
    {
        public static MvcHtmlString ImportJavaScripts(this HtmlHelper htmlHelper, params string[] scriptPaths)
        {
            for (int i = 0; i < scriptPaths.Length; i++)
            {
                string scriptPath = scriptPaths[i];

                if (scriptPath.IndexOf("v=") >= 0)
                {
                    continue;
                }

                if (scriptPath.IndexOf("?") < 0)
                {
                    scriptPath += "?";
                }
                else
                {
                    scriptPath += "&";
                }

                scriptPath += "v=" + GlobalVariables.StaticContentVersion;
                scriptPaths[i] = scriptPath;
            }

            string scriptsResult = string.Join("\n", scriptPaths.Select(path => GenerateScriptTag(path).ToString()));

            return new MvcHtmlString(scriptsResult);
        }

        private static TagBuilder GenerateScriptTag(string scriptPath)
        {
            TagBuilder builder = new TagBuilder("script");
            builder.Attributes.Add("type", "text/javascript");
            builder.Attributes.Add("src", scriptPath);

            return builder;
        }

        public static MvcHtmlString ImportCss(this HtmlHelper htmlHelper, params string[] cssPaths)
        {
            for (int i = 0; i < cssPaths.Length; i++)
            {
                string cssPath = cssPaths[i];

                if (cssPath.IndexOf("v=") >= 0)
                {
                    continue;
                }

                if (cssPath.IndexOf("?") < 0)
                {
                    cssPath += "?";
                }
                else
                {
                    cssPath += "&";
                }

                cssPath += "v=" + GlobalVariables.StaticContentVersion;
                cssPaths[i] = cssPath;
            }

            string cssItems = string.Join("\n", cssPaths.Select(path => GenerateCssLinkTag(path).ToString()));

            return new MvcHtmlString(cssItems);
        }

        public static string GetMiseWebTokenUrl(this UrlHelper urlHelper, string action, string controller, object routeParams = null)
        {
            return DependencyUtil.ResolveService<IRouteUtilService>().GetMiseWebTokenUrl(action, controller, routeParams);
        }

        private static TagBuilder GenerateCssLinkTag(string cssPath)
        {
            var builder = new TagBuilder("link");
            builder.Attributes.Add("type", "text/css");
            builder.Attributes.Add("rel", "stylesheet");
            builder.Attributes.Add("href", cssPath);

            return builder;
        }
    }
}