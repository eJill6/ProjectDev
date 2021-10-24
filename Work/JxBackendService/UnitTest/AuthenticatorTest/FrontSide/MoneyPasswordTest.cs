using Google.Authenticator;
using JxBackendService.Common.Extensions;
using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Repository;
using JxBackendService.Interface.Service;
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
    public class MoneyPasswordTest : BaseTest
    {
        private readonly IUserInfoRelatedReadService _userInfoRelatedReadService;
        private readonly IUserInfoRelatedService _userInfoRelatedService;

        protected override EnvironmentUser EnvLoginUser => new EnvironmentUser()
        {
            Application = JxApplication.FrontSideWeb,
            LoginUser = new BasicUserInfo() { UserId = 73555, UserName = "amber04" }
        };

        public MoneyPasswordTest()
        {
            _userInfoRelatedReadService = DependencyUtil.ResolveJxBackendService<IUserInfoRelatedReadService>(EnvLoginUser, DbConnectionTypes.Slave);
            _userInfoRelatedService = DependencyUtil.ResolveJxBackendService<IUserInfoRelatedService>(EnvLoginUser, DbConnectionTypes.Master);
        }

        /// <summary>
        /// 資金密碼過期判斷
        /// </summary>
        [TestMethod]
        public void IsMoneyPasswordExpired()
        {
            Assert.IsFalse(_userInfoRelatedReadService.IsMoneyPasswordExpired(EnvLoginUser.LoginUser.UserId));
        }

        /// <summary>
        /// 資金密碼修改
        /// </summary>
        [TestMethod]
        public void SaveMoneyPassword()
        {
            BaseReturnModel returnModel = _userInfoRelatedService.SavePassword(new JxBackendService.Model.Param.User.SavePasswordParam()
            {
                UserID = EnvLoginUser.LoginUser.UserId,
                OldPasswordHash = "aA00001111".ToPasswordHash(),
                NewPasswordHash = "bBA00001111".ToPasswordHash(),
                SavePasswordType = PasswordType.Money,
            }, null);

            Assert.IsTrue(returnModel.IsSuccess);

            returnModel = _userInfoRelatedService.SavePassword(new JxBackendService.Model.Param.User.SavePasswordParam()
            {
                UserID = EnvLoginUser.LoginUser.UserId,
                OldPasswordHash = "bBA00001111".ToPasswordHash(),
                NewPasswordHash = "aA00001111".ToPasswordHash(),
                SavePasswordType = PasswordType.Money,
            }, null);

            Assert.IsTrue(returnModel.IsSuccess);
        }
    }

}

