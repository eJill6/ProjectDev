using Autofac;
using JxBackendService.Common.Extensions;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.Config;
using JxBackendService.Interface.Service.Util;
using JxBackendService.Model.Entity;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Repository.Base;
using JxBackendServiceN6.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;

namespace UnitTestN6
{
    [TestClass]
    public class UnitTest1 : BaseUnitTest
    {
        public UnitTest1()
        {
            //var containerBuilder = new ContainerBuilder();
            //string assemblyPath = AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar;
            //containerBuilder = DependencyUtil.GetJxBackendServiceContainerBuilder(assemblyPath, containerBuilder);
            //containerBuilder = DependencyUtilN6.GetJxBackendServiceContainerBuilder(assemblyPath, containerBuilder);
            //DependencyUtil.SetContainer(containerBuilder.Build());
        }

        protected override JxApplication Application => JxApplication.FrontSideWeb;

        [TestMethod]
        public void TestDBConnection()
        {
            //Server=localhost;Database=mydb;User=ruyut;Password=ruyut;TrustServerCertificate=true
            string inlodbConnectionString = "Data Source=amd2-dev-db.ark88.net;Initial Catalog=InLodb;Persist Security Info=True;User ID=polyqqccwin;Password=polyqqccwin;TrustServerCertificate=true";
            var dbHelperSQL = new DbHelperSQL(inlodbConnectionString);

            List<UserInfo> users = dbHelperSQL.QueryList<UserInfo>("SELECT TOP 1 * FROM USERINFO", null);

            using (SqlConnection conn = new SqlConnection(inlodbConnectionString))
            {
                conn.Open();
            }
        }

        [TestMethod]
        public void TestNLog()
        {
            var logUtilService = DependencyUtil.ResolveService<ILogUtilService>();
            logUtilService.ForcedDebug(DateTime.Now);
        }

        [TestMethod]
        public void TestReadAppSettings()
        {
            var configUtilService = DependencyUtil.ResolveService<IConfigUtilService>();
            string connString = configUtilService.Get("Master_Inlodb_ConnectionString");
        }

        [TestMethod]
        public void MD5Test()
        {
            string md5Result = HashExtension.MD5("69778");
            string pwd = md5Result.Substring(0, 16);
            Assert.IsTrue(pwd == "8E84B99D8E6287E7");
        }
    }
}