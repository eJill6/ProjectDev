using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.Wcf;
using Autofac.Integration.WebApi;
using JxBackendService.DependencyInjection;
using JxBackendServiceNF.DependencyInjection;
using JxBackendServiceNF.Model.ServiceModel;
using Microsoft.Extensions.Logging;
using PolyDataBase.Services;
using SLPolyGame.Web.Common;
using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace SLPolyGame.Web
{
    public class Global : System.Web.HttpApplication
    {
        private System.Threading.Timer timerforOnlineWarn;

        private static ILogger<Global> _logger = null;

        protected void Application_Start(object sender, EventArgs e)
        {
            // 加上autofac
            var builder = new ContainerBuilder();

            var webAssembly = Assembly.GetExecutingAssembly();

            string assemblyPath = System.IO.Path.Combine(
                System.AppDomain.CurrentDomain.BaseDirectory, "bin\\");

            builder.RegisterInstance(new NLog.Extensions.Logging.NLogLoggerFactory()).As<ILoggerFactory>().SingleInstance();
            builder.RegisterGeneric(typeof(Logger<>)).As(typeof(ILogger<>)).SingleInstance();

            // Register JxLottery Service
            var serviceAssmbly = Assembly.LoadFrom(string.Concat(assemblyPath, "JxLottery.Services.dll"));
            builder.RegisterAssemblyTypes(serviceAssmbly)
                .Where(t => t.Name.EndsWith("Service"))
                .AsImplementedInterfaces();

            // Register JxLottery Adapter
            var adapterAssmbly = Assembly.LoadFrom(string.Concat(assemblyPath, "JxLottery.Adapters.dll"));
            builder.RegisterAssemblyTypes(adapterAssmbly)
                .Where(t => t.Name.EndsWith("Service") || t.Name.EndsWith("Adapter"))
                .AsImplementedInterfaces();

            builder.RegisterType<CacheBase>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<ZipService>()
                .As<IZipService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<DAL.PalyInfo>()
                .As<DAL.PalyInfo>()
                .InstancePerLifetimeScope();

            builder.RegisterType<BLL.PalyInfo>()
                .As<BLL.PalyInfo>()
                .InstancePerLifetimeScope();

            builder.RegisterType<BLL.SysSettings>()
                .As<BLL.SysSettings>()
                .InstancePerLifetimeScope();

            builder.RegisterType<BLL.CurrentLotteryInfo>()
                .As<BLL.CurrentLotteryInfo>()
                .InstancePerLifetimeScope();

            builder.RegisterControllers(webAssembly);
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder = DependencyUtil.GetJxBackendServiceContainerBuilder(assemblyPath, builder);
            builder = DependencyUtilNF.GetJxBackendServiceContainerBuilder(assemblyPath, builder);

            // Set the dependency resolver.
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            AutofacHostFactory.Container = container;
            DependencyUtil.SetContainer(container);
            var dependencyResolver = new AutofacWebApiDependencyResolver(container);
            GlobalConfiguration.Configuration.DependencyResolver = dependencyResolver;

            InitMOperationBehavior();

            _logger = container.Resolve<ILogger<Global>>();

            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);

            RegisterGlobalConfig();

            try
            {
                _logger.LogInformation("SV服务启动时强制用户下线");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "SV服务启动时强制用户下线操作失败，错误描述：" + ex.Message + ",堆栈：" + ex.StackTrace);
            }

            timerforOnlineWarn = new System.Threading.Timer(new System.Threading.TimerCallback(p => OnlineWarn()), null, Timeout.Infinite, Timeout.Infinite);

            timerforOnlineWarn.Change(60 * 1000, System.Threading.Timeout.Infinite);
        }

        private void RegisterGlobalConfig()
        {
            try
            {
                var dataPath = HttpRuntime.AppDomainAppPath.ToString() + @"Data\";

                if (!Directory.Exists(dataPath))
                {
                    Directory.CreateDirectory(dataPath);
                }

                string productName = GlobalCache.ProductName;
                Constant.LotteryTypeName.HSSSC = string.Format(Constant.LotteryTypeName.HSSSC, productName);
                Constant.LotteryTypeName.HS115 = string.Format(Constant.LotteryTypeName.HS115, productName);
                Constant.LotteryTypeName.HSSFC = string.Format(Constant.LotteryTypeName.HSSFC, productName);
                Constant.LotteryTypeName.HSPK = string.Format(Constant.LotteryTypeName.HSPK, productName);
                Constant.LotteryTypeName.HSSEC_MMC = string.Format(Constant.LotteryTypeName.HSSEC_MMC, productName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "SV服务启动时发现配置文件参数设置有误，错误描述：" + ex.Message + ",堆栈：" + ex.StackTrace);
            }
        }

        protected void Application_End(object sender, EventArgs e)
        {
            _logger.LogInformation("应用程序域重启，保存缓存...");

            timerforOnlineWarn.Dispose();

            timerforOnlineWarn = null;

            _logger.LogInformation("应用程序域重启，保存缓存结束");
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            try
            {
                Exception objErr = Server.GetLastError().GetBaseException();
                string error = string.Empty;
                error += "应用程序发生了未经处理的异常<br>";
                error += "发生时间：" + System.DateTime.Now.ToString() + "<br />";
                error += "发生异常页：" + Request.Url.ToString() + "<br />";
                error += "异常信息：" + objErr.Message + "<br />";
                error += "错误源：" + objErr.Source + "<br />";
                error += "堆栈信息：" + objErr.StackTrace + "<br />";
                if (OperationContext.Current != null)
                {
                    error += "访问者IP：" + IP.GetDoWorkIP() + "<br />";
                }
                Server.ClearError();

                _logger.LogError(error);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "应用程序发生了未经处理的异常");
            }
        }

        private void OnlineWarn()
        {
            if (timerforOnlineWarn != null)
            {
                timerforOnlineWarn.Change(60 * 1000, Timeout.Infinite);
            }
        }

        private void InitMOperationBehavior()
        {
            CommonMOperationBehavior.CreateApplicationInspector = (operationDescription) => new DispatchMessageInspector(operationDescription);
        }
    }
}