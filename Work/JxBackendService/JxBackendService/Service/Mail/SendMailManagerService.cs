using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.Mail;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Param.Mail;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using JxBackendService.Resource.Element;
using JxBackendService.Service.Base;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace JxBackendService.Service.Mail
{
    public class SendMailManagerService : BaseService, ISendMailManagerService
    {
        private static readonly ConcurrentDictionary<string, int> _providerErrorCount = new ConcurrentDictionary<string, int>();
        private static readonly int _errorHandleCountLimit = 10;
        
        private readonly ISendMailLogService _sendMailLogService;

        public SendMailManagerService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _sendMailLogService = ResolveJxBackendService<ISendMailLogService>();
        }

        public BaseReturnModel SendMail(SendMailParam sendMailParam, object refInfo)
        {
            string currentErrorMsg;

            //按照provider優先順序(sort)發信
            foreach (MailServiceProvider mailServiceProvider in MailServiceProvider.GetAll())
            {
                var mailService = DependencyUtil.ResolveKeyed<IMailService>(mailServiceProvider);
                int currentErrorCount;

                try
                {
                    var stopwatch = new Stopwatch();
                    stopwatch.Start();
                    BaseReturnModel baseReturnModel = mailService.SendMail(sendMailParam);
                    stopwatch.Stop();
                    currentErrorCount = CalcErrorCount(mailServiceProvider.Value, baseReturnModel.IsSuccess);

                    if (baseReturnModel.IsSuccess)
                    {
                        _sendMailLogService.Create(mailServiceProvider.Value, stopwatch.ElapsedMilliseconds / (decimal)1000 , refInfo);
                        return baseReturnModel;
                    }

                    throw new Exception(baseReturnModel.Message);
                }
                catch (Exception ex)
                {
                    currentErrorCount = CalcErrorCount(mailServiceProvider.Value, false);
                    ErrorMsgUtil.ErrorHandle(ex, EnvLoginUser);
                    currentErrorMsg = ex.Message;
                }

                if (currentErrorCount >= _errorHandleCountLimit)
                {
                    //warning to telegram
                    ErrorMsgUtil.ErrorHandle(new Exception($"{GetType().Name}發送mail連續失敗{_errorHandleCountLimit}次,{currentErrorMsg}"), EnvLoginUser);
                    //發送過告警就歸0,否則線路不通的情況下會一直發警告
                    CalcErrorCount(mailServiceProvider.Value, true);
                }
            }

            //不回傳實際錯誤訊息
            return new BaseReturnModel(MessageElement.OperationFail);
        }

        private int CalcErrorCount(string providerKey, bool isSuccess)
        {
            _providerErrorCount.TryGetValue(providerKey, out int errorCount);

            if (isSuccess)
            {
                errorCount = 0;
            }
            else
            {
                errorCount++;
            }

            _providerErrorCount[providerKey] = errorCount;
            return errorCount;
        }
    }
}
