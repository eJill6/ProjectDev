using System.Configuration;

namespace Maticsoft.DBUtility
{
    
    public class PubConstant
    {        
        /// <summary>
        /// ��ȡ�����ַ���
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
