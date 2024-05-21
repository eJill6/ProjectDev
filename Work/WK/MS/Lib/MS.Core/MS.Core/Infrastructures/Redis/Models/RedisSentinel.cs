namespace MS.Core.Infrastructure.Redis.Models.Settings.Redis
{
    /// <summary>
    /// Redis的MasterSlave連線串
    /// </summary>
    public class RedisSentinel
    {
        /// <summary>
        /// Master連線串
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// 是否讀寫分離
        /// </summary>
        public bool IsReadWritwSeparation { get; set; }

        /// <summary>
        /// Slaves連線串
        /// </summary>
        public IList<string> Sentinel { get; set; } = new List<string>();
    }
}