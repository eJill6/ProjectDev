using System.Collections.Generic;
using JxBackendService.Common.Extensions;
using JxBackendService.Common.Util;
using JxBackendService.Interface.Repository.BackSideUser;
using JxBackendService.Interface.Service.BackSideUser;
using JxBackendService.Model.BackSideWeb;
using JxBackendService.Model.Entity.BackSideUser;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.BackSideWeb.Permission;
using JxBackendService.Model.Paging;
using JxBackendService.Model.Param.BackSideWeb;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using JxBackendService.Resource.Element;
using JxBackendService.Service.Base;

namespace JxBackendService.Service.BackSideWeb
{
    public class BWUserInfoService : BaseBackSideService, IBWUserInfoService
    {
        private readonly IBWUserInfoRep _bwUserInfoRep;

        private readonly IBWRoleInfoService _bwRoleInfoService;

        private readonly PermissionKeyDetail _permissionKey = PermissionKeyDetail.UserManagement;

        private static readonly string _editFixedPassword = "$M@kPo#8df";

        public BWUserInfoService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _bwUserInfoRep = ResolveJxBackendService<IBWUserInfoRep>();
            _bwRoleInfoService = ResolveJxBackendService<IBWRoleInfoService>();
        }

        public BaseReturnDataModel<int> Create(CreateBWUserInfoParam createParam)
        {
            if (createParam.RoleID == 0)
            {
                return new BaseReturnDataModel<int>(ReturnCode.DataIsNotCompleted);
            }

            bool isExistUserName = _bwUserInfoRep.IsExistUserName(createParam.UserName);

            if (isExistUserName)
            {
                return new BaseReturnDataModel<int>(ReturnCode.UserNameAlreadyUsed);
            }

            bool isRoleExist = _bwRoleInfoService.IsRoleExist(createParam.RoleID);

            if (!isRoleExist)
            {
                return new BaseReturnDataModel<int>(ReturnCode.RoleIsNotExist);
            }

            BaseReturnDataModel<long> result = _bwUserInfoRep.CreateByProcedure(new BWUserInfo
            {
                UserName = createParam.UserName,
                RoleID = createParam.RoleID,
                Password = createParam.Password.ToPasswordHash(),
            });

            int userId = _bwUserInfoRep.GetUserInfoIdByUsername(createParam.UserName).UserID;

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
            if (updateParam.Password != _editFixedPassword)
            {
                target.Password = updateParam.Password.ToPasswordHash();
            }

            Dictionary<int, string> roleInfoMaps = _bwRoleInfoService.GetAllBWRoleInfoMaps();

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

            bool isSuccess = _bwUserInfoRep.UpdateByProcedure(target);

            if (!isSuccess)
            {
                return new BaseReturnModel(ReturnCode.UpdateFailed);
            }

            CreateOperationLog(PermissionElement.Edit, source.UserName, compareContent);

            return new BaseReturnModel(ReturnCode.Success);
        }

        public BaseReturnModel Delete(int userId)
        {
            BWUserInfo bwUserInfo = GetBWUserInfo(userId);

            if (bwUserInfo == null)
            {
                return new BaseReturnModel(ReturnCode.SearchResultIsEmpty);
            }

            bool isSuccess = _bwUserInfoRep.DeleteByProcedure(bwUserInfo);

            if (!isSuccess)
            {
                return new BaseReturnModel(ReturnCode.OperationFailed);
            }

            CreateOperationLog(PermissionElement.Delete, bwUserInfo.UserName);

            return new BaseReturnModel(ReturnCode.Success);
        }

        public PagedResultModel<UserManagementViewModel> GetPagedBWUserInfos(QueryBWUserInfoParam queryParam)
        {
            PagedResultModel<BWUserInfo> pagedResult = _bwUserInfoRep.GetList(queryParam);

            if (pagedResult.TotalCount == 0)
            {
                return new PagedResultModel<UserManagementViewModel>(queryParam);
            }

            Dictionary<int, string> roleMaps = _bwRoleInfoService.GetAllBWRoleInfoMaps();
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

            bwUserInfo.Password = _editFixedPassword;

            return bwUserInfo.CastByJson<EditUserInfo>();
        }

        private BWUserInfo GetBWUserInfo(int userId) => _bwUserInfoRep.GetSingleByKey(InlodbType.Inlodb, new BWUserInfo { UserID = userId });

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
    }
}