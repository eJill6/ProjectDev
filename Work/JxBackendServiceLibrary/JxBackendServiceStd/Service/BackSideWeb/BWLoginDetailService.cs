using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Repository.BackSideUser;
using JxBackendService.Interface.Service.BackSideUser;
using JxBackendService.Interface.Service.Net;
using JxBackendService.Model.Entity;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.BackSideWeb.Login;
using JxBackendService.Model.Param.User;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using JxBackendService.Service.Base;
using System;

namespace JxBackendService.Service.BackSideWeb
{
    public class BWLoginDetailService : BaseBackSideService, IBWLoginDetailService
    {
        private readonly IBWLoginDetailRep _bwLoginDetailRep;

        private readonly IIpUtilService _ipUtilService;

        public BWLoginDetailService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _bwLoginDetailRep = ResolveJxBackendService<IBWLoginDetailRep>(DbConnectionTypes.Master);
            _ipUtilService = DependencyUtil.ResolveService<IIpUtilService>();
        }

        public BaseReturnDataModel<long> InsertLoginDetail(BWLoginResultParam param)
        {
            string clientIP = _ipUtilService.GetIPAddress();

            return _bwLoginDetailRep.CreateByProcedure(new BWLoginDetail()
            {
                UserID = param.UserID,
                UserName = param.UserName,
                LoginIp = clientIP,
                LoginTime = DateTime.Now,
                MachineName = param.MachineName,
                WinLoginName = param.WinLoginName,
                LocalIP = clientIP,
                LocalUTCTime = param.LocalUTCTime,
                LoginFail = param.LoginStatus != LoginStatuses.Success,
                LoginToolVersion = param.LoginToolVersion,
                LoginStatus = (int)param.LoginStatus
            });
        }
    }
}