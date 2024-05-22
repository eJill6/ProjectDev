using System;
using System.Configuration;

namespace IMBGDataBase.DBUtility
{
    public class PubConstant
    {
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

