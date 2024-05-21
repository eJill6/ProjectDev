using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.Web;
using JxBackendService.Interface.Service.Web.BackSideWeb;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Param.User;
using JxBackendService.Model.ViewModel;
using Microsoft.AspNetCore.Mvc.Razor.TagHelpers;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text.Encodings.Web;

namespace BackSideWeb.Helpers.Tag
{
    [HtmlTargetElement("img", Attributes = "src")]
    /// <summary>只有後台才用這樣的方式render image內容，否則會影響後端效能，以及圖片下載的流量會全部回到主站台</summary>
    public class ImgTagHelper : UrlResolutionTagHelper
    {
        private readonly Lazy<IDecryptoService> _decryptoService;

        public ImgTagHelper(IUrlHelperFactory urlHelperFactory, HtmlEncoder htmlEncoder) : base(urlHelperFactory, htmlEncoder)
        {
            _decryptoService = DependencyUtil.ResolveService<IDecryptoService>();
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            string? src = context.AllAttributes.Where(w => w.Name == "src").SingleOrDefault()?.Value.ToString();

            if (!string.IsNullOrEmpty(src) && src.EndsWith(".aes", StringComparison.OrdinalIgnoreCase))
            {
                string newSrc = string.Empty;

                var backSideWebUserService = DependencyUtil.ResolveService<IBackSideWebUserService>().Value;
                BackSideWebUser backSideWebUser = backSideWebUserService.GetUser();

                ErrorMsgUtil.DoWorkWithErrorHandle(
                    new EnvironmentUser()
                    {
                        Application = JxApplication.BackSideWeb,
                        LoginUser = backSideWebUser
                    },
                    () =>
                    {
                        newSrc = _decryptoService.Value.FetchSingleDownload(src).Result;
                    }); ;

                output.Attributes.SetAttribute("src", newSrc);

                base.Process(context, output);
            }
        }
    }
}