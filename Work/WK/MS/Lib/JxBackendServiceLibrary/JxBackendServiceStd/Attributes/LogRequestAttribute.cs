using JxBackendService.Common.Extensions;
using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.Util;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.IO;
using System.Text;

namespace JxBackendService.Attributes
{
    public class LogRequestAttribute : ActionFilterAttribute
    {
        public LogRequestAttribute()
        {
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var logUtilService = DependencyUtil.ResolveService<ILogUtilService>().Value;

            try
            {
                base.OnActionExecuting(filterContext);

                HttpRequest request = filterContext.HttpContext.Request;
                string url = filterContext.HttpContext.Request.ToUri().ToString();
                logUtilService.ForcedDebug($"LogRequest Start. url={url}");

                if (HttpMethod.Post.Value.Equals(request.Method, StringComparison.OrdinalIgnoreCase))
                {
                    Stream inputStream = request.Body;

                    using (var streamReader = new StreamReader(inputStream, Encoding.UTF8,
                        detectEncodingFromByteOrderMarks: true,
                        bufferSize: 1024,
                        leaveOpen: true))
                    {
                        inputStream.Seek(0, SeekOrigin.Begin);
                        string postBody = streamReader.ReadToEnd();

                        //讀取完畢要將讀取位置還原到起始點
                        inputStream.Seek(0, SeekOrigin.Begin);

                        logUtilService.ForcedDebug($"LogRequest Param = {postBody.Replace("\r", string.Empty).Replace("\n", string.Empty)} ");
                    }
                }
            }
            catch (Exception ex)
            {
                logUtilService.ForcedDebug(ex);
            }
        }
    }
}