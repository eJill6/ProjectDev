using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Infrastructure.Extensions
{
    public static class HtmlExtensions
    {
        /// <summary>
        /// 分頁
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="setting">設定值</param>
        /// <param name="htmlAttributes">html attributes</param>
        /// <returns></returns>
        public static MvcHtmlString Pagination(this HtmlHelper htmlHelper, PaginationSetting setting, object htmlAttributes = null)
        {
            var div = new TagBuilder("div");
            div.InnerHtml = htmlHelper.Partial("_Pagination", setting).ToHtmlString();
            div.Attributes["id"] = "pagination";
            if (htmlAttributes != null)
            {
                div.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
            }

            return new MvcHtmlString(div.ToString());
        }
    }
}