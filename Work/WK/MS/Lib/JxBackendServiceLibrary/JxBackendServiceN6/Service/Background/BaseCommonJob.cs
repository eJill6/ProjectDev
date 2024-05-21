using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.Util;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ErrorHandle;
using JxBackendService.Model.ViewModel;

namespace JxBackendServiceN6.Service.Background
{
    public abstract class BaseCommonJob
    {
        private readonly Lazy<ILogUtilService> _logUtilService;

        private static readonly Lazy<IEnvironmentService> s_environmentService = DependencyUtil.ResolveService<IEnvironmentService>();

        public JxApplication Application => s_environmentService.Value.Application;

        public EnvironmentUser EnvUser => new EnvironmentUser()
        {
            Application = Application,
            LoginUser = new BasicUserInfo() { UserId = 0 }
        };

        protected ILogUtilService LogUtilService => _logUtilService.Value;

        protected BaseCommonJob()
        {
            _logUtilService = DependencyUtil.ResolveService<ILogUtilService>();
        }

        protected void JobErrorHandle(Exception exception)
        {
            JobErrorHandle(exception.ToString());
        }

        protected void JobErrorHandle(string exceptionMessage)
        {
            string debugMessage = new
            {
                Title = Application.Value,
                Environment.MachineName,
                EnvironmentCode = SharedAppSettings.GetEnvironmentCode().Value,
                Jobs = GetType().Name
            }.ToJsonString();

            string errorMsg = $"{debugMessage} {exceptionMessage}";
            LogUtilService.Error(errorMsg);

            TelegramUtil.SendMessageWithEnvInfoAsync(new SendTelegramParam()
            {
                ApiUrl = SharedAppSettings.TelegramApiUrl,
                EnvironmentUser = EnvUser,
                Message = errorMsg
            });
        }

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