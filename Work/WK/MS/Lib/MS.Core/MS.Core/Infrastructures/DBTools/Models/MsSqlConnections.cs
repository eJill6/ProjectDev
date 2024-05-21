namespace MS.Core.Infrastructures.DBTools.Models
{
    /// <summary>
    /// SqlServer連線串
    /// </summary>
    public class MsSqlConnections
    {
        /// <summary>
        /// sqlserver連線字串
        /// </summary>
        public Dictionary<string, string> Connections { get; set; } = new Dictionary<string, string>();
    }
}