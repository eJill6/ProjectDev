using JxBackendService.Common.Util;
using JxBackendService.Service;
using JxBackendService.Service.Mail;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JxBackendService.Model.Enums
{
    public class MailServiceProvider : BaseStringValueModel<MailServiceProvider>
    {
        public Type MailServiceType { get; private set; }
        
        public int? WarningSentCount { get; private set; }
        
        public int? TimeZone { get; private set; }
        
        public DatePeriods? StatDatePeriod { get; set; }

        public bool IsSlidingPeriod { get; set; }

        private MailServiceProvider() { }

        public static MailServiceProvider AWSSES = new MailServiceProvider()
        {
            Value = "AWSSES",
            MailServiceType = typeof(AWSSendMailService),
            TimeZone = 0,
            WarningSentCount = 49000,
            StatDatePeriod = DatePeriods.Day,
            IsSlidingPeriod = true,
            Sort = 1,
        };

        public static MailServiceProvider SendGrid = new MailServiceProvider()
        {
            Value = "SendGrid",
            MailServiceType = typeof(SendGridMailService),
            TimeZone = 0,
            WarningSentCount = 90,
            StatDatePeriod = DatePeriods.Day,
            Sort = 2,
        };

        public static MailServiceProvider InternalApi = new MailServiceProvider()
        {
            Value = "InternalApi",
            MailServiceType = typeof(InternalMailService),
            Sort = 3
        };

        public static List<MailServiceProvider> GetWarningMailServiceProvider()
        {
            return GetAll().Where(w => w.WarningSentCount.HasValue == true).ToList();
        }
    }
}
