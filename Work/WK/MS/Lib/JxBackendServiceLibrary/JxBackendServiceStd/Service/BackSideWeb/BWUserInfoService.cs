using JxBackendService.Common.Extensions;
using JxBackendService.Common.Util;
using JxBackendService.Common.Util.Cache;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Repository.BackSideUser;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.BackSideUser;
using JxBackendService.Interface.Service.MessageQueue;
using JxBackendService.Model.BackSideWeb;
using JxBackendService.Model.Entity.BackSideUser;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.BackSideWeb;
using JxBackendService.Model.Enums.BackSideWeb.Permission;
using JxBackendService.Model.MessageQueue;
using JxBackendService.Model.Paging;
using JxBackendService.Model.Param.BackSideWeb;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using JxBackendService.Resource.Element;
using JxBackendService.Service.Base;
using System;
using System.Collections.Generic;

namespace JxBackendService.Service.BackSideWeb
{
    public class BWUserInfoService : BaseBackSideService, IBWUserInfoService, IBWChangePasswordService
    {
        private readonly Lazy<IBWUserInfoRep> _bwUserInfoRep;

        private readonly Lazy<IBWRoleInfoService> _bwRoleInfoService;

        private readonly Lazy<IJxCacheService> _jxCacheService;

        private readonly Lazy<IMessageQueueDelayJobService> _messageQueueDelayJobService;

        private readonly Lazy<ICacheDelayJobService> _cacheDelayJobService;

        private readonly PermissionKeyDetail _permissionKey = PermissionKeyDetail.UserManagement;

        private static readonly int s_passwordExpiredDays = 30;

        private static readonly string s_editFixedPassword = "$M@kPo#8df";

        private static readonly int s_delayLogoutSeconds = 2;

        public BWUserInfoService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _bwUserInfoRep = ResolveJxBackendService<IBWUserInfoRep>();
            _bwRoleInfoService = ResolveJxBackendService<IBWRoleInfoService>();
            _jxCacheService = DependencyUtil.ResolveService<IJxCacheService>();
            _messageQueueDelayJobService = ResolveJxBackendService<IMessageQueueDelayJobService>(DbConnectionTypes.Slave);
            _cacheDelayJobService = ResolveJxBackendService<ICacheDelayJobService>(DbConnectionTypes.Slave);
        }

        public BaseReturnDataModel<int> Create(CreateBWUserInfoParam createParam)
        {
            if (createParam.RoleID == 0)
            {
                return new BaseReturnDataModel<int>(ReturnCode.DataIsNotCompleted);
            }

            bool isExistUserName = _bwUserInfoRep.Value.IsExistUserName(createParam.UserName);

            if (isExistUserName)
            {
                return new BaseReturnDataModel<int>(ReturnCode.UserNameAlreadyUsed);
            }

            bool isRoleExist = _bwRoleInfoService.Value.IsRoleExist(createParam.RoleID);

            if (!isRoleExist)
            {
                return new BaseReturnDataModel<int>(ReturnCode.RoleIsNotExist);
            }

            BaseReturnDataModel<long> result = _bwUserInfoRep.Value.CreateByProcedure(new BWUserInfo
            {
                UserName = createParam.UserName,
                RoleID = createParam.RoleID,
                Password = createParam.Password.ToPasswordHash(),
                PasswordExpiredDate = GenerateNewPasswordExpiredDate(),
            });

            int userId = _bwUserInfoRep.Value.GetUserInfoByUserName(createParam.UserName).UserID;

            if (!result.IsSuccess)
            {
                return new BaseReturnDataModel<int>(result.Message);
            }

            CreateOperationLog(PermissionElement.Insert, createParam.UserName);

            return new BaseReturnDataModel<int>(ReturnCode.Success, userId);
        }

        public BaseReturnModel Update(UpdateBWUserInfoParam updateParam)
        {
            if (updateParam.RoleID == 0)
            {
                return new BaseReturnModel(ReturnCode.DataIsNotCompleted);
            }

            BWUserInfo source = GetBWUserInfo(updateParam.UserID);

            if (source == null)
            {
                return new BaseReturnModel(ReturnCode.SearchResultIsEmpty);
            }

            var target = source.CastByJson<BWUserInfo>();

            target.RoleID = updateParam.RoleID;

            // 代表密碼有調整過內容
            if (updateParam.Password != s_editFixedPassword)
            {
                target.Password = updateParam.Password.ToPasswordHash();
                target.PasswordExpiredDate = GenerateNewPasswordExpiredDate();
            }

            Dictionary<int, string> roleInfoMaps = _bwRoleInfoService.Value.GetAllBWRoleInfoMaps();

            string compareContent = GetOperationCompareContent(new List<RecordCompareParam>
            {
                new RecordCompareParam
                {
                    Title = DisplayElement.Role,
                    OriginValue = roleInfoMaps[source.RoleID],
                    NewValue = roleInfoMaps[target.RoleID]
                },
                new RecordCompareParam
                {
                    Title = DisplayElement.UpdatePassword,
                    OriginValue = source.Password,
                    NewValue = target.Password,
                    IsVisibleCompareValue = false
                },
            }, ActTypes.Update);

            if (compareContent.IsNullOrEmpty())
            {
                return new BaseReturnModel(ReturnCode.Success);
            }

            bool isSuccess = _bwUserInfoRep.Value.UpdateByProcedure(target);

            if (!isSuccess)
            {
                return new BaseReturnModel(ReturnCode.UpdateFailed);
            }

            CreateOperationLog(PermissionElement.Edit, source.UserName, compareContent);
            DelayLogoutUser(source.UserID);

            return new BaseReturnModel(ReturnCode.Success);
        }

        public BaseReturnModel Delete(int userId)
        {
            BWUserInfo bwUserInfo = GetBWUserInfo(userId);

            if (bwUserInfo == null)
            {
                return new BaseReturnModel(ReturnCode.SearchResultIsEmpty);
            }

            bool isSuccess = _bwUserInfoRep.Value.DeleteByProcedure(bwUserInfo);

            if (!isSuccess)
            {
                return new BaseReturnModel(ReturnCode.OperationFailed);
            }

            CreateOperationLog(PermissionElement.Delete, bwUserInfo.UserName);
            DelayLogoutUser(bwUserInfo.UserID);

            return new BaseReturnModel(ReturnCode.Success);
        }

        public PagedResultModel<UserManagementViewModel> GetPagedBWUserInfos(QueryBWUserInfoParam queryParam)
        {
            PagedResultModel<BWUserInfo> pagedResult = _bwUserInfoRep.Value.GetList(queryParam);

            if (pagedResult.TotalCount == 0)
            {
                return new PagedResultModel<UserManagementViewModel>(queryParam);
            }

            Dictionary<int, string> roleMaps = _bwRoleInfoService.Value.GetAllBWRoleInfoMaps();
            var resultModel = pagedResult.CastByJson<PagedResultModel<UserManagementViewModel>>();

            resultModel.ResultList.ForEach(result =>
            {
                result.RoleName = roleMaps[result.RoleID];
            });

            return resultModel;
        }

        public EditUserInfo GetEditUserInfo(int userId)
        {
            BWUserInfo bwUserInfo = GetBWUserInfo(userId);

            if (bwUserInfo == null)
            {
                return null;
            }

            bwUserInfo.Password = s_editFixedPassword;

            return bwUserInfo.CastByJson<EditUserInfo>();
        }

        public BaseReturnModel ChangePassword(ChangePasswordParam param)
        {
            int userId = EnvLoginUser.LoginUser.UserId;

            BWUserInfo source = GetBWUserInfo(userId);

            if (source == null)
            {
                return new BaseReturnModel(ReturnCode.SearchResultIsEmpty);
            }

            if (source.Password != param.OldPassword.ToPasswordHash())
            {
                return new BaseReturnModel(MessageElement.OldPasswordIsNotValid);
            }

            var target = source.CastByJson<BWUserInfo>();
            target.Password = param.NewPassword.ToPasswordHash();
            target.PasswordExpiredDate = GenerateNewPasswordExpiredDate();

            if (source.Password == target.Password)
            {
                return new BaseReturnModel(MessageElement.OldPasswordSameAsNewPassword);
            }

            bool isSuccess = _bwUserInfoRep.Value.UpdateByProcedure(target);

            if (!isSuccess)
            {
                return new BaseReturnModel(ReturnCode.UpdateFailed);
            }

            BWOperationLogService.CreateOperationLog(new CreateBWOperationLogByTypeParam
            {
                OperationType = OperationType.ChangePassword,
                Content = DisplayElement.UpdatePassword
            });

            DelayLogoutUser(userId);

            return new BaseReturnModel(ReturnCode.Success);
        }

        public bool IsPasswordExpiredCheckWithCache()
        {
            int userId = EnvLoginUser.LoginUser.UserId;

            return _jxCacheService.Value.GetCache(new SearchCacheParam
            {
                Key = CacheKey.CheckUserPasswordExpiration(userId),
                CacheSeconds = 10 * 60,
            },
            () => new BaseReturnDataModel<bool>(ReturnCode.Success, IsPasswordExpired())).DataModel;
        }

        private bool IsPasswordExpired()
        {
            int userId = EnvLoginUser.LoginUser.UserId;
            BWUserInfo userInfo = GetBWUserInfo(userId);

            return userInfo != null && DateTime.Now > userInfo.PasswordExpiredDate.GetValueOrDefault();
        }

        private DateTime GenerateNewPasswordExpiredDate() => DateTime.Today.AddDays(s_passwordExpiredDays);

        private BWUserInfo GetBWUserInfo(int userId) => _bwUserInfoRep.Value.GetSingleByKey(InlodbType.Inlodb, new BWUserInfo { UserID = userId });

        private void CreateOperationLog(string actionName, string userName)
        {
            CreateOperationLog(actionName, userName, updateContent: string.Empty);
        }

        private void CreateOperationLog(string actionName, string userName, string updateContent)
        {
            string content = string.Format(BWOperationLogElement.UserManagementMessage,
                _permissionKey.Name,
                actionName,
                userName,
                updateContent);

            BWOperationLogService.CreateOperationLog(new CreateBWOperationLogParam
            {
                PermissionKey = _permissionKey,
                Content = content
            });
        }

        private void DelayLogoutUser(int userId)
        {
            //清除快取
            CacheKey cacheKey = CacheKey.BackSideUser(userId);
            _cacheDelayJobService.Value.AddDeleteDelayJobParam(cacheKey, s_delayLogoutSeconds);

            CacheKey checkUserPasswordExpirationCacheKey = CacheKey.CheckUserPasswordExpiration(userId);
            _cacheDelayJobService.Value.AddDeleteDelayJobParam(checkUserPasswordExpirationCacheKey, s_delayLogoutSeconds);

            //發送delay MQ讓前端登出
            _messageQueueDelayJobService.Value.AddDelayJobParam(new BWUserLogoutMessage() { UserID = userId }, s_delayLogoutSeconds);
        }
    }
}