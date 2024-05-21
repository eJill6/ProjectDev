namespace MS.Core.Infrastructure.Redis.Models.Settings.Redis
{
    /// <summary>
    /// Redis的MasterSlave連線串
    /// </summary>
    public class RedisMasterSlave
    {
        /// <summary>
        /// Master連線串
        /// </summary>
        public string Master { get; set; }

        /// <summary>
        /// Slaves連線串
        /// </summary>
        public IList<string> Slaves { get; set; } = new List<string>();
    }
}