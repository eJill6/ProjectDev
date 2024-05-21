using Castle.DynamicProxy;
using JxBackendService.DependencyInjection;
using JxBackendService.Interceptors.Base;
using JxBackendService.Interface.Service.Finance;
using JxBackendService.Interface.Service.GlobalSystem;
using JxBackendService.Interface.Service.Util;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using JxBackendService.Service.Base;
using System;
using System.Collections.Generic;

namespace JxBackendService.Interceptors
{
    public class MiseLiveTransferServiceInterceptor : IInterceptor
    {
        private static readonly HashSet<string> s_interceptMethodFilters = new HashSet<string>()           
        {
            nameof(IRechargeService.RechargeAllFromMiseLive),
            nameof(IWithdrawService.WithdrawToMiseLive),
        };

        public void Intercept(IInvocation invocation)
        {
            invocation.Proceed(); //執行原流程

            try
            {
                string methodName = invocation.Method.Name;
                var result = invocation.ReturnValue as BaseReturnModel;

                if (!s_interceptMethodFilters.Contains(methodName) ||
                    result == null || result.IsSuccess == true)
                {
                    return;
                }

                object invocationTarget = invocation.InvocationTarget;
                EnvironmentUser environmentUser = (invocationTarget as BaseService).EnvLoginUser;

                var sendTelegramMessageService = DependencyUtil
                    .ResolveJxBackendService<ISendTelegramMessageService>(environmentUser, DbConnectionTypes.Slave).Value;

                string wrapMessage = MiseLiveMessage.CombineMessage(result.Message);

                sendTelegramMessageService.SendToCustomerService(environmentUser.LoginUser,
                    new Exception(wrapMessage).ToString());
            }
            catch (Exception ex)
            {
                DependencyUtil.ResolveService<ILogUtilService>().Value.Error(ex);
            }
        }
    }
}