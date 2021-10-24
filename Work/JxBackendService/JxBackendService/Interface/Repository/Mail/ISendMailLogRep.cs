using JxBackendService.Model.Entity.Mail;
using JxBackendService.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Interface.Repository.Mail
{
    public interface ISendMailLogRep : IBaseDbRepository<SendMailLog>
    {
        int GetProviderSentCount(MailServiceProvider mailServiceProvider, DateTime startDate, DateTime endDate);
    }
}
