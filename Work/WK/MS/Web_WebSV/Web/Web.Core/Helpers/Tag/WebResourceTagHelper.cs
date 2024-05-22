using ControllerShareLib.Helpers;
using JxBackendService.Common.Util;
using Microsoft.AspNetCore.Mvc.Razor.TagHelpers;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text.Encodings.Web;

namespace Web.Core.Helpers.Tag
{
    [HtmlTargetElement("link", Attributes = "href")]
    [HtmlTargetElement("script", Attributes = "src")]
    [HtmlTargetElement("img", Attributes = "src")]
    [HtmlTargetElement("img", Attributes = "aes-src")]
    public class WebResourceTagHelper : UrlResolutionTagHelper
    {
        private static readonly string s_attributeAspAppendVersion = "asp-append-version";

        private static readonly string s_attributeSrc = "src";

        private static readonly string s_attributeAESSrc = "aes-src";

        private static readonly string s_attributeHref = "href";

        private static readonly string s_tagScript = "script";

        private static readonly string s_tagImg = "img";

        public WebResourceTagHelper(IUrlHelperFactory urlHelperFactory, HtmlEncoder htmlEncoder) : base(urlHelperFactory, htmlEncoder)
        {
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            TagHelperAttribute? aspAppendVersionAttribute = context.AllAttributes
                .Where(w => w.Name == s_attributeAspAppendVersion)
                .SingleOrDefault();

            if (GlobalCacheHelper.IsUseCDN ||
                aspAppendVersionAttribute == null)
            {
                var overWriteAttributeNames = new List<string>();

                if (context.TagName == s_tagScript || context.TagName == s_tagImg)
                {
                    overWriteAttributeNames.Add(s_attributeSrc);

                    if (context.TagName == s_tagImg)
                    {
                        overWriteAttributeNames.Add(s_attributeAESSrc);
                    }
                }
                else
                {
                    overWriteAttributeNames.Add(s_attributeHref);
                }

                bool isAppendVersion = true;

                if (aspAppendVersionAttribute != null)
                {
                    isAppendVersion = aspAppendVersionAttribute.Value.ToNonNullString() == "true";
                }

                //完整重寫src/href屬性
                foreach (string overWriteAttributeName in overWriteAttributeNames)
                {
                    TagHelperAttribute? tagHelperAttribute = context.AllAttributes.Where(w => w.Name == overWriteAttributeName).SingleOrDefault();

                    if (tagHelperAttribute == null)
                    {
                        continue;
                    }

                    string attributeValue = tagHelperAttribute.Value.ToString();
                    string newAttributeValue = WebResourceHelper.Content(attributeValue, isAppendVersion, isUseRequestHost: false);
                    output.Attributes.SetAttribute(overWriteAttributeName, newAttributeValue);
                }


                base.Process(context, output);
            }
        }
    }
}