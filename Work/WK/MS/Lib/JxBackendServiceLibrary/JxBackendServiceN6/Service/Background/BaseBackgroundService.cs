using Autofac;
using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.Threading;
using JxBackendService.Interface.Service.Util;
using JxBackendService.Model.Common;
using JxBackendService.Model.ErrorHandle;
using JxBackendService.Model.ViewModel;
using JxBackendServiceN6.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace JxBackendServiceN6.Service.Background
{
    public abstract class BaseBackgroundService : BackgroundService
    {
        private static readonly Lazy<ILogUtilService> s_logUtilService = DependencyUtil.ResolveService<ILogUtilService>();

        private static readonly Lazy<IEnvironmentService> s_environmentService = DependencyUtil.ResolveService<IEnvironmentService>();

        /// <summary>讓子類提供主要的BackgroundService類型，避免父類誤抓到UnitTest專案啟動的Type</summary>
        protected abstract Type MainBackgroundServiceType { get; }

        protected static EnvironmentUser EnvUser => new EnvironmentUser()
        {
            Application = EnvironmentService.Application,
            LoginUser = new BasicUserInfo()
        };

        protected static ILogUtilService LogUtilService => s_logUtilService.Value;

        protected static IEnvironmentService EnvironmentService => s_environmentService.Value;

        public BaseBackgroundService()
        {
            try
            {
                string assemblyPath = AppDomain.CurrentDomain.BaseDirectory;
                // 先建立前置的Container，為了下面註冊正常使用
                var preBuilder = DependencyUtil.GetJxBackendServiceContainerBuilder(assemblyPath, new ContainerBuilder());
                preBuilder = DependencyUtilN6.GetJxBackendServiceContainerBuilder(assemblyPath, preBuilder);
                preBuilder = DependencyUtilN6.RegisterConfiguration(preBuilder);
                RegisterMainBackgroundService(preBuilder);
                DependencyUtil.SetContainer(preBuilder.Build());

                RegisterService();
                var appSettingService = DependencyUtil.ResolveKeyed<IAppSettingService>(EnvironmentService.Application, SharedAppSettings.PlatformMerchant).Value;
                var threadPoolService = DependencyUtil.ResolveService<IThreadPoolService>().Value;
                threadPoolService.SetMinThreads(appSettingService.MinWorkerThreads);
            }
            catch (Exception ex)
            {
                ErrorMsgUtil.ErrorHandle(ex, EnvUser);

                throw;
            }
        }

        private void RegisterService()
        {
            string assemblyPath = AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar;
            ContainerBuilder builder = DependencyUtil.GetJxBackendServiceContainerBuilder(assemblyPath, null);
            builder = DependencyUtilN6.GetJxBackendServiceContainerBuilder(assemblyPath, builder);
            builder = DependencyUtilN6.RegisterConfiguration(builder);
            builder = DependencyUtilN6.RegisterHttpContextAccessor(builder);
            RegisterMainBackgroundService(builder);
            AppendServiceToContainerBuilder(builder);

            DependencyUtil.SetContainer(builder.Build());
        }

        private void RegisterMainBackgroundService(ContainerBuilder containerBuilder) => containerBuilder.RegisterServiceFromType(MainBackgroundServiceType);

        protected virtual void AppendServiceToContainerBuilder(ContainerBuilder containerBuilder)
        { }

        protected void DoJobWithCancellationToken(CancellationToken cancellationToken, int jobIntervalSeconds, Func<bool> doJob)
        {
            int totalDelaySeconds = int.MaxValue;

            while (!cancellationToken.IsCancellationRequested)
            {
                bool isDelayAndWait = true;

                if (totalDelaySeconds >= jobIntervalSeconds)
                {
                    totalDelaySeconds = 0;

                    ErrorMsgUtil.DoWorkWithErrorHandle(EnvUser, () =>
                    {
                        isDelayAndWait = doJob.Invoke();
                    });
                }
                else
                {
                    totalDelaySeconds++;
                }

                if (isDelayAndWait)
                {
                    TaskUtil.DelayAndWait(1000);
                }
                else
                {
                    totalDelaySeconds = int.MaxValue;
                }
            }
        }

        protected void AddDebugMessage(string debugMessage)
        {
            LogUtilService.ForcedDebug(debugMessage);

            TelegramUtil.SendMessageWithEnvInfoAsync(new SendTelegramParam()
            {
                ApiUrl = SharedAppSettings.TelegramApiUrl,
                EnvironmentUser = EnvUser,
                Message = debugMessage
            });
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            try
            {
                AddDebugMessage("服務停止");
            }
            catch (Exception ex)
            {
                ErrorMsgUtil.ErrorHandle(ex, EnvUser);
            }
            finally
            {
                TaskUtil.DelayAndWait(2000);
            }

            return base.StopAsync(cancellationToken);
        }
    }
}