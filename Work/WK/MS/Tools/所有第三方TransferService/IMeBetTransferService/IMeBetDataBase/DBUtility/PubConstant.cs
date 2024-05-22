using System;
using System.Configuration;

namespace IMeBetDataBase.DBUtility
{
    public class PubConstant
    {
        /// <summary>
        /// 鳳蟀諉趼睫揹
        /// </summary>
        public static string ConnectionString
        {
            get
            {
                string _connectionString = ConfigurationManager.AppSettings["ConnectionString"];

                return CommUtil.CommUtil.DecryptDES(_connectionString);
            }
        }
    }
}

