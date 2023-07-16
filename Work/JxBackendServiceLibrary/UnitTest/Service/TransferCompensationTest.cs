using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel.ThirdParty;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnitTest.Base;

namespace UnitTest.Util
{
    [TestClass]
    public class TransferCompensationTest : BaseTest
    {
        private readonly ITransferCompensationService _transferCompensationService;

        public TransferCompensationTest()
        {
            _transferCompensationService = DependencyUtil.ResolveJxBackendService<ITransferCompensationService>(EnvLoginUser, DbConnectionTypes.Master);
        }

        [TestMethod]
        public void TestSaveMoneyOutCompensation()
        {
            List<int> userIds = new List<int> { 888, 1017, 6251, 6253 };

            BaseReturnModel result = null;

            foreach (int userId in userIds)
            {
                result = _transferCompensationService.SaveMoneyOutCompensation(new SaveCompensationParam
                {
                    TransferID = DateTime.Now.ToString("yyyyMMddHHmmsss"),
                    UserID = userId,
                    ProductCode = PlatformProduct.IMBG.Value,
                });

                Task.Delay(100).Wait();
            }
        }

        [TestMethod]
        public void TestProcessedMoneyOutCompensation()
        {
            BaseReturnModel result = _transferCompensationService.ProcessedMoneyOutCompensation(new ProcessedCompensationParam
            {
                UserID = 6253,
                ProductCode = PlatformProduct.WLBG.Value,
            });
        }

        [TestMethod]
        public void GetTransferCompensations()
        {
            List<int> userIds = _transferCompensationService.GetTransferCompensationUserIds(PlatformProduct.WLBG);
        }
    }
}