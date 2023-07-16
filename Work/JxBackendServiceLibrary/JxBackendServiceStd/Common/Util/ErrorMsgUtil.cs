using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.Util;
using JxBackendService.Model;
using JxBackendService.Model.Attributes;
using JxBackendService.Model.Common;
using JxBackendService.Model.ViewModel;
using System;
using System.Linq;

namespace JxBackendService.Common.Util
{
    public static class ErrorMsgUtil
    {
        public static void DoWorkWithErrorHandle(EnvironmentUser environmentUser, Action serviceWork)
        {
            DoWorkWithErrorHandle(environmentUser, serviceWork, false);
        }

        public static void DoWorkWithErrorHandle(EnvironmentUser environmentUser, Action serviceWork, bool isThrowException)
        {
            DoWorkWithErrorHandle(environmentUser, () =>
            {
                serviceWork.Invoke();
                return true;
            }, isThrowException);
        }

        public static T DoWorkWithErrorHandle<T>(EnvironmentUser environmentUser, Func<T> serviceWork)
        {
            return DoWorkWithErrorHandle(environmentUser, serviceWork, false);
        }

        public static T DoWorkWithErrorHandle<T>(EnvironmentUser environmentUser, Func<T> serviceWork,
            bool isThrowException, T exceptionDefaultResult = default(T))
        {
            try
            {
                return serviceWork.Invoke();
            }
            catch (Exception ex)
            {
                ErrorHandle(ex, environmentUser);

                if (isThrowException)
                {
                    throw ex;
                }
            }

            return exceptionDefaultResult;
        }

        public static string GetErrorMsgWithEnvironmentInfo(this Exception ex, EnvironmentUser environmentUser)
        {
            return GetErrorMsgWithEnvironmentInfo(ex.ToString(), environmentUser);
        }

        public static string GetErrorMsgWithEnvironmentInfo(string exceptionMessage, EnvironmentUser environmentUser)
        {
            EnvironmentInfo environmentInfo = environmentUser.ToEnvironmentInfo();
            string errorMessage = $"EnvironmentInfo={environmentInfo.ToJsonString(ignoreNull: true, ignoreDefault: true)}, "
                + exceptionMessage;

            return errorMessage;
        }

        public static void ErrorHandle(this Exception exception, EnvironmentUser environmentUser)
        {
            exception.ErrorHandle(environmentUser, isSendMessageToTelegram: true);
        }

        public static void ErrorHandle(this Exception exception, EnvironmentUser environmentUser, bool isSendMessageToTelegram)
        {
            if (exception == null)
            {
                return;
            }

            if (exception.GetType().GetCustomAttributes(true).Where(w => w is IgnoreErrorHandleAttribute).Any())
            {
                return;
            }

            //log error
            var logUtilService = DependencyUtil.ResolveService<ILogUtilService>();
            logUtilService.Error(exception.GetErrorMsgWithEnvironmentInfo(environmentUser));

            //notification
            if (isSendMessageToTelegram)
            {
                TelegramUtil.SendMessageWithEnvInfoAsync(new SendTelegramParam()
                {
                    ApiUrl = SharedAppSettings.TelegramApiUrl,
                    EnvironmentUser = environmentUser,
                    Message = exception.ToString()
                });
            }
        }
    }
}