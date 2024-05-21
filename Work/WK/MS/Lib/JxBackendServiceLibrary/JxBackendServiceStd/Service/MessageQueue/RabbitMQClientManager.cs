using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.Util;
using RabbitMqUtilCore.Base;
using System;

namespace JxBackendService.Service.MessageQueue
{
    public class RabbitMQClientManager : BaseRabbitmqManager
    {
        private static readonly Lazy<ILogUtilService> s_logUtilService = DependencyUtil.ResolveService<ILogUtilService>();

        public RabbitMQClientManager(string hostName, int port, string userName, string password) : base(hostName, port, userName, password)
        {
        }

        public RabbitMQClientManager(string hostName, int port, string userName, string password, string virtualHost) : base(hostName, port, userName, password, virtualHost)
        {
        }

        protected override void DoErrorHandle(Exception ex)
        {
            s_logUtilService.Value.Error(ex);
        }
    }
}