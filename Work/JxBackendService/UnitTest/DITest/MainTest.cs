using Autofac;
using Autofac.Builder;
using Autofac.Core;
using Autofac.Features.Scanning;
using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.Enums;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel.Game;
using JxBackendService.Service.Game;
using JxBackendService.Service.ThirdPartyTransfer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnitTest.Base;

namespace UnitTest.DITest
{
    [TestClass]
    public class MainTest : BaseTest
    {
        public MainTest()
        {

        }

        [TestMethod]
        public void TestResolveJxBackendInterface()
        {
            //var p1 = new NamedParameter("envLoginUser", EnvLoginUser);
            //var p2 = new NamedParameter("connectionString", InlodbConnectionString);

            var stopWatch = new Stopwatch();
            stopWatch.Start();
            for (int i = 1; i <= 100000; i++)
            {
                //var tpGameApiServices = container.Resolve<IEnumerable<ITPGameApiService>>(p1, p2);
                //var tpGameApiService1 = tpGameApiServices.Where(w => w.Product.Value == PlatformProduct.IMSG.Value).Single();
                var tpGameApiService5 = DependencyUtil.ResolveJxBackendService<ITPGameApiService>(
                    PlatformProduct.IMSG,
                    SharedAppSettings.PlatformMerchant,
                    EnvLoginUser,
                    DbConnectionTypes.Master);

                //var tpGameApiService2 = (ITPGameApiService)Activator.CreateInstance(PlatformProduct.IMSG.TPGameApiServiceType, EnvLoginUser, InlodbConnectionString);

                //var tpGameApiService3 = container.Resolve<ITPGameApiService>(p1, p2);

                //var tpGameApiService4 = container.ResolveKeyed<ITPGameApiService>(PlatformProduct.IMSG.Value, p1, p2);

            }

            stopWatch.Stop();
            Console.WriteLine(stopWatch.Elapsed.TotalSeconds);

            //var list = tpGameApiService.GetTPGameUnprocessedMoneyInInfo();
        }

        [TestMethod]
        public void TestPlatformProductService()
        {
            IPlatformProductService platformProductService = DependencyUtil.ResolveKeyed<IPlatformProductService>(JxApplication.MobileApi, SharedAppSettings.PlatformMerchant);
            var list = platformProductService.GetAll();
            platformProductService = DependencyUtil.ResolveKeyed<IPlatformProductService>(JxApplication.FrontSideWeb, SharedAppSettings.PlatformMerchant);
            list = platformProductService.GetAll();

            IPlatformProductService platformProductService2 = DependencyUtil.ResolveKeyed<IPlatformProductService>(JxApplication.MobileApi, SharedAppSettings.PlatformMerchant);
            var selectListItems = platformProductService2.GetProductSelectList(ProductTypes.OtherLottery, true);

            Console.WriteLine(list.ToJsonString());
        }

        [TestMethod]
        public void TestReportTypeService()
        {
            IReportTypeService reportTypeService = DependencyUtil.ResolveKeyedForModel<IReportTypeService>(JxApplication.MobileApi, SharedAppSettings.PlatformMerchant);

            List<JxBackendSelectListItem> menus = reportTypeService.GetMenus();

            foreach (var menu in menus)
            {
                List<PlatformProduct> products = reportTypeService.GetProductsByReportType(menu.Value.ToInt32());
                List<JxBackendSelectListItem> productItems = reportTypeService.GetProductSelectItemsByReportType(menu.Value.ToInt32());
                ReportInnerSetting setting = reportTypeService.GetReportInnerSetting(menu.Value.ToInt32());
            }


            IReportTypeService webReportTypeService = DependencyUtil.ResolveKeyedForModel<IReportTypeService>(JxApplication.FrontSideWeb, SharedAppSettings.PlatformMerchant);

            menus = webReportTypeService.GetMenus();

            foreach (var menu in menus)
            {
                List<PlatformProduct> products = webReportTypeService.GetProductsByReportType(menu.Value.ToInt32());
                List<JxBackendSelectListItem> productItems = webReportTypeService.GetProductSelectItemsByReportType(menu.Value.ToInt32());
                ReportInnerSetting setting = webReportTypeService.GetReportInnerSetting(menu.Value.ToInt32());
            }
        }
    }
}
