using JxBackendService.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Interface.Service.Mail
{
    public interface ISendMailLogReadService
    {
        int GetProviderSentCount(MailServiceProvider mailServiceProvider, DateTime startDate, DateTime endDate);
    }

    public interface ISendMailLogService
    {
        bool Create(string providerTypeName, decimal elapsedSeconds, object refInfo);
    }
}
