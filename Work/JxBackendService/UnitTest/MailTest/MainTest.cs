using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Param.Mail;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;
using UnitTest.Base;

namespace UnitTest.MailTest
{
    [TestClass]
    public class MainTest : BaseTest
    {

        public MainTest()
        {

        }

        private SendMailParam CreateSendMailParam()
        {
            var sendMailParam = new SendMailParam()
            {
                MailTo = null,
                Subject = "測試通知" + DateTime.Now.ToFormatDateTimeString(),
                Body = $"這是一封測試信(id={Guid.NewGuid()}) <br/> " + DateTime.Now.ToFormatDateTimeString()
            };

            if (sendMailParam.MailTo.IsNullOrEmpty())
            {
                throw new ArgumentNullException();
            }

            return sendMailParam;
        }

        [TestMethod]
        public void TestInnerMail()
        {
            var mailService = DependencyUtil.ResolveKeyed<IMailService>(MailServiceProvider.InternalApi);
            mailService.SendMail(CreateSendMailParam());
        }

        [TestMethod]
        public void TestSendGridMail()
        {
            var mailService = DependencyUtil.ResolveKeyed<IMailService>(MailServiceProvider.SendGrid);
            mailService.SendMail(CreateSendMailParam());
        }

        [TestMethod]
        public void TestSendMailManager()
        {
            var sendMailManagerService = DependencyUtil.ResolveJxBackendService<ISendMailManagerService>(EnvLoginUser, DbConnectionTypes.Master);
            const int sendMailCount = 1;

            for (int i = 1; i <= sendMailCount; i++)
            {
                sendMailManagerService.SendMail(CreateSendMailParam(), new { Name = nameof(TestSendMailManager) });
            }
        }

        [TestMethod]
        public void TestSendAmountSendGridMail()
        {
            var sendMailManagerService = DependencyUtil.ResolveJxBackendService<ISendMailManagerService>(EnvLoginUser, DbConnectionTypes.Master);

            for (int i = 1; i <= 130; i++)
            {
                sendMailManagerService.SendMail(CreateSendMailParam(), new { Name = nameof(TestSendMailManager) });
                Thread.Sleep(1000);
            }
        }

        [TestMethod]
        public void TestDateRange()
        {
            DatePeriods datePeriod = DatePeriods.Month;
            int timeZone = -6;

            DateTime providerStartDate;
            DateTime providerEndDate;

            if (datePeriod == DatePeriods.Day)
            {
                DateTime providerDate = DateTime.UtcNow.AddHours(timeZone);
                providerStartDate = new DateTime(providerDate.Year, providerDate.Month, providerDate.Day, 0, 0, 0);
                providerEndDate = providerStartDate.AddDays(1);
            }
            else if (datePeriod == DatePeriods.Month)
            {
                DateTime providerDate = DateTime.UtcNow.AddHours(timeZone);
                providerStartDate = new DateTime(providerDate.Year, providerDate.Month, 1, 0, 0, 0);
                providerEndDate = providerStartDate.AddMonths(1);
            }
            else
            {
                throw new NotSupportedException();
            }

            DateTime chinaStartDate = providerStartDate.AddHours(-timeZone).ToChinaDateTime();
            DateTime chinaEndDate = providerEndDate.AddHours(-timeZone).ToChinaDateTime();

        }
    }
}