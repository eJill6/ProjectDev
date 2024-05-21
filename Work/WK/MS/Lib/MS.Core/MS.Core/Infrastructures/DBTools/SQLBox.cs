using System.Data;

namespace MMService.DBTools
{
    public class SQLBox
    {
        /// <summary>
        /// sql語法
        /// </summary>
        public string? Sql { get; set; }
        /// <summary>
        /// 參數
        /// </summary>
        public object? Parameter { get; set; }

        public int? CommandTimeout { get; set; }

        public CommandType? CommandType { get; set; }
    }
}
