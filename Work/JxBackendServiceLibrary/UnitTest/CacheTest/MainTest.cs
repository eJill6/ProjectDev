using JxBackendService.Common.Util;
using JxBackendService.Common.Util.Cache;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Model;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Param.Cache;
using JxBackendService.Model.ViewModel;
using JxBackendService.Service.Cache;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using UnitTest.Base;

namespace UnitTest.CacheTest
{
    [TestClass]
    public class MainTest : BaseTest
    {
        private readonly IJxCacheService _jxCacheService;

        public MainTest()
        {
            _jxCacheService = DependencyUtil.ResolveServiceForModel<IJxCacheService>(JxApplication.FrontSideWeb);
        }

        [TestMethod]
        public void TestFrontSideUserHash()
        {
            var loginHash = Guid.NewGuid().ToString().Replace("-", "");
            CacheKey cacheKey = CacheKey.GetFrontSideUserInfoKey(loginHash);
            string userData = "{\"UserID\":69778,\"UserName\":\"jackson\",\"RebatePro\":0.075,\"AddedRebatePro\":0.002,\"key\":\"Mobile69778Qaj0cHPAsY\",\"HostName\":\"::1\",\"RoleID\":4,\"lastLoginIp\":\"0000:0000:0000:0000:0000:0000:0000:0001\",\"lastLoginAddress\":\"\",\"IsGiveBonus\":true,\"IsPwdRotection\":true,\"NewAddedRebatePro\":0.0000,\"AGUserName\":\"jackson\",\"AGGamePwd\":\"E7B84ACF2D62C5B0\",\"Level\":3,\"AgAvaliableScores\":0.0,\"SportAvaliableScores\":0.0,\"PTAvaliableScores\":0.0,\"AgFreezeScores\":0.0,\"SportFreezeScores\":0.0,\"PTFreezeScores\":0.0,\"AllGain\":-5.7450,\"AGAllGain\":0.0,\"SportAllGain\":0.0,\"PTAllGain\":0.0,\"LCAvaliableScores\":0.0,\"LCFreezeScores\":0.0,\"LCAllGain\":0.0,\"IMAvaliableScores\":0.0,\"IMFreezeScores\":0.0,\"IMAllGain\":0.0,\"RGAvaliableScores\":0.0,\"RGFreezeScores\":0.0,\"RGAllGain\":0.0}";
            //userData = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";

            _jxCacheService.SetCache(new SearchCacheParam()
            {
                Key = cacheKey,
                CacheSeconds = 100,
                IsSlidingExpiration = true,
            }, userData);

            string data = null;

            for (int i = 1; i < 20; i++)
            {
                data = _jxCacheService.GetCache<string>(new SearchCacheParam()
                {
                    Key = cacheKey,
                    CacheSeconds = 100,
                    IsSlidingExpiration = true,
                }, null);
            }

            Assert.IsTrue(!data.IsNullOrEmpty());

            _jxCacheService.RemoveCache(cacheKey);

            //jxCacheService.SetCache<string>(new SearchCacheParam()
            //{
            //    Key = cacheKey,
            //    CacheSeconds = 1,
            //    IsSlidingExpiration = true,
            //}, string.Empty);

            data = _jxCacheService.GetCache<string>(new SearchCacheParam()
            {
                Key = cacheKey,
                CacheSeconds = 100,
                IsSlidingExpiration = true,
            }, null);

            Assert.IsTrue(data.IsNullOrEmpty());
        }

        [TestMethod]
        public void TestCacheServerMemory()
        {
            CacheKey cacheKey = CacheKey.GetFrontSideUserInfoKey("afasdfrqweprqpw");
            _jxCacheService.RemoveCache(cacheKey);

            _jxCacheService.GetCache(new SearchCacheParam()
            {
                Key = cacheKey,
                CacheSeconds = 5,
                IsSlidingExpiration = false,
                IsForceRefresh = false,
            }, () => "123");

            _jxCacheService.GetCache(new SearchCacheParam()
            {
                Key = cacheKey,
                CacheSeconds = 5,
                IsSlidingExpiration = false,
                IsForceRefresh = true,
            }, () => "123123");

            string content = null;
            Thread.Sleep(3000);
            content = _jxCacheService.GetCache<string>(cacheKey);
            Thread.Sleep(3000);
            content = _jxCacheService.GetCache<string>(cacheKey);

            Assert.IsTrue(content == null);

            _jxCacheService.GetCache(new SearchCacheParam()
            {
                Key = cacheKey,
                CacheSeconds = 5,
                IsSlidingExpiration = true,
                IsForceRefresh = false,
            }, () => DateTime.Now.ToFormatDateTimeString());

            Thread.Sleep(3000);
            content = _jxCacheService.GetCache<string>(cacheKey);
            Assert.IsTrue(content != null);

            Thread.Sleep(3000);
            content = _jxCacheService.GetCache<string>(cacheKey);
            Assert.IsTrue(content == null);
        }
    }
}