namespace MS.Core.Infrastructures.MQPublisher
{
    public class MQSettings
    {
        public string Host { get; set; } = null!;
        public string Port { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string VirtualHost { get; set; } = null!;
        public string MQConnectionStr { get; set; }
        public MQSettings[] Datas { get; set; } = new MQSettings[0];
        public MQSettings GetMQConnection()
        {
            this.MQConnectionStr = $"host={Host};port={Port};virtualHost={VirtualHost};username={UserName};password={Password}";
            return this;
        }
    }
}
