using JxBackendService.Service.MessageQueue;

namespace RabbitMQClientTest
{
    public class TestRabbitMQClientManager : RabbitMQClientManager
    {
        public TestRabbitMQClientManager(string hostName, int port, string userName, string password) : base(hostName, port, userName, password)
        {
        }

        public TestRabbitMQClientManager(string hostName, int port, string userName, string password, string virtualHost) : base(hostName, port, userName, password, virtualHost)
        {
        }

        protected override void DoErrorHandle(Exception ex)
        {
            base.DoErrorHandle(ex);
            Console.WriteLine(ex.ToString());
        }
    }
}