using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Model.MiseLive.Request;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.MiseLive;
using JxBackendService.Model.Common;
using JxBackendService.Model.Entity;
using JxBackendService.Model.Enums;
using JxBackendService.Model.MiseLive.Request;
using JxBackendService.Model.MiseLive.Response;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using JxBackendService.Repository.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using UnitTest.Base;

namespace UnitTest.Util
{
    [TestClass]
    public class TPGameAccountTest : BaseTest
    {
        private readonly ITPGameAccountReadService _tpGameAccountReadService;

        public TPGameAccountTest()
        {
            _tpGameAccountReadService = DependencyUtil.ResolveJxBackendService<ITPGameAccountReadService>(SharedAppSettings.PlatformMerchant, EnvLoginUser, DbConnectionTypes.Slave);
        }

        [TestMethod]
        public void TestLocalToThirdParty()
        {
            BaseReturnDataModel<UserAccountSearchReault> returnModel = _tpGameAccountReadService.GetByTPGameAccount(null, "mslD_888");
            Assert.AreEqual(ReturnCode.Success.Value, returnModel.Code);

            BaseReturnDataModel<UserAccountSearchReault> returnModelByUserId = _tpGameAccountReadService.GetByLocalAccount(588);
            Assert.AreEqual(ReturnCode.Success.Value, returnModel.Code);
        }

        [TestMethod]
        public void TestMethod1()
        {
            string inlodbConnectionString = "Data Source=amd2-dev-db.ark88.net;Initial Catalog=InLodb;Persist Security Info=True;User ID=polyqqccwin;Password=polyqqccwin;TrustServerCertificate=true";

            //using (var conn = new SqlConnection(inlodbConnectionString))
            //{
            //    conn.Open();
            //}

            //Server=localhost;Database=mydb;User=ruyut;Password=ruyut;TrustServerCertificate=true
            var dbHelperSQL = new DbHelperSQL(inlodbConnectionString);

            List<UserInfo> users = dbHelperSQL.QueryList<UserInfo>("SELECT TOP 1 * FROM USERINFO", null);
        }

        [TestMethod]
        public void MultiTaskLogon()
        {
            string strUserId = "513999";
            //string requestUrl = $"http://localhost:62340/?userId={strUserId}&userName=%E5%A4%A7%E8%A1%A3%E5%93%A5&clientWebPageValue=GameCenter";
            string requestUrl = $"http://192.168.104.70/?userId={strUserId}&userName=GOGOGO&clientWebPageValue=GameCenter";
            string apiResult;

            var webRequestParam = new WebRequestParam()
            {
                Purpose = $"Service請求",
                Method = HttpMethod.Get,
                Url = requestUrl,
                ContentType = HttpWebRequestContentType.Json,
                IsResponseValidJson = true
            };

            int requestCount = 100;

            Task[] tasks = new Task[requestCount];

            for (int i = 0; i < requestCount; i++)
            {
                tasks[i] = Task.Run(() => HttpWebRequestUtil.GetResponse(webRequestParam, out HttpStatusCode httpStatusCode));
            }

            // 等待所有的 Task 完成
            Task.WaitAll(tasks);
        }
    }
}