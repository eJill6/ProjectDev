using System;
using JxBackendService.Interface.Repository.Mail;
using JxBackendService.Model.Entity.Mail;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Repository.Base;
using JxBackendService.Repository.Extensions;

namespace JxBackendService.Repository.User
{
    public class SendMailLogRep : BaseDbRepository<SendMailLog>, ISendMailLogRep
    {
        public SendMailLogRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType) { }

        public int GetProviderSentCount(MailServiceProvider mailServiceProvider, DateTime startDate, DateTime endDate)
        {
            string sql = $@"
SELECT  COUNT(0)
{GetFromTableSQL(InlodbType.Inlodb)}
WHERE   ProviderTypeName = @providerTypeName AND CreateDate >= @startDate AND 
        CreateDate < @endDate
";
            return DbHelper.QueryFirstOrDefault<int>(sql, new
            {
                providerTypeName = mailServiceProvider.Value.ToVarchar(50),
                startDate,
                endDate
            });
        }
    }
}
