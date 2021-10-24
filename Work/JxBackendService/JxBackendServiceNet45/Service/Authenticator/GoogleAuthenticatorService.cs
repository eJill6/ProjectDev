using Google.Authenticator;
using JxBackendService.Model.Enums;
using JxBackendServiceNet45.Interface.Service.Authenticator;
using JxBackendServiceNet45.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JxBackendServiceNet45.Service.Authenticator
{
    public class GoogleAuthenticatorService : IAuthenticatorService
    {
        private static readonly string _defaultIssuer = GlobalVariables.BrandCode;

        public AuthenticatorType AuthenticatorType => AuthenticatorType.Google;

        public GoogleAuthenticatorService()
        {

        }

        public SetupCode GetSetupCode(CreateQrCodeImageParam createQrCodeImageParam)
        {
            var twoFactorAuthenticator = new TwoFactorAuthenticator();
            string issuer = _defaultIssuer;

            if (createQrCodeImageParam.EnvironmentCode != EnvironmentCode.Production)
            {
                issuer += $"({createQrCodeImageParam.EnvironmentCode.Value})";
            }

            bool secretIsBase32 = false;

            SetupCode setupCode = twoFactorAuthenticator.GenerateSetupCode(
                issuer,
                createQrCodeImageParam.AccountTitleNoSpaces,
                createQrCodeImageParam.AccountSecretKey, secretIsBase32);

            return setupCode;
        }

        public bool IsPinValid(string accountSecretKey, string clientPin, bool isCompareExactly)
        {
            //這邊不使用套件的驗證,測試沒有緩衝的效果
            //current會在[10], [9]會是過期的前一個
            //這邊取得兩個讓用戶最少有30秒的緩衝

            const int currentPinIndex = 10;
            string[] allPins = new TwoFactorAuthenticator().GetCurrentPINs(accountSecretKey);
            List<string> filterPins = new List<string>();
            filterPins.Add(allPins[currentPinIndex]);

            if (!isCompareExactly)
            {
                filterPins.Add(allPins[currentPinIndex - 1]);
            }

            return filterPins.Any(a => a == clientPin);
        }
    }
}
