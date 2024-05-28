using Autofac;
using BatchService;
using BatchService.Model.Enum;
using JxBackendService.Model.Attributes;

namespace ProductTransferService
{
    [MockService]
    public partial class BatchMockService : BatchMainService
    {
        protected override void AppendServiceToContainerBuilder(ContainerBuilder containerBuilder)
        {
            base.AppendServiceToContainerBuilder(containerBuilder);
            //containerBuilder.RegisterType(typeof(HttpWebRequestUtilMockService)).AsImplementedInterfaces();
        }

        protected override List<JobSetting> GetJobSettings()
        {
            JobSetting.DeleteChatMessage.IsTriggerQuartzJobOnStartup = true;

            return base.GetJobSettings().Where(w => w == JobSetting.AddChatMessage).ToList();
        }
    }
}