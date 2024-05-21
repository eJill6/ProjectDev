using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.Web;
using Web.Models.ViewModel;

namespace Web.Infrastructure
{
    public static class ExceptionFilterUtil
    {
        private static readonly string ForbiddenExceptionText = "SLPolyGame.Web.Common.ForbiddenException";

        private static readonly string UnauthorizedExceptionText = "SLPolyGame.Web.Common.UnauthorizedException";

        private static readonly string ValidationExceptionText = "SLPolyGame.Web.Common.ValidationException";

        public static ExceptionInfoViewModel ConvertToExceptionInfo(Exception contextException, EnvironmentUser environmentUser)
        {
            // code: 提供予前端可以準確判斷錯誤的類型, 使用標準的http status code
            int code = (int)HttpStatusCode.InternalServerError;
            string message = null;
            string details = null;
            bool isSendLogExceptionMessage = false;

            if (contextException is FaultException<ExceptionDetail>)
            {
                FaultException<ExceptionDetail> exception = (FaultException<ExceptionDetail>)contextException;
                string ex_message = exception.Message;
                string ex_type = exception.Detail.Type;

                //優先以Excepction類型判斷
                //20210817 [API]回傳"登錄已過期"給APP時，移除data資料
                if (ex_type == UnauthorizedExceptionText
                    || ex_type == ForbiddenExceptionText
                    || ex_message.Contains("登录已过期")) //直接根據內容判斷, 兼容舊版/未知來源
                {
                    return new ExceptionInfoViewModel()
                    {
                        IsLoginExpired = true
                    };
                }
                else if (ex_type == ValidationExceptionText)
                {
                    message = ex_message;

                    code = (int)HttpStatusCode.BadRequest;
                }
                else if (ex_message.Contains("频繁"))
                {
                    //msv並沒有找到這種exception, 可能只是web留下來的代碼.
                    message = "您对页面功能的访问过于频繁，请稍候重试！";
                    // Too Many Requests
                    code = 429;
                }
                else if (ex_message.Contains("版本過舊"))
                {
                    //msv並沒有找到這種exception, 可能只是web留下來的代碼.
                    message = ex_message;
                    // Upgrade Required.
                    code = 426;
                }
                else
                {
                    //其他未知錯誤, 需要log下來以後再分類.
                    message = "应用程序发生错误，请重新登录或稍候重试！";
                    code = (int)HttpStatusCode.InternalServerError;
                    details = ex_message;

                    isSendLogExceptionMessage = true;
                }
            }
            else if (contextException is ValidationException)
            {
                message = contextException.Message;
                code = (int)HttpStatusCode.BadRequest;
            }
            else if (contextException is ArgumentException)
            {
                message = "应用程序发生错误，请重新登录或稍候重试！";
                details = contextException.Message;
                code = (int)HttpStatusCode.BadRequest;

                isSendLogExceptionMessage = true;
            }
            else
            {
                //其他未知錯誤, 需要log下來以後再分類.
                message = "应用程序发生错误，请重新登录或稍候重试！";
                code = (int)HttpStatusCode.InternalServerError;
                details = contextException.Message;

                isSendLogExceptionMessage = true;
            }

            //未知的Exception再走紀錄Log且發送TG訊息
            if (isSendLogExceptionMessage)
            {
                //秘色先註解TGFilter,之後有需要再建起來
                //var instantMessageFilterService = DependencyUtil.ResolveService<IInstantMessageFilterService>();
                //var httpContextService = DependencyUtil.ResolveService<IHttpContextService>();
                //bool isSendMessageToTelegram = instantMessageFilterService.IsAllowToSend(httpContextService.GetAbsoluteUri(), contextException);

                ErrorMsgUtil.ErrorHandle(contextException, environmentUser, isSendMessageToTelegram: true);
            }

            return new ExceptionInfoViewModel()
            {
                code = code,
                message = message,
                details = details
            };
        }
    }
}