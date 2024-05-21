using BackSideWebLoginToolService.Model;
using Newtonsoft.Json;
using System;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace MSLoginTool
{
    public partial class LoginForm : Form
    {
        private static string Key => LoginParamSettings.Key;

        private static string Iv => LoginParamSettings.Iv;

        private static string Ver = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion.ToString();

        private static string[] ntpservers => LoginParamSettings.ntpservers;

        protected Label LblAuthenticatorCode { get => lblAuthenticatorCode; set => lblAuthenticatorCode = value; }

        protected TextBox TxtAuthenticatorCode { get => txtAuthenticatorCode; set => txtAuthenticatorCode = value; }

        protected virtual bool IsClearInputAfterGenerateCode => true;

        protected string GetUserName() => txt_UserName.Text;

        public LoginForm()
        {
            InitializeComponent();
            CenterToScreen();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            Text += $" Ver.{Ver}";
        }

        private void btn_Enter_Click(object sender, EventArgs e)
        {
            string userName = txt_UserName.Text;
            string userPWD = txt_UserPWD.Text;
            string authenticatorCode = GetAuthenticatorCode();

            if (!IsCheckOk(userName, userPWD, authenticatorCode))
            {
                ShowToast(true, "用户名或密码或Google验证码不可以空");

                return;
            }

            string loginParamJsonString = CreateLoginParamJsonString(userName, userPWD, authenticatorCode);
            txt_Code.Text = EncryptDES(loginParamJsonString, Key, Iv);

            if (IsClearInputAfterGenerateCode)
            {
                txt_UserName.Text = string.Empty;
                txt_UserPWD.Text = string.Empty;
                txtAuthenticatorCode.Text = string.Empty;
            }

            if (chkCreateAndCopy.Checked)
            {
                CopyCode();
            }
        }

        private void CopyCode()
        {
            try
            {
                if (!string.IsNullOrEmpty(txt_Code.Text))
                {
                    Clipboard.Clear();
                    Clipboard.SetText(txt_Code.Text);
                    ShowToast(true, "拷贝验证码成功");
                }
            }
            catch
            {
                ShowToast(false, "拷贝验证码失敗");
            }
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            CopyCode();
        }

        private void ShowToast(bool isAutoClose, string message)
        {
            new ToastNotification(isAutoClose, message).Show();
            this.Focus();
        }

        private bool IsCheckOk(string userName, string userPWD, string authenticatorCode)
        {
            if (string.IsNullOrWhiteSpace(userName) ||
                string.IsNullOrWhiteSpace(userPWD) ||
                string.IsNullOrWhiteSpace(authenticatorCode)
                )
            {
                return false;
            }

            return true;
        }

        private string CreateLoginParamJsonString(string userName, string userPWD, string authenticatorCode)
        {
            BWLoginToolParam loginParam = new BWLoginToolParam
            {
                UserId = userName,
                UserPWD = userPWD,
                AuthenticatorCode = authenticatorCode,
                MachineName = Environment.MachineName,
                WinLoginName = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                UserIPs = GetIpAddresses(),
                UTCTime = GetWebTime(),
                LoginToolVersion = Ver
            };

            return JsonConvert.SerializeObject(loginParam);
        }

        protected virtual string GetAuthenticatorCode()
        {
            return txtAuthenticatorCode.Text;
        }

        /// <summary>
        /// 取得本機IP列表
        /// </summary>
        /// <returns></returns>
        private static string[] GetIpAddresses()
        {
            string hostName = Environment.MachineName;

            return System.Net.Dns.GetHostAddresses(hostName).Select(i => i.ToString()).ToArray();
        }

        /// <summary>
        /// 進行加密
        /// </summary>
        /// <param name="data"></param>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        /// <returns></returns>
        private static string EncryptDES(string data, string key, string iv)
        {
            try
            {
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                des.Key = Encoding.ASCII.GetBytes(key);
                des.IV = Encoding.ASCII.GetBytes(iv);
                byte[] s = Encoding.ASCII.GetBytes(data);
                ICryptoTransform desencrypt = des.CreateEncryptor();
                return BitConverter.ToString(desencrypt.TransformFinalBlock(s, 0, s.Length)).Replace("-", string.Empty);
            }
            catch { return string.Empty; }
        }

        /// <summary>
        /// 取得NTP時間
        /// </summary>
        /// <returns></returns>
        private DateTime GetWebTime()
        {
            DateTime webTime = DateTime.UtcNow;
            byte[] ntpData = new byte[48];
            ntpData[0] = 0x1B;

            foreach (string ntpserver in ntpservers)
            {
                try
                {
                    IPAddress[] addresses = Dns.GetHostEntry(ntpserver).AddressList;

                    IPEndPoint ipEndPoint = new IPEndPoint(addresses[0], 123);
                    Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                    socket.Connect(ipEndPoint);
                    socket.ReceiveTimeout = 3000;
                    socket.Send(ntpData);
                    socket.Receive(ntpData);
                    socket.Close();

                    const byte serverReplyTime = 40;
                    ulong intPart = BitConverter.ToUInt32(ntpData, serverReplyTime);
                    ulong fractPart = BitConverter.ToUInt32(ntpData, serverReplyTime + 4);
                    intPart = swapEndian(intPart);
                    fractPart = swapEndian(fractPart);
                    ulong milliseconds = (intPart * 1000) + ((fractPart * 1000) / 0x100000000UL);

                    // UTC time
                    webTime = (new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc)).AddMilliseconds(milliseconds);

                    break;
                }
                catch
                {
                }
            }

            return webTime;
        }

        /// <summary>
        /// 小端儲存與大端儲存的轉換
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        private uint swapEndian(ulong x)
        {
            return (uint)(((x & 0x000000ff) << 24) +
            ((x & 0x0000ff00) << 8) +
            ((x & 0x00ff0000) >> 8) +
            ((x & 0xff000000) >> 24));
        }
    }
}