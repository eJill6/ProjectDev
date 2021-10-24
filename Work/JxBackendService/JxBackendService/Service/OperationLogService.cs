using IPToolModel;
using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Repository;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.Security;
using JxBackendService.Interface.Service.Net;
using JxBackendService.Model.Entity;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Param.User;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using JxBackendService.Repository;
using JxBackendService.Resource.Element;
using JxBackendService.Service.Base;
using System;

namespace JxBackendService.Service
{
    public class OperationLogService : BaseService, IOperationLogService
    {
        private readonly IOperationLogRep _operationLogRep;
        private readonly IAreaReadService _areaReadService;
        private readonly IIpSystemApiService _ipSystemApiService;
        private readonly IDeviceService _deviceService;
        private readonly IIpUtilService _ipUtilService;

        public OperationLogService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _operationLogRep = ResolveJxBackendService<IOperationLogRep>();
            _areaReadService = DependencyUtil.ResolveJxBackendService<IAreaReadService>(EnvLoginUser, DbConnectionTypes.Slave);
            _ipSystemApiService  = ResolveJxBackendService<IIpSystemApiService>();
            _deviceService = ResolveKeyed<IDeviceService>(envLoginUser.Application);
            _ipUtilService = DependencyUtil.ResolveService<IIpUtilService>();
        }

        /// <summary>
        /// 後台異動會員的操作記錄
        /// </summary>
        public BaseReturnDataModel<long> InsertModifyMemberOperationLog(InsertModifyMemberOperationLogParam param)
        {
            return _operationLogRep.CreateByProcedure(new OperationLog()
            {
                Category = param.Category.Value,
                Content = param.Content,
                OperateDate = DateTime.Now,
                OperateUserSysNo = EnvLoginUser.LoginUser.UserId,
                OperateUserName = EnvLoginUser.LoginUser.UserName,
                UserId = param.AffectedUserId,
                UserName = param.AffectedUserName,
            });
        }

        /// <summary>
        /// 後台異動系統的操作記錄
        /// </summary>
        public BaseReturnDataModel<long> InsertModifySystemOperationLog(InsertModifySystemOperationLogParam param)
        {
            return _operationLogRep.CreateByProcedure(new OperationLog()
            {
                Category = param.Category.Value,
                Content = param.Content,
                OperateDate = DateTime.Now,
                OperateUserSysNo = EnvLoginUser.LoginUser.UserId,
                OperateUserName = EnvLoginUser.LoginUser.UserName,
                UserId = 0,
                UserName = string.Empty,
            });
        }

        /// <summary>
        /// 前台會員自己操作紀錄
        /// </summary>
        public BaseReturnDataModel<long> InsertFrontSideOperationLog(InsertFrontSideOperationLogParam param)
        {
            return InsertFrontSideOperationLog(JxOperationLogCategory.ChangeUserInfo, param);
        }

        /// <summary>
        /// 前台會員自己操作紀錄
        /// </summary>
        public BaseReturnDataModel<long> InsertFrontSideOperationLog(JxOperationLogCategory jxOperationLogCategory, InsertFrontSideOperationLogParam param)
        {
            return _operationLogRep.CreateByProcedure(new OperationLog()
            {
                Category = jxOperationLogCategory.Value,
                Content = param.Content,
                OperateDate = DateTime.Now,
                OperateUserSysNo = EnvLoginUser.LoginUser.UserId,
                OperateUserName = EnvLoginUser.LoginUser.UserName,
                UserId = param.AffectedUserId,
                UserName = param.AffectedUserName
            });
        }

        public BaseReturnDataModel<long> InsertFrontSideOperationLogWithUserLoginDetails(InsertFrontSideOperationLogParam param)
        {
            return InsertFrontSideOperationLogWithUserLoginDetails(JxOperationLogCategory.ChangeUserInfo, param);
        }

        /// <summary>
        /// 前台會員自己操作紀錄, 附帶使用者登入資訊 (IP, 地區, 裝置)
        /// </summary>
        public BaseReturnDataModel<long> InsertFrontSideOperationLogWithUserLoginDetails(JxOperationLogCategory jxOperationLogCategory, InsertFrontSideOperationLogParam param)
        {
            JxIpInformation ipInfo = _ipUtilService.GetDoWorkIPInformation();
            string area = _areaReadService.GetArea(ipInfo);
            string deviceName = _deviceService.GetDeviceName();

            //組合內容: "{content}, IP:{}, 地區:{}, 裝置:{}"
            string compositeContent = string.Join(", ", param.Content, 
                string.Format(OperationLogContentElement.UserLoginDetails, ipInfo.DestinationIP, area, deviceName));

            return InsertFrontSideOperationLog(new InsertFrontSideOperationLogParam
            {
                AffectedUserId = param.AffectedUserId,
                AffectedUserName = param.AffectedUserName,
                Content = compositeContent
            });
        }
    }
}
