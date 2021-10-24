using System;
using System.Collections.Generic;
using System.Linq;
using JxBackendService;
using JxBackendService.Common.Extensions;
using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Param.User;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.Game;
using JxBackendService.Resource.Element;
using JxBackendService.Service.User;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTest.Base;

namespace UnitTest.TPGameAccountTest
{
    [TestClass]
    public class MainTest : BaseTest
    {
        private readonly ITPGameAccountService _tpGameAccountService;
        private readonly ITPGameAccountReadService _tpGameAccountReadService;
        private readonly UserInfoRelatedService _userInfoRelatedService;

        public MainTest()
        {
            _tpGameAccountService = DependencyUtil.ResolveJxBackendService<ITPGameAccountService>(SharedAppSettings.PlatformMerchant, EnvLoginUser, DbConnectionTypes.Master);
            _tpGameAccountReadService = DependencyUtil.ResolveJxBackendService<ITPGameAccountReadService>(SharedAppSettings.PlatformMerchant, EnvLoginUser, DbConnectionTypes.Slave);
            _userInfoRelatedService = new UserInfoRelatedService(EnvLoginUser, DbConnectionTypes.Master);
        }


        [TestMethod]
        public void TestGetAll()
        {
            List<PlatformProduct> list = PlatformProduct.GetAll();
            PlatformProduct productAg = list.Where(w => w.Value == PlatformProduct.AG.Value).Single();
            Assert.AreEqual(PlatformProduct.AG.Name, PlatformProductElement.AG);
        }

        [TestMethod]
        public void TestLocalToThirdParty()
        {
            BaseReturnDataModel<UserAccountSearchReault> returnModel = _tpGameAccountReadService.GetByLocalAccount("jackson");
            Assert.AreEqual(ReturnCode.Success.Value, returnModel.Code);
        }

        [TestMethod]
        public void TestThirdPartyToLocal()
        {
            BaseReturnDataModel<UserAccountSearchReault> returnModel = _tpGameAccountReadService.GetByTPGameAccount(null, "jx_69778");
            Assert.AreEqual(ReturnCode.Success.Value, returnModel.Code);

            returnModel = _tpGameAccountReadService.GetByTPGameAccount(PlatformProduct.IM, "jx_69778");
            Assert.AreEqual(1, returnModel.DataModel.TPGameAccountSearchResults.Count);

            returnModel = _tpGameAccountReadService.GetByTPGameAccount(PlatformProduct.IM, "im2orjx_69778");
            Assert.AreEqual(returnModel.DataModel.TPGameAccountSearchResults.Count, 1);

            returnModel = _tpGameAccountReadService.GetByTPGameAccount(null, "im2orjx_69778");
            Assert.AreNotEqual(returnModel.DataModel.TPGameAccountSearchResults.Count, 0);
        }

        [TestMethod]
        public void TestValidationUserBank()
        {
            _userInfoRelatedService.CheckUserBankHasActive(111, ModifyUserDataTypes.UserEmail);
            UserVaildBankParam param = new UserVaildBankParam()
            {
                UserID = 69778,
                BankTypeID = 6,
                CardUser = "我",
                BankCard = "7222021603014391508",
                ModifyUserDataType = ModifyUserDataTypes.UserEmail
            };

            _userInfoRelatedService.ValidationUserBank(param);

            var item = new UserModifyDataParam
            {
                UserID = 69778,
                BankTypeID = 6,
                CardUser = "我",
                BankCard = "7222021603014391508",
                ModifyUserDataType = ModifyUserDataTypes.UserEmail,
                Memo = "TTTTT",
                ModifyContent = "78945643123156"
            };

            _userInfoRelatedService.ModifyUserDataContent(item);
        }

        [TestMethod]
        public void TestGetAllTPGameUserScores()
        {
            List<UserProductScore> userProductScores = _tpGameAccountReadService.GetAllTPGameUserScores(69778, false);
            string json = userProductScores.ToJsonString();
            System.Diagnostics.Debug.WriteLine(json);
        }

        [TestMethod]
        public void Test1()
        {
            var userNames = new HashSet<string>
            {
                "jxd_69778"
            };

            Dictionary<string, int> resultMap = _tpGameAccountReadService.GetUserIdsByTPGameAccounts(PlatformProduct.ABEB, userNames);
            System.Diagnostics.Debug.WriteLine(resultMap.ToJsonString());
        }

        [TestMethod]
        public void Test2()
        {
            string email = "eric.lei@ark88.net";
            var appSettingService= DependencyUtil.ResolveKeyedForModel<IAppSettingService>(JxApplication.FrontSideWeb, SharedAppSettings.PlatformMerchant);
            string encryptedEmail= email.ToEncryptedEmail(appSettingService.EmailHash);
        }

        //[TestMethod]
        //public void TestCRUD()
        //{
        //    var loginUser = new BasicUserInfo()
        //    {
        //        UserId = 1,
        //        UserName = "Admin"
        //    };

        //    var tpGameAccountService = new TPGameAccountService(loginUser, InlodbConnectionString);
        //    tpGameAccountService.Create(69778, "jackson", PlatformProduct.AG, "jackson");
        //    tpGameAccountService.Update(69778, PlatformProduct.AG, "jackson1");
        //    tpGameAccountService.Delete(69778, PlatformProduct.AG);
        //}
    }
}
