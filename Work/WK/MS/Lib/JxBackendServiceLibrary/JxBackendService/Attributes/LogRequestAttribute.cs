using JxBackendService.Common.Util;
using JxBackendService.Common.Util.Route;
using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace JxBackendService.Attributes
{
    public class LogRequestAttribute : ActionFilterAttribute
    {
        private bool _isCamelCaseNaming;


        public LogRequestAttribute(bool isCamelCaseNaming = true)
        {
            _isCamelCaseNaming = isCamelCaseNaming;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {
                base.OnActionExecuting(filterContext);

                string url = filterContext.HttpContext.Request.Url.ToString();
                LogUtil.ForcedDebug($"LogRequest Start. url={url}");

                HttpRequestBase request = filterContext.HttpContext.Request;

                if (HttpMethod.Post.Value.Equals(request.HttpMethod, StringComparison.OrdinalIgnoreCase))
                {
                    Stream inputStream = request.InputStream;

                    using (var streamReader = new StreamReader(inputStream, Encoding.UTF8,
                        detectEncodingFromByteOrderMarks: true,
                        bufferSize: 1024,
                        leaveOpen: true)) 
                    {
                        inputStream.Seek(0, SeekOrigin.Begin);
                        string postBody = streamReader.ReadToEnd();
                        
                        //讀取完畢要將讀取位置還原到起始點
                        inputStream.Seek(0, SeekOrigin.Begin);
                        
                        LogUtil.ForcedDebug($"LogRequest Param = {postBody.Replace("\r",string.Empty).Replace("\n",string.Empty)} ");
                    }
                }

                //以下為舊寫法, 會從action內的參數取得, 但已經是匹配後的結果, 在測試上無法得知原始post資料
                //var allParamContent = new StringBuilder();
                
                //if (filterContext.ActionParameters != null)
                //{
                //    foreach (string key in filterContext.ActionParameters.Keys)
                //    {
                //        if (allParamContent.Length > 0)
                //        {
                //            allParamContent.Append(Environment.NewLine);
                //        }

                //        string paramContents = $"{key} = {filterContext.ActionParameters[key].ToJsonString(isCamelCaseNaming: _isCamelCaseNaming)}";
                //        allParamContent.Append(paramContents);
                //    }
                //}

                //LogUtil.ForcedDebug($"LogRequest Param = {allParamContent} ");
            }
            catch (Exception ex)
            {
                LogUtil.ForcedDebug(ex);
            }
        }
    }
}

