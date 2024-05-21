using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.Config;
using JxBackendService.Model.Enums;
using System;
using System.Configuration;
using System.Text;

namespace SLPolyGame.Web.DBUtility
{
    public class PubConstant
    {
        private static readonly Lazy<IConfigUtilService> s_configUtilService = DependencyUtil.ResolveService<IConfigUtilService>();

        /// <summary>
        /// 获取连接字符串
        /// </summary>
        public static string ConnectionString
        {
            get
            {

                string _connectionString = s_configUtilService.Value.Get("ConnectionString").Trim();

                return GetTrustServerCertificateConnectString(CommUtil.CommUtil.DecryptDES(_connectionString));
            }
        }

        /// <summary>
        /// 获取连接字符串
        /// </summary>
        public static string ConnectionString_Bak
        {
            get
            {
                string _connectionString = s_configUtilService.Value.Get("ConnectionString_Bak").Trim();

                return GetTrustServerCertificateConnectString(CommUtil.CommUtil.DecryptDES(_connectionString));
            }
        }

        /// <summary>
        /// 获取连接字符串
        /// </summary>
        public static string ConnectionString_Login
        {
            get
            {
                string _connectionString = s_configUtilService.Value.Get("ConnectionString_Login").Trim();

                return GetTrustServerCertificateConnectString(CommUtil.CommUtil.DecryptDES(_connectionString));
            }
        }

        private static string GetTrustServerCertificateConnectString(string connectionString)
        {
            var fullConnectionString = new StringBuilder(connectionString);

            if (connectionString.IndexOf("TrustServerCertificate", StringComparison.CurrentCultureIgnoreCase) < 0)
            {
                if (!connectionString.EndsWith(";"))
                {
                    fullConnectionString.Append(";");
                }

                fullConnectionString.Append("TrustServerCertificate=true");
            }

            return fullConnectionString.ToString();
        }
    }
}
