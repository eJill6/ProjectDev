using RabbitMqUtilCore.Base;
using System;

namespace RabbitMqUtilCore
{
    public class CommonRabbitmqManager : BaseRabbitmqManager
    {
        public CommonRabbitmqManager(string hostName, int port, string userName, string password) : base(hostName, port, userName, password)
        {
        }

        public CommonRabbitmqManager(string hostName, int port, string userName, string password, string virtualHost) : base(hostName, port, userName, password, virtualHost)
        {
        }

        protected override void DoErrorHandle(Exception ex)
        {
            //do nothing
        }
    }
}