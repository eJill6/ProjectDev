using JxBackendService.Common.Util;
using JxBackendService.Interface.Repository.Mail;
using JxBackendService.Interface.Service.Mail;
using JxBackendService.Model.Entity.Mail;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Service.Base;
using System;

namespace JxBackendService.Service.Mail
{
    public class SendMailLogService : BaseService, ISendMailLogService, ISendMailLogReadService
    {
        private readonly ISendMailLogRep _sendMailLogRep;

        public SendMailLogService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _sendMailLogRep = ResolveJxBackendService<ISendMailLogRep>();
        }

        public int GetProviderSentCount(MailServiceProvider mailServiceProvider, DateTime startDate, DateTime endDate)
        {
            return _sendMailLogRep.GetProviderSentCount(mailServiceProvider, startDate, endDate);
        }

        public bool Create(string providerTypeName, decimal elapsedSeconds, object refInfo)
        {
            //找回密碼有可能在未登入狀態下使用
            string createUser = EnvLoginUser.LoginUser.UserName;

            if (createUser.IsNullOrEmpty())
            {
                createUser = GlobalVariables.SystemOperator;
            }

            return _sendMailLogRep.CreateByProcedure(new SendMailLog()
            {
                ProviderTypeName = providerTypeName,
                ElapsedSeconds = elapsedSeconds,
                RefInfoJson = refInfo.ToJsonString(),
                CreateUser = createUser,
                UpdateUser = createUser
            }).IsSuccess;
        }
    }
}
