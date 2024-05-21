using AutoMapper;
using HtmlAgilityPack;
using JxBackendService.Model.Paging;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace BackSideWeb.Helpers
{
    public class MMHelpers
    {
        public static string StripHtmlTags(string htmlString)
        {
            // 创建 HtmlDocument 对象并加载 HTML 字符串
            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(htmlString);

            // 提取纯文本内容
            string plainText = htmlDocument.DocumentNode.InnerText;

            return plainText;
        }
        public static string HtmlToString(string html)
        {
            if (string.IsNullOrEmpty(html))
            {
                return string.Empty;
            }

            // 删除HTML标签
            string plainText = Regex.Replace(html, "<.*?>", string.Empty);

            // 移除 &nbsp; 实体
            plainText = Regex.Replace(plainText, "&nbsp;", " ");

            return plainText;
        }
        public static TDestination MapModel<TSource, TDestination>(TSource sourceModel)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TSource, TDestination>();
            });

            var mapper = config.CreateMapper();
            TDestination destinationModel = mapper.Map<TSource, TDestination>(sourceModel);

            return destinationModel;
        }
        public static List<TDestination> MapModelList<TSource, TDestination>(List<TSource> sourceList)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TSource, TDestination>();
            });

            var mapper = config.CreateMapper();
            List<TDestination> destinationList = mapper.Map<List<TSource>, List<TDestination>>(sourceList);

            return destinationList;
        }
        public static PagedResultModel<TDestination> MapPagedResultModel<TSource, TDestination>(PagedResultModel<TSource> sourceModel)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap(typeof(PagedResultModel<>), typeof(PagedResultModel<>));
                cfg.CreateMap<TSource, TDestination>();
            });

            var mapper = config.CreateMapper();
            PagedResultModel<TDestination> destinationModel = mapper.Map<PagedResultModel<TSource>, PagedResultModel<TDestination>>(sourceModel);

            return destinationModel;
        }
    }
}
