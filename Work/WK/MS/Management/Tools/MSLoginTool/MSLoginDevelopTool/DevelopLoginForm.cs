using Dapper;
using Google.Authenticator;
using MSLoginTool;
using System;
using System.Collections.Concurrent;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;

namespace MSLoginDevelopTool
{
    public class DevelopLoginForm : LoginForm
    {
        private ComboBox _cbxEnvironment;

        private static readonly ConcurrentDictionary<string, string> s_connectionStringMap = new ConcurrentDictionary<string, string>();

        protected override bool IsClearInputAfterGenerateCode => false;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Text += $" 開發人員專用";

            InitCustomControls();
        }

        protected override string GetAuthenticatorCode()
        {
            string userName = GetUserName();
            string accountSecretKey = GetAccountSecretKey(userName);

            return GetCurrentPin(accountSecretKey);
        }

        private void InitCustomControls()
        {
            TxtAuthenticatorCode.Visible = false;
            LblAuthenticatorCode.Text = "環境：";

            _cbxEnvironment = new ComboBox
            {
                AutoSize = true,
                Location = TxtAuthenticatorCode.Location,
                Name = "cbxEnvironment",
                TabIndex = 9,
                DropDownStyle = ComboBoxStyle.DropDownList,
            };

            _cbxEnvironment.Items.Add("DEV");
            _cbxEnvironment.Items.Add("SIT");
            _cbxEnvironment.Items.Add("UAT");
            _cbxEnvironment.SelectedIndex = 0;

            Controls.Add(_cbxEnvironment);
        }

        private string GetAccountSecretKey(string userName)
        {
            string encryptAccountSecretKey = GetBWUserEncryptAccountSecretKey(userName);
            string commonDataHash = GetCommonDataHash();

            return ToDescryptedData(encryptAccountSecretKey, commonDataHash);
        }

        private string ToDescryptedData(string encryptedData, string commonDataHash)
        {
            DESTool tool = new DESTool(commonDataHash);
            string descryptedContent = CommUtil.CommUtil.DecryptDES(tool.DESDeCode(encryptedData));
            return descryptedContent;
        }

        private string GetBWUserEncryptAccountSecretKey(string userName)
        {
            string sql = @"
                SELECT EncryptAccountSecretKey
                FROM [Inlodb].[dbo].[BWUserAuthenticator] WITH(NOLOCK)
                WHERE
	                UserID = (
		                SELECT UserID FROM BWUserInfo WITH(NOLOCK)
		                WHERE UserName = @userName) ";

            return ExecuteScalar<string>(sql, new { userName = userName.ToNVarchar(50) }, CommandType.Text);
        }

        private T ExecuteScalar<T>(string sql, object param, CommandType cmdType)
        {
            using (IDbConnection connection = GetSqlConnection())
            {
                return connection.ExecuteScalar<T>(sql, param, commandType: cmdType);
            }
        }

        private IDbConnection GetSqlConnection()
        {
            string connectionString = GetConnectionString();
            IDbConnection dbConnection = new SqlConnection(connectionString);

            return dbConnection;
        }

        private string GetConnectionString()
        {
            if (s_connectionStringMap.TryGetValue(_cbxEnvironment.SelectedItem.ToString(), out string connectionString))
            {
                return connectionString;
            }

            string encryptConnectionString = GetConfigByEnvironment("ConnectionString");
            connectionString = CommUtil.CommUtil.DecryptDES(encryptConnectionString);
            s_connectionStringMap.TryAdd(_cbxEnvironment.SelectedItem.ToString(), connectionString);

            return connectionString;
        }

        private string GetCommonDataHash()
        {
            string commonHash = GetConfigByEnvironment("CommonHash");

            return commonHash;
        }

        private string GetConfigByEnvironment(string key)
        {
            string configKey = $"{key}.{_cbxEnvironment.SelectedItem}";

            return ConfigurationManager.AppSettings[configKey];
        }

        private string GetCurrentPin(string accountSecretKey)
        {
            //這邊不使用套件的驗證,測試沒有緩衝的效果
            //current會在[10], [9]會是過期的前一個
            //這邊取得兩個讓用戶最少有30秒的緩衝
            return new TwoFactorAuthenticator().GetCurrentPINs(accountSecretKey).Skip(9).Take(1).Single();
        }
    }
}