using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMPTDataBase.DBUtility
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
