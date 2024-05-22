using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.Security;
using JxBackendService.Model.Security;
using MobileApiCreateSign.Model;
using Org.BouncyCastle.Math;

namespace MobileApiCreateSign
{
    public partial class Generate : Form
    {
        private static int _userID = 0;

        private static string _userName = string.Empty;

        private static string _depositUrl = string.Empty;

        private readonly BigInteger _mobilePrivateKey = new BigInteger("58879247872359737192647643173138716901210792759286662204075188319470732859006");

        public Generate()
        {
            InitializeComponent();
        }

        private void BtnGenerate_Click(object sender, EventArgs e)
        {
            string tempUserID = txtUserID.Text.ToTrimString();

            if (tempUserID.IsNullOrEmpty())
            {
                txtMessage.Text = "請輸入 userID！";
                return;
            }

            if (!int.TryParse(tempUserID, out _userID))
            {
                txtMessage.Text = "userID 輸入格式不正確！";
                return;
            }

            _userName = txtUserName.Text.ToTrimString();

            if (_userName.IsNullOrEmpty())
            {
                txtMessage.Text = "請輸入 userName！";
                return;
            }

            _depositUrl = txtDepositUrl.Text.ToTrimString();

            if (_depositUrl.IsNullOrEmpty())
            {
                txtMessage.Text = "請輸入 depositUrl！";
                return;
            }

            txtMessage.Text = GenerateResult();

            Clipboard.Clear();
            Clipboard.SetText(txtMessage.Text);
            ShowToast(true, "複製成功");
        }

        private void ShowToast(bool isAutoClose, string message)
        {
            new Toast(isAutoClose, message).Show();
            Focus();
        }

        private string GenerateResult()
        {
            IECDiffieHellmanService _ecDiffieHellmanService = DependencyUtil.ResolveService<IECDiffieHellmanService>();
            Coordinate userPublicKey = _ecDiffieHellmanService.CalculatePublicKey(_mobilePrivateKey);
            BigInteger userRoomKey = _ecDiffieHellmanService.CalculateRoomKey(_mobilePrivateKey, userPublicKey);

            var param = new MobileApiLogonSignParam
            {
                UserID = _userID,
                UserName = _userName,
                DepositUrl = _depositUrl,
                Key = userRoomKey.ToNonNullString(),
                Coordinate = $"{userPublicKey.X},{userPublicKey.Y}",
                Timestamp = DateTime.UtcNow.ToUnixOfTime(),
            };

            string sign = ValidSignUtil.CreateSign(param);
            param.Sign = sign;

            return param.ToJsonString();
        }

        private void BtnExample_Click(object sender, EventArgs e)
        {
            txtUserID.Text = "588";
            txtUserName.Text = "哈哈哈哈";
            txtDepositUrl.Text = "http://gogogo.com";
        }

        private void GenerateForm_Load(object sender, EventArgs e)
        {
        }
    }
}