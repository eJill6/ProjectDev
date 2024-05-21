namespace MS.Core.Infrastructures.ZeroOne.Models
{
    public class ZeroOneSettings
    {
        public string Domain { get; set; } = null!;
        public string Salt { get; set; } = null!;
        public string UrlDomain { get; set; } = null!;
        public string Xid { get; set; } = null!;
        public string MediaSalt { get; set; } = null!;
        public string RabbitMqConnection { get; set; } = null!;
        public string M3U8Key { get; set; } = null!;
    }
}
