using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using JxBackendService.Model.ViewModel;

namespace JxBackendService.Common.Util
{
    public static class JxTask
    {
        public static Task Run(EnvironmentUser environmentUser, Action action)
        {
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;

            return Task.Factory.StartNew(() =>
            {
                try
                {
                    action.Invoke();
                }
                catch (Exception ex)
                {
                    ErrorMsgUtil.ErrorHandle(ex, environmentUser);
                    Thread.Sleep(2000);
                }
            });
        }

    }
}