using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace MS.Core.Extensions
{
    public static class HtmlExtensions
    {
        public static string StripHtmlTags(this string htmlString)
        {
            // 创建 HtmlDocument 对象并加载 HTML 字符串
            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(htmlString);

            // 提取纯文本内容
            string plainText = htmlDocument.DocumentNode.InnerText;

            plainText = Regex.Replace(plainText, "<.*?>", string.Empty);

            // 移除 &nbsp; 实体
            plainText = Regex.Replace(plainText, "&nbsp;", " ");

            return plainText;
        }
    }
}
