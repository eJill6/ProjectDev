using JxBackendService.Common.Util;
using JxBackendService.Interface.Repository;
using JxBackendService.Interface.Service;
using JxBackendService.Model.Entity;
using JxBackendService.Model.Entity.Base;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Param;
using JxBackendService.Model.Param.User;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using JxBackendService.Repository;
using JxBackendService.Repository.Base;
using JxBackendService.Repository.User;
using JxBackendService.Service.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace JxBackendService
{
    public class LoginDetailBackEndService : BaseService, ILoginDetailBackEndService
    {
        private readonly ILoginDetailBackEndRep _loginDetailBackEndRep;

        public LoginDetailBackEndService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _loginDetailBackEndRep = ResolveJxBackendService<ILoginDetailBackEndRep>();
        }

        public BaseReturnDataModel<long> InsertLoginDetail(InsertLoginDetailBackendParam param)
        {
            return _loginDetailBackEndRep.CreateByProcedure(new LoginDetail_BackEnd()
            {
                UserID = param.UserID,
                UserName = param.UserName,
                LoginIp = param.LoginIp,
                LoginTime = param.LoginTime,
                MachineName = param.MachineName,
                WinLoginName = param.WinLoginName,
                LocalIP = param.LoginIp,
                LocalUTCTime = param.LocalUTCTime,
                LoginFail = param.LoginStatus != LoginStatuses.Success,
                LoginToolVersion = param.LoginToolVersion,
                LoginStatus = (int)param.LoginStatus
            });
        }
    }
}
