namespace JxBackendService.Model.MessageQueue
{
    public class CommonRabbitMQSetting
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public string VirtualHost { get; set; }
    }

    public class RabbitMQSetting : CommonRabbitMQSetting
    {
        public string HostName { get; set; }

        public int Port { get; set; }
    }

    public class RabbitMQWebSocketSetting : CommonRabbitMQSetting
    {
        public string StompServiceUrl { get; set; }
    }
}