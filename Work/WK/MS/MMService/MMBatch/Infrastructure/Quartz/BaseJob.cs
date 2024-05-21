using Quartz;

namespace MMBatch.Infrastructure.Quartz
{
    [DisallowConcurrentExecution()]
    public abstract class BaseJob : IJob
    {
        public abstract Task Execute(IJobExecutionContext context);
    }
}
