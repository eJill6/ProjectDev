using JxBackendService.Common.Util;
using JxBackendService.Interface.Repository;
using JxBackendService.Interface.Repository.Security;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.Security;
using JxBackendService.Model.Entity;
using JxBackendService.Model.Entity.Security;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Param.User;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using JxBackendService.Resource.Element;
using JxBackendService.Service.Base;
using System;

namespace JxBackendService.Service.Security
{
    public class FailureOperationService : BaseService, IFailureOperationService
    {
        private readonly IFailureOperationCountRep _failureOperationCountRep;
        private readonly IUserInfoRep _userInfoRep;
        private readonly IOperationLogService _operationLogService;

        public FailureOperationService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _failureOperationCountRep = ResolveJxBackendService<IFailureOperationCountRep>();
            _userInfoRep = ResolveJxBackendService<IUserInfoRep>();
            _operationLogService = ResolveJxBackendService<IOperationLogService>();
        }

        public BaseReturnDataModel<bool?> AddFailOperation(WebActionType webActionType, SubActionType subActionType)
        {
            return AddFailOperation(webActionType, subActionType, null);
        }

        public BaseReturnDataModel<bool?> AddFailOperation(WebActionType webActionType, SubActionType subActionType, string webActionTypeNameParam)
        {
            FailureOperationCount failureOperationCount = _failureOperationCountRep.GetSingleByKey(InlodbType.Inlodb,
                new FailureOperationCount()
                {
                    UserID = EnvLoginUser.LoginUser.UserId,
                    WebActionType = webActionType.Value,
                    SubActionType = subActionType.Value
                });

            if (failureOperationCount == null)
            {
                failureOperationCount = new FailureOperationCount()
                {
                    UserID = EnvLoginUser.LoginUser.UserId,
                    WebActionType = webActionType.Value,
                    SubActionType = subActionType.Value,
                    TotalCount = 1
                };

                _failureOperationCountRep.CreateByProcedure(failureOperationCount);
            }
            else
            {
                failureOperationCount.TotalCount++;
                _failureOperationCountRep.UpdateByProcedure(failureOperationCount);
            }

            //寫入一般失敗的操作紀錄
            if (!subActionType.Name.IsNullOrEmpty())
            {
                string operationLogContent = string.Format(webActionType.Name, webActionTypeNameParam) + ", " + subActionType.Name;

                _operationLogService.InsertFrontSideOperationLogWithUserLoginDetails(new InsertFrontSideOperationLogParam
                {
                    AffectedUserId = EnvLoginUser.LoginUser.UserId,
                    AffectedUserName = EnvLoginUser.LoginUser.UserName,
                    Content = operationLogContent
                });
            }

            bool? isDoFailHandleSuccess = null;

            if (failureOperationCount.TotalCount >= subActionType.FailCountLimit)
            {
                isDoFailHandleSuccess = DoFailHandle(EnvLoginUser.LoginUser.UserId, webActionType, subActionType, webActionTypeNameParam);

                if (!isDoFailHandleSuccess.Value)
                {
                    return new BaseReturnDataModel<bool?>(MessageElement.OperationFail, isDoFailHandleSuccess);
                }
                
                // 凍結成功 刪除失敗操作紀錄
                ClearCount(webActionType, subActionType);

                return new BaseReturnDataModel<bool?>(ReturnCode.Success, isDoFailHandleSuccess);
            }

            return new BaseReturnDataModel<bool?>(ReturnCode.Success, isDoFailHandleSuccess);
        }

        public bool ClearCount(WebActionType webActionType, SubActionType subActionType)
        {
            return _failureOperationCountRep.DeleteByProcedure(new FailureOperationCount()
            {
                UserID = EnvLoginUser.LoginUser.UserId,
                WebActionType = webActionType.Value,
                SubActionType = subActionType.Value
            });
        }

        private bool DoFailHandle(int userId, WebActionType webActionType, SubActionType subActionType, string webActionTypeNameParam)
        {
            switch (subActionType.HandleType)
            {
                case HandleTypes.DisableAccount:
                    return DisableAccount(userId, webActionType, subActionType, webActionTypeNameParam);
                default:
                    throw new NotImplementedException();
            }
        }

        private bool DisableAccount(int userId, WebActionType webActionType, SubActionType subActionType, string webActionTypeNameParam)
        {
            UserInfo userInfo = _userInfoRep.GetSingleByKey(InlodbType.Inlodb, new Model.Entity.UserInfo() { UserID = userId });

            if (userInfo == null || !userInfo.IsActive)
            {
                return true;
            }

            bool isSuccess = _userInfoRep.UpdateUserActive(userId, false);

            if (!isSuccess)
            {
                return isSuccess;
            }

            //冻结帐户, 备注:操作 {0} 功能，资金密码错误达{1}次, 全线：false
            string operationLogContent = string.Format(subActionType.HandleOperationLogContent,
                string.Format(webActionType.Name, webActionTypeNameParam),
                subActionType.FailCountLimit);

            return _operationLogService.InsertFrontSideOperationLogWithUserLoginDetails(JxOperationLogCategory.ExceptionalBehavior,
                new InsertFrontSideOperationLogParam
                {
                    AffectedUserId = EnvLoginUser.LoginUser.UserId,
                    AffectedUserName = EnvLoginUser.LoginUser.UserName,
                    Content = operationLogContent
                }).IsSuccess;
        }

    }
}