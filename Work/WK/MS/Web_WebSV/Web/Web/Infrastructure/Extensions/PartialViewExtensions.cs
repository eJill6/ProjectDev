using JxBackendService.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Infrastructure.Extensions
{
    public static class PartialViewExtensions
    {
        public static MvcHtmlString Partial(this HtmlHelper htmlHelper, string partialViewName, object model)
        {
            htmlHelper.ViewContext.ViewData.Model = model;
            var controllerContext = htmlHelper.ViewContext.Controller.ControllerContext;
            var viewDataDictionary = new ViewDataDictionary(htmlHelper.ViewData) { Model = model };
            using (var writer = new StringWriter(CultureInfo.CurrentCulture))
            {
                var viewContext = new ViewContext(controllerContext,
                    htmlHelper.ViewContext.View, viewDataDictionary, htmlHelper.ViewContext.TempData, writer);

                var sharedRazorViewEngine = new RazorViewEngine();
                var viewEngineResult = sharedRazorViewEngine.FindPartialView(controllerContext, partialViewName, false);
                var partialView = viewEngineResult?.View as RazorView;
                if (partialView == null)
                {
                    throw new ViewNotFoundException(partialViewName);
                }

                viewEngineResult.View.Render(viewContext, writer);

                return new MvcHtmlString(writer.ToString());
            }
        }
    }
}