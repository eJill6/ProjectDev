using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.Util;
using JxBackendService.Model;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Resource.Element;
using JxBackendServiceNF.Service.Util;
using Quartz;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatchService.Job.Base
{
    public interface IJxJob
    {
        JxApplication Application { get; }

        EnvironmentUser EnvUser { get; }
    }

    [DisallowConcurrentExecution]
    public abstract class BaseQuartzJob : IJxJob, IJob
    {
        protected ILogUtilService LogUtilService { get; private set; }

        protected BaseQuartzJob()
        {
            LogUtilService = DependencyUtil.ResolveService<ILogUtilService>();
        }

        public abstract JxApplication Application { get; }

        public EnvironmentUser EnvUser => new EnvironmentUser()
        {
            Application = Application,
            LoginUser = new BasicUserInfo() { UserId = 0 }
        };

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

        protected void JobErrorHandle(Exception ex)
        {
            string debugMessage = new
            {
                Title = "BatchService",
                Environment.MachineName,
                EnvironmentCode = SharedAppSettings.GetEnvironmentCode(JxApplication.BatchService).Value,
                Jobs = GetType().Name
            }.ToJsonString();

            string errorMsg = $"{debugMessage} {ex}";
            LogUtilService.Error(errorMsg);

            TelegramUtil.SendMessageWithEnvInfoAsync(new SendTelegramParam()
            {
                ApiUrl = SharedAppSettings.TelegramApiUrl,
                EnvironmentUser = EnvUser,
                Message = errorMsg
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
        public override JxApplication Application => JxApplication.BatchService;
    }

    public class BaseQueueUserWorkItemJob : IJxJob
    {
        protected ILogUtilService LogUtilService { get; private set; }

        public BaseQueueUserWorkItemJob()
        {
            LogUtilService = DependencyUtil.ResolveService<ILogUtilService>();
        }

        public JxApplication Application => JxApplication.BatchService;

        public EnvironmentUser EnvUser => new EnvironmentUser()
        {
            Application = Application,
            LoginUser = new BasicUserInfo() { UserId = 0 }
        };

        protected EnvironmentUser CreateEnvironmentUser(int userId)
        {
            return new EnvironmentUser()
            {
                Application = Application,
                LoginUser = new BasicUserInfo()
                {
                    UserId = userId
                }
            };
        }
    }
}