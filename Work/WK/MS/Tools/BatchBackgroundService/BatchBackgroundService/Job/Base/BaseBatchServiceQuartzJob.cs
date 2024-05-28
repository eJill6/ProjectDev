using BatchService.Interface;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendServiceN6.Service.Background;
using Quartz;
using System.Diagnostics;

namespace BatchService.Job.Base
{
    public interface IJxJob
    {
        JxApplication Application { get; }

        EnvironmentUser EnvUser { get; }
    }

    [DisallowConcurrentExecution]
    public abstract class BaseQuartzJob : BaseCommonJob, IJxJob, IJob
    {
        protected BaseQuartzJob()
        {
        }

        public abstract void DoJob();

        public virtual bool IsForcedDebug => false;

        public async Task Execute(IJobExecutionContext context)
        {
            await Task.Run(() =>
            {
                try
                {
                    string startMsg = $"{GetType().FullName} Start";
                    string endMsg = $"{GetType().FullName} End";
                    LogMsg(startMsg);

                    var stopwatch = new Stopwatch();
                    stopwatch.Start();
                    DoJob();
                    stopwatch.Stop();

                    LogMsg(endMsg);
                }
                catch (Exception ex)
                {
                    JobErrorHandle(ex);
                }
            });
        }

        private void LogMsg(string msg)
        {
            if (IsForcedDebug)
            {
                LogUtilService.ForcedDebug(msg);
            }
            else
            {
                LogUtilService.Debug(msg);
            }
        }
    }

    public abstract class BaseBatchServiceQuartzJob : BaseQuartzJob
    {
    }

    public abstract class BaseTaskJob : BaseCommonJob, IJxJob, ITaskJob
    {
        public void DoJob(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return;
            }

            DoWork(cancellationToken);
        }

        protected abstract void DoWork(CancellationToken cancellationToken);
    }
}