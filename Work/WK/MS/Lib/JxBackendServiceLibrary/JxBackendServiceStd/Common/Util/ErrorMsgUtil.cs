using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.Util;
using JxBackendService.Model.Attributes;
using JxBackendService.Model.Common;
using JxBackendService.Model.ErrorHandle;
using JxBackendService.Model.ViewModel;
using System;
using System.Linq;
using System.Text;

namespace JxBackendService.Common.Util
{
    public enum SendErrorMsgTypes
    {
        Direct,

        Queue
    }

    public static class ErrorMsgUtil
    {
        public static void DoWorkWithErrorHandle(EnvironmentUser environmentUser, Action serviceWork)
        {
            DoWorkWithErrorHandle(environmentUser, serviceWork, isThrowException: false);
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
            return DoWorkWithErrorHandle(environmentUser, serviceWork, isThrowException: false);
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
            exception.ErrorHandle(environmentUser, isSendMessageToTelegram: true, errorId: null);
        }

        public static void ErrorHandle(this Exception exception, EnvironmentUser environmentUser, bool isSendMessageToTelegram, long? errorId)
        {
            if (IsIgnoreException(exception))
            {
                return;
            }

            var allErrorMsg = new StringBuilder();

            if (errorId.HasValue)
            {
                allErrorMsg.Append($"ErrorID = {errorId}, ");
            }

            allErrorMsg.Append(exception.ToString());

            ErrorHandle(allErrorMsg.ToString(), environmentUser, isSendMessageToTelegram);
        }

        public static void ErrorHandle(string errorMsg, EnvironmentUser environmentUser, bool isSendMessageToTelegram)
        {
            Action sendJob = null;

            //notification
            if (isSendMessageToTelegram)
            {
                sendJob = () =>
                {
                    TelegramUtil.SendMessageWithEnvInfoAsync(new SendTelegramParam()
                    {
                        ApiUrl = SharedAppSettings.TelegramApiUrl,
                        EnvironmentUser = environmentUser,
                        Message = errorMsg
                    });
                };
            }

            ErrorHandle(
                errorMsg: GetErrorMsgWithEnvironmentInfo(errorMsg, environmentUser),
                sendJob);
        }

        public static void ErrorHandle(this Exception exception, EnvironmentUser environmentUser, Action sendJob)
        {
            exception.ErrorHandle(environmentUser, sendJob, errorId: null);
        }

        public static void ErrorHandle(this Exception exception, EnvironmentUser environmentUser, Action sendJob, long? errorId)
        {
            if (IsIgnoreException(exception))
            {
                return;
            }

            var allErrorMsg = new StringBuilder();

            if (errorId.HasValue)
            {
                allErrorMsg.Append($"ErrorID = {errorId}, ");
            }

            string errorMsg = exception.GetErrorMsgWithEnvironmentInfo(environmentUser);
            allErrorMsg.Append(errorMsg);

            ErrorHandle(allErrorMsg.ToString(), sendJob);
        }

        public static void ErrorHandle(string errorMsg, Action sendJob)
        {
            if (errorMsg.IsNullOrEmpty())
            {
                return;
            }

            //log error
            var logUtilService = DependencyUtil.ResolveService<ILogUtilService>().Value;
            logUtilService.Error(errorMsg);

            //notification
            if (sendJob != null)
            {
                sendJob.Invoke();
            }
        }

        private static bool IsIgnoreException(Exception exception)
        {
            if (exception == null)
            {
                return true;
            }

            if (exception.GetType().GetCustomAttributes(true).Where(w => w is IgnoreErrorHandleAttribute).Any())
            {
                return true;
            }

            return false;
        }
    }
}