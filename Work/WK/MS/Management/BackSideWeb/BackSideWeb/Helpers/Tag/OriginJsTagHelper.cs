using JxBackendService.DependencyInjection;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using Microsoft.AspNetCore.Mvc.Razor.Infrastructure;
using Microsoft.AspNetCore.Mvc.Razor.TagHelpers;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Data.Entity.Infrastructure;
using System.Text.Encodings.Web;

namespace BackSideWeb.Helpers.Tag
{
    [HtmlTargetElement("script", Attributes = "src")]
    public class OriginJsTagHelper : UrlResolutionTagHelper
    {
        private static readonly HashSet<string> s_ignoreSrcs = new HashSet<string>()
        {
            "~/lib/jquery/jquery-3.6.0.min.js",
            "~/lib/jquery/jquery-ui.min.js",
            "~/lib/jquery/jquery.tmpl.min.js",
        };

        public OriginJsTagHelper(IUrlHelperFactory urlHelperFactory, HtmlEncoder htmlEncoder) : base(urlHelperFactory, htmlEncoder)
        {
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (SharedAppSettings.GetEnvironmentCode() == EnvironmentCode.Development)
            {
                string src = context.AllAttributes.Where(w => w.Name == "src").Single().Value.ToString();

                if (s_ignoreSrcs.Contains(src))
                {
                    return;
                }

                if (!string.IsNullOrEmpty(src) && src.EndsWith(".min.js"))
                {
                    string jsPath = src.Substring(0, src.Length - 7) + ".js";
                    output.Attributes.SetAttribute("src", jsPath);
                    base.Process(context, output);
                }
            }
        }
    }
}