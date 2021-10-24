using Google.Authenticator;
using JxBackendService.Common.Extensions;
using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Repository;
using JxBackendService.Interface.Service;
using JxBackendService.Model.Entity.User.Authenticator;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.Permission;
using JxBackendServiceNet45.Interface.Service.Authenticator;
using JxBackendServiceNet45.Model.Enums;
using JxBackendServiceNet45.Model.ViewModel.Authenticator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web.Security;
using UnitTest.Base;

namespace UnitTest.AuthenticatorTest.FrontSide
{
    /// <summary>
    /// 谷歌身份绑定
    /// </summary>
    [TestClass]
    public class GoogleAuthenticatorVerifyTest : BaseTest
    {
        private readonly IUserAuthenticatorValidService _userAuthenticatorValidService;
        private readonly IUserAuthenticatorValidReadService _userAuthenticatorValidReadService;
        private readonly IUserInfoRelatedReadService _userInfoRelatedReadService;

        public GoogleAuthenticatorVerifyTest()
        {
            _userAuthenticatorValidService = DependencyUtil.ResolveJxBackendService<IUserAuthenticatorValidService>(EnvLoginUser, DbConnectionTypes.Master);
            _userAuthenticatorValidReadService = DependencyUtil.ResolveJxBackendService<IUserAuthenticatorValidReadService>(EnvLoginUser, DbConnectionTypes.Slave);
            _userInfoRelatedReadService = DependencyUtil.ResolveJxBackendService<IUserInfoRelatedReadService>(EnvLoginUser, DbConnectionTypes.Slave);
        }

        /// <summary>
        /// step1 進入綁定頁面
        /// </summary>
        [TestMethod]
        public void TestDisplayBindingPage()
        {
            //from cache
            int userId = 69778;
            string userName = "jackson";


            if (!_userAuthenticatorValidReadService.GetUserAuthenticatorInfo(userId).IsSuccess)
            {
                QrCodeViewModel qrCodeViewModel = _userAuthenticatorValidService.GetQrCode(new CreateQrCodeViewModelParam()
                {
                    Application = JxApplication.FrontSideWeb,
                    CreateQrCodeWithType = AuthenticatorType.Google,
                    IsForcedRefresh = true,
                    SearchUser = new BaseBasicUserInfo() { UserId = userId, UserName = userName },
                });

                System.Diagnostics.Debug.WriteLine(qrCodeViewModel.ToJsonString());
                Console.ReadLine();
            }
        }

        /// <summary>
        /// step2 綁定或解綁
        /// </summary>
        [TestMethod]
        public void TestVerifiedAuthenticator()
        {
            //from cache
            int userId = 69778;
            string userName = "jackson";

            //from form input
            string moneyPassword = "a000111";
            string pin = "090984";

            var returnModel = _userAuthenticatorValidService.VerifyAuthenticator(new JxBackendService.Model.Param.User.VerifiedAuthenticatorParam()
            {
                User = new BaseBasicUserInfo() { UserId = userId, UserName = userName },
                IsVerified = true,
                MoneyPasswordHash = moneyPassword.ToPasswordHash(),
                Pin = pin,
            });

            Assert.IsTrue(returnModel.IsSuccess);

            returnModel = _userAuthenticatorValidService.VerifyAuthenticator(new JxBackendService.Model.Param.User.VerifiedAuthenticatorParam()
            {
                User = new BaseBasicUserInfo() { UserId = userId, UserName = userName },
                IsVerified = false,
                MoneyPasswordHash = moneyPassword.ToPasswordHash(),
                Pin = pin,
            });

            Assert.IsTrue(returnModel.IsSuccess);
        }

        [TestMethod]
        public void TestQrCodeImageUrl()
        {
            QrCodeViewModel qrCodeViewModel = _userAuthenticatorValidService.GetQrCode(new CreateQrCodeViewModelParam()
            {
                Application = JxApplication.FrontSideWeb,
                SearchUser = new BaseBasicUserInfo() { UserId = 69778, UserName = "jackson" },
                CreateQrCodeWithType = AuthenticatorType.Google,
                IsForcedRefresh = false
            });

            File.WriteAllText("C:/temp/qrcode.htm", $"<img src='{qrCodeViewModel.ImageUrl}' />");
        }

        [TestMethod]
        public void TestGetAuthenticatorPermission()
        {
            AuthenticatorPermission authenticatorPermission =_userAuthenticatorValidReadService
                .GetAuthenticatorPermission(69778, UserAuthenticatorSettingTypes.TransferToChild);
        }
    }

}

