using Autofac;
using BatchService.Model.Enum;
using BatchService.Service;
using JxBackendService.Model.Enums;
using JxBackendService.Service.Net;
using System.Collections.Generic;
using System.Linq;

namespace BatchServiceUnitTest
{
    public partial class BatchMainMockService : BatchMainService
    {
        protected override void AppendServiceToContainerBuilder(ContainerBuilder containerBuilder)
        {
            //containerBuilder.RegisterType(typeof(HttpWebRequestUtilMockService)).AsImplementedInterfaces();
        }

        protected override List<JobSetting> GetJobSettings()
        {
            JobSetting.CheckIdleAvailableScores.IsStartNow = true;

            return base.GetJobSettings().Where(w => w == JobSetting.TransferToMiseLive || w == JobSetting.CheckIdleAvailableScores).ToList();
        }
    }
}