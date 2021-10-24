using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.VIP;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Paging;
using JxBackendService.Model.Param.User;
using JxBackendService.Model.ViewModel.VIP;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using UnitTest.Base;

namespace UnitTest.ServiceTest
{
    [TestClass]
    public class VIPUserChangeLogServiceTest : BaseTest
    {


        [TestMethod]
        public void GetVIPPointsChangeLogs()
        {
            var vipUserChangeLogService = DependencyUtil.ResolveJxBackendService<IVIPUserChangeLogService>(EnvLoginUser, DbConnectionTypes.Slave);

            for (int i = 1; i <= 4; i++)
            {
                PagedResultModel<VIPPointsChangeLogModel> pagedResult = vipUserChangeLogService.GetVIPPointsChangeLogs(new BaseUserScoreSearchParam()
                {
                    UserID = EnvLoginUser.LoginUser.UserId,
                    PageNum = i,
                    PageSize = 500,
                    StartDate = DateTime.Now.AddDays(-30),
                    EndDate = DateTime.Now
                });

                File.WriteAllText($"{nameof(GetVIPPointsChangeLogs)}_{i}.json", pagedResult.ToJsonString());
            }
        }

        [TestMethod]
        public void GetVIPFlowChangeLogs()
        {
            var vipUserChangeLogService = DependencyUtil.ResolveJxBackendService<IVIPUserChangeLogService>(EnvLoginUser, DbConnectionTypes.Slave);

            for (int i = 1; i <= 4; i++)
            {
                PagedResultModel<VIPFlowChangeLogModel> pagedResult = vipUserChangeLogService.GetVIPFlowChangeLogs(new BaseUserScoreSearchParam()
                {
                    UserID = EnvLoginUser.LoginUser.UserId,
                    PageNum = i,
                    PageSize = 350,
                    StartDate = DateTime.Now.AddDays(-30),
                    EndDate = DateTime.Now
                });

                File.WriteAllText($"{nameof(GetVIPFlowChangeLogs)}_{i}.json", pagedResult.ToJsonString());
            }
        }

        [TestMethod]
        public void GetVIPAgentScoreChangeLogs()
        {
            var vipUserChangeLogService = DependencyUtil.ResolveJxBackendService<IVIPUserChangeLogService>(EnvLoginUser, DbConnectionTypes.Slave);

            for (int i = 1; i <= 4; i++)
            {
                PagedResultModel<VIPAgentAccountLogModel> pagedResult = vipUserChangeLogService.GetVIPAgentScoreChangeLogs(new BaseUserScoreSearchParam()
                {
                    UserID = EnvLoginUser.LoginUser.UserId,
                    PageNum = i,
                    PageSize = 350,
                    StartDate = DateTime.Now.AddDays(-30),
                    EndDate = DateTime.Now
                });

                File.WriteAllText($"{nameof(GetVIPAgentScoreChangeLogs)}_{i}.json", pagedResult.ToJsonString());
            }
        }
    }
}
