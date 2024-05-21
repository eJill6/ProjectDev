namespace MS.Core.Infrastructure.Redis.Models.Settings.Redis
{
    /// <summary>
    /// Redis連線字串
    /// </summary>
    public class RedisConnections
    {
        /// <summary>
        /// Master Slave模式的連線串
        /// </summary>
        public RedisMasterSlave MasterSlave { get; set; }

        /// <summary>
        /// Sentinel 連線串
        /// </summary>
        public RedisSentinel Sentinel { get; set; }
    }
}