using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Infrastructure.Extensions
{
    public static class ActionLinkExtensions
    {
        public static MvcHtmlString ActionLink(this HtmlHelper htmlHelper, string linkText = null, object htmlAttributes = null)
        {
            return ActionLink(htmlHelper, linkText, true, htmlAttributes);
        }

        public static MvcHtmlString ActionLink(this HtmlHelper htmlHelper, string linkText, bool preventDefault, object htmlAttributes)
        {
            return new MvcHtmlString(GenerateActionLinkTagBuilder(linkText, preventDefault, htmlAttributes).ToString());
        }

        private static TagBuilder GenerateActionLinkTagBuilder(string linkText, bool preventDefault, object htmlAttributes)
        {
            var builder = new TagBuilder("a");
            builder.InnerHtml = linkText;
            if (preventDefault)
            {
                builder.Attributes["href"] = "javascript:void(0);";
            }

            if (htmlAttributes != null &&
                HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes) is IDictionary<string, object> attributes)
            {
                var mergeAttributes = attributes.Where(attr => attr.Value != null)
                    .ToDictionary(attr => attr.Key, attr => attr.Value);
                builder.MergeAttributes(mergeAttributes, replaceExisting: true);
            }

            return builder;
        }
    }
}