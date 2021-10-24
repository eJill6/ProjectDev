using Google.Authenticator;
using JxBackendService.Common.Extensions;
using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Repository;
using JxBackendService.Model.Entity.User.Authenticator;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using JxBackendServiceNet45.Interface.Service.Authenticator;
using JxBackendServiceNet45.Model.Enums;
using JxBackendServiceNet45.Model.ViewModel.Authenticator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using UnitTest.Base;

namespace UnitTest.AuthenticatorTest
{
    [TestClass]
    public class BackSideWebTest : BaseTest
    {
        private readonly IUserAuthenticatorValidService _userAuthenticatorValidService;
        private readonly IUserAuthenticatorValidReadService _userAuthenticatorValidReadService;
        private readonly IUserInfoRep _userInfoRep;
        private readonly IUserAuthenticatorRep _userAuthenticatorRep;

        public BackSideWebTest()
        {
            _userAuthenticatorValidService = DependencyUtil.ResolveJxBackendService<IUserAuthenticatorValidService>(EnvLoginUser, DbConnectionTypes.Master);
            _userAuthenticatorValidReadService = DependencyUtil.ResolveJxBackendService<IUserAuthenticatorValidReadService>(EnvLoginUser, DbConnectionTypes.Slave);
            _userInfoRep = DependencyUtil.ResolveJxBackendService<IUserInfoRep>(EnvLoginUser, DbConnectionTypes.Master);
            _userAuthenticatorRep = DependencyUtil.ResolveJxBackendService<IUserAuthenticatorRep>(EnvLoginUser, DbConnectionTypes.Master);
        }

        /// <summary>
        /// 後台清除綁定
        /// </summary>
        [TestMethod]
        public void UnverifyMemberAutheticator()
        {
            //from form input
            string userName = "jackson";

            BaseReturnModel returnModel = _userAuthenticatorValidService.ForceUnverifiedAuthenticator(userName);

            System.Diagnostics.Debug.WriteLine(returnModel.ToJsonString());
            Console.ReadLine();
        }


        [TestMethod]
        public void TestQrCodeImageUrl()
        {
            QrCodeViewModel qrCodeViewModel = _userAuthenticatorValidService.GetQrCode(new CreateQrCodeViewModelParam()
            {
                Application = JxApplication.BackSideWeb,
                SearchUser = new BaseBasicUserInfo() { UserId = 1, UserName = "admin"},
                CreateQrCodeWithType = AuthenticatorType.Google,
                IsForcedRefresh = false
            });

            File.WriteAllText("C:/temp/qrcode.htm", $"<img src='{qrCodeViewModel.ImageUrl}' />");
        }        

        [TestMethod]
        public void TestManualEntryKey()
        {
            string userName = "admin";
            
            int? userId = _userInfoRep.GetBackSideUserId(userName);
            string encryptAccountSecretKey = _userAuthenticatorRep
                .GetSingleByKey(InlodbType.Inlodb, new UserAuthenticator() { UserID = userId.Value })
                .EncryptAccountSecretKey;

            string commonDataHash = "!@#$2wsx";

            string accountSecretKey = encryptAccountSecretKey.ToDescryptedData(commonDataHash);
            var twoFactorAuthenticator = new TwoFactorAuthenticator();
            SetupCode setupCode = twoFactorAuthenticator.GenerateSetupCode(userName, userName, accountSecretKey, false);
            File.WriteAllText("c:/temp/ManualEntryKey.txt", setupCode.ManualEntryKey);
        }
    }
}
