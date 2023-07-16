using JxBackendService.Common.Extensions;
using JxBackendService.Common.Util;
using JxBackendService.Common.Util.Cache;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Repository.BackSideUser;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.BackSideUser;
using JxBackendService.Interface.Service.Enums;
using JxBackendService.Model.BackSideWeb;
using JxBackendService.Model.Common;
using JxBackendService.Model.Entity.BackSideUser;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.BackSideWeb.Permission;
using JxBackendService.Model.Paging;
using JxBackendService.Model.Param.BackSideWeb;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using JxBackendService.Resource.Element;
using JxBackendService.Service.Base;
using System.Collections.Generic;
using System.Linq;

namespace JxBackendService.Service.BackSideWeb
{
    public class BWRoleInfoService : BaseBackSideService, IBWRoleInfoService
    {
        private readonly IBWUserInfoRep _bwUserInfoRep;

        private readonly IBWRoleInfoRep _bwRoleInfoRep;

        private readonly IBWRolePermissionRep _bwRolePermissionRep;

        private readonly PermissionKeyDetail _permissionKey = PermissionKeyDetail.RoleManagement;

        private readonly IJxCacheService _jxCacheService;

        private readonly IMenuTypeService _menuTypeService;

        private readonly IPermissionKeyDetailService _permissionKeyDetailService;

        public BWRoleInfoService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _bwRoleInfoRep = ResolveJxBackendService<IBWRoleInfoRep>();
            _bwRolePermissionRep = ResolveJxBackendService<IBWRolePermissionRep>();
            _bwUserInfoRep = ResolveJxBackendService<IBWUserInfoRep>();
            _jxCacheService = DependencyUtil.ResolveServiceForModel<IJxCacheService>(envLoginUser.Application);
            _menuTypeService = DependencyUtil.ResolveService<IMenuTypeService>();
            _permissionKeyDetailService = DependencyUtil.ResolveService<IPermissionKeyDetailService>();
        }

        public EditRolePermission GetEditRoleInfo(int roleId)
        {
            BWRoleInfo roleInfo = _bwRoleInfoRep.GetSingleByKey(InlodbType.Inlodb, new BWRoleInfo { RoleID = roleId });

            if (roleInfo == null)
            {
                return null;
            }

            List<PermissionInfo> allMenuRolePermissionInfos = GetAllRolePermissionInfos();
            List<BWRolePermission> userRolePermission = _bwRolePermissionRep.GetRolePermissionList(roleId);

            List<int> userAuthorityTypes = null;

            foreach (PermissionInfo info in allMenuRolePermissionInfos)
            {
                if (info.PermissionKey == null)
                {
                    continue;
                }

                userAuthorityTypes = userRolePermission
                    .Where(w => w.PermissionKey == info.PermissionKey.Value)
                    .Select(s => s.AuthorityType)
                    .ToList();

                if (!userAuthorityTypes.Any())
                {
                    continue;
                }

                foreach (AuthorityInfo authorityInfo in info.AuthorityInfo)
                {
                    if (userAuthorityTypes.Contains(authorityInfo.AuthorityType))
                    {
                        authorityInfo.IsChecked = true;
                    }
                }
            }

            var editRolePermission = roleInfo.CastByJson<EditRolePermission>();
            editRolePermission.UserRolePermissonInfos = allMenuRolePermissionInfos;

            return editRolePermission;
        }

        private List<PermissionInfo> GetAllRolePermissionInfos()
        {
            List<MenuType> menuTypes = _menuTypeService.GetAll();
            List<PermissionKeyDetail> permissionKeys = _permissionKeyDetailService.GetAll();
            var allPermissionInfos = new List<PermissionInfo>();

            foreach (MenuType root in menuTypes)
            {
                allPermissionInfos.Add(new PermissionInfo { MenuType = root });
                List<PermissionKeyDetail> permissionDetails = permissionKeys.Where(w => w.MenuType == root).ToList();

                foreach (PermissionKeyDetail permissionDetail in permissionDetails)
                {
                    List<AuthorityInfo> authorityInfos = AuthorityTypeDetail.GetAll()
                        .Select(s => new AuthorityInfo()
                        {
                            AuthorityType = s.Value,
                            IsDisplay = permissionDetail.AuthorityTypeDetails.Any(a => a == s)
                        })
                        .ToList();

                    allPermissionInfos.Add(new PermissionInfo
                    {
                        MenuType = root,
                        PermissionKey = permissionDetail,
                        AuthorityInfo = authorityInfos
                    });
                }
            }

            return allPermissionInfos;
        }

        public BaseReturnDataModel<int> Create(CreateRoleInfoParam createParam)
        {
            List<BWRoleInfo> bwRoleInfos = _bwRoleInfoRep.GetRoleInfoByRoleName(createParam.RoleName);

            if (bwRoleInfos.Any())
            {
                return new BaseReturnDataModel<int>(ReturnCode.SomeDataTypeIsExists, createParam.RoleName);
            }

            var bwRoleInfo = new BWRoleInfo
            {
                RoleName = createParam.RoleName,
            };

            string content = GetOperationCompareContent(new List<RecordCompareParam> {
                new RecordCompareParam{
                    Title = DisplayElement.RoleName,
                    NewValue = bwRoleInfo.RoleName
                }
            }, ActTypes.Insert);

            CreateOperationLog(PermissionElement.Insert, content);

            return _bwRoleInfoRep.CreateByProcedure(bwRoleInfo).CastByJson<BaseReturnDataModel<int>>();
        }

        public BaseReturnModel Delete(int roleId)
        {
            BWRoleInfo bwRoleInfo = _bwRoleInfoRep.GetSingleByKey(InlodbType.Inlodb, new BWRoleInfo { RoleID = roleId });

            if (bwRoleInfo == null)
            {
                return new BaseReturnModel(ReturnCode.SearchResultIsEmpty);
            }

            bool isExistRoleId = _bwUserInfoRep.IsExistRoleId(roleId);

            if (isExistRoleId)
            {
                return new BaseReturnModel(ReturnCode.NoDeletionForBWRole);
            }

            bool isSuccess = _bwRoleInfoRep.DeleteByProcedure(bwRoleInfo);

            if (!isSuccess)
            {
                return new BaseReturnModel(ReturnCode.OperationFailed);
            }

            string content = GetOperationCompareContent(new List<RecordCompareParam> {
                new RecordCompareParam{
                    Title = DisplayElement.RoleName,
                    NewValue = bwRoleInfo.RoleName
                }
            }, ActTypes.Delete);

            CreateOperationLog(PermissionElement.Delete, content);

            return new BaseReturnModel(ReturnCode.Success);
        }

        public BaseReturnModel SaveRolePermission(UpdateRolePermissionParam updateParam)
        {
            int roleID = updateParam.RoleID;
            string oldRoleName = GetEditRoleInfo(roleID).RoleName;
            string newRoleName = updateParam.RoleName;

            List<RolePermissionInfo> oldPermissionInfos = _bwRolePermissionRep.GetRolePermissionList(roleID).CastByJson<List<RolePermissionInfo>>().OrderBy(x => x.PermissionKey).ToList();
            List<RolePermissionInfo> newPermissionInfos = updateParam.PermissionKeys.OrderBy(x => x.PermissionKey).ToList();

            bool isChangeRoleName = oldRoleName != newRoleName;

            List<RecordCompareParam> recordCompareParams = ConvertToRecordCompareParams(oldPermissionInfos, newPermissionInfos);
            bool isChangeRolePermission = recordCompareParams.Any(x => x.OriginValue != x.NewValue);

            //沒異動角色名稱與角色權限，就不用寫操作紀錄
            if (!isChangeRoleName && !isChangeRolePermission)
            {
                return new BaseReturnModel(ReturnCode.Success);
            }

            //有異動角色名稱才要更新
            if (isChangeRoleName)
            {
                BaseReturnModel updateRoleResult = Update(roleID, newRoleName);

                if (!updateRoleResult.IsSuccess)
                {
                    return updateRoleResult;
                }

                recordCompareParams.Insert(
                    index: 0,
                    new RecordCompareParam
                    {
                        Title = DisplayElement.RoleName,
                        OriginValue = oldRoleName,
                        NewValue = newRoleName
                    });
            }

            //有異動角色權限才要更新
            if (isChangeRolePermission)
            {
                string newPermissionInfoJsonString = newPermissionInfos.ToJsonString();

                var saveParam = new ProSaveBWRolePermission
                {
                    RoleID = roleID,
                    CreateUser = EnvLoginUser.LoginUser.UserId.ToString(),
                    PermissionAuthTypeJson = newPermissionInfoJsonString
                };

                BaseReturnModel updatePermissionsResult = _bwRolePermissionRep.SaveBWRolePermission(saveParam);

                if (!updatePermissionsResult.IsSuccess)
                {
                    return updatePermissionsResult;
                }
            }

            string operationContent = GetOperationCompareContent(recordCompareParams, ActTypes.Update);
            CreateOperationLog(PermissionElement.Edit, newRoleName, operationContent);

            return new BaseReturnModel(ReturnCode.Success);
        }

        private List<RecordCompareParam> ConvertToRecordCompareParams(List<RolePermissionInfo> oldPermissionInfos, List<RolePermissionInfo> newPermissionInfos)
        {
            var allPermissionInfos = new List<RolePermissionInfo>();
            allPermissionInfos.AddRange(oldPermissionInfos);
            allPermissionInfos.AddRange(newPermissionInfos);

            if (!allPermissionInfos.Any())
            {
                return new List<RecordCompareParam>();
            }

            Dictionary<PermissionKeyDetail, string> oldPermissionMaps = GetPermissionMap(allPermissionInfos, oldPermissionInfos);
            Dictionary<PermissionKeyDetail, string> newPermissionMaps = GetPermissionMap(allPermissionInfos, newPermissionInfos);
            List<RecordCompareParam> recordCompareParam = new List<RecordCompareParam>();

            foreach (KeyValuePair<PermissionKeyDetail, string> info in oldPermissionMaps)
            {
                oldPermissionMaps.TryGetValue(info.Key, out string oldPermission);
                newPermissionMaps.TryGetValue(info.Key, out string newPermission);

                recordCompareParam.Add(new RecordCompareParam
                {
                    Title = $"【{info.Key.Name}】",
                    OriginValue = oldPermission,
                    NewValue = newPermission
                });
            }

            return recordCompareParam;
        }

        public Dictionary<int, string> GetAllBWRoleInfoMaps()
        {
            List<BWRoleInfo> allRoleInfos = _jxCacheService.GetCache(
                new SearchCacheParam
                {
                    Key = CacheKey.BackSideUserRole(),
                    CacheSeconds = 60,
                    IsCloneInstance = false,
                },
                () => _bwRoleInfoRep.GetAllBWRoleInfos());

            return allRoleInfos.ToDictionary(d => d.RoleID, d => d.RoleName);
        }

        public bool IsRoleExist(int roleId) => GetAllBWRoleInfoMaps().ContainsKey(roleId);

        public List<JxBackendSelectListItem> GetRoleSelectListItems()
        {
            Dictionary<int, string> allRoleInfos = GetAllBWRoleInfoMaps();

            var selectListItems = allRoleInfos.Select(s => new JxBackendSelectListItem
            {
                Value = s.Key.ToString(),
                Text = s.Value,
            }).ToList();

            selectListItems.AddBlankOption(hasBlankOption: true, defaultDisplayText: SelectItemElement.PlzChoice, defaultValue: null);

            return selectListItems;
        }

        private BaseReturnModel Update(int roleID, string roleName)
        {
            BWRoleInfo bwRoleInfo = _bwRoleInfoRep.GetSingleByKey(InlodbType.Inlodb, new BWRoleInfo { RoleID = roleID });

            if (bwRoleInfo == null)
            {
                return new BaseReturnModel(ReturnCode.SearchResultIsEmpty);
            }

            // 排除更新本身的roleId
            List<BWRoleInfo> bwRoleInfos = _bwRoleInfoRep.GetRoleInfoByRoleName(roleName).Where(w => w.RoleID != roleID).ToList();

            if (bwRoleInfos.Any())
            {
                return new BaseReturnModel(ReturnCode.SomeDataTypeIsExists, roleName);
            }

            bwRoleInfo.RoleName = roleName;

            bool isSuccess = _bwRoleInfoRep.UpdateByProcedure(bwRoleInfo);

            if (!isSuccess)
            {
                return new BaseReturnModel(ReturnCode.UpdateFailed);
            }

            return new BaseReturnModel(ReturnCode.Success);
        }

        public PagedResultModel<RoleManagementViewModel> GetPagedBWRoleInfos(QueryBWRoleInfoParam queryParam)
        {
            PagedResultModel<BWRoleInfo> pagedResult = _bwRoleInfoRep.GetList(queryParam);

            if (pagedResult.TotalCount == 0)
            {
                return new PagedResultModel<RoleManagementViewModel>();
            }

            var resultModel = pagedResult.CastByJson<PagedResultModel<RoleManagementViewModel>>();

            return resultModel;
        }

        public Dictionary<string, HashSet<int>> GetUserRolePermissions(int userId)
        {
            BWUserInfo bwUserInfo = _bwUserInfoRep.GetSingleByKey(InlodbType.Inlodb, new BWUserInfo { UserID = userId });
            List<BWRolePermission> userRolePermission = _bwRolePermissionRep.GetRolePermissionList(bwUserInfo.RoleID);

            Dictionary<string, HashSet<int>> permissionMaps = userRolePermission.
                Select(s => s.PermissionKey).
                Distinct().
                ToDictionary(d => d, d => new HashSet<int>());

            foreach (string permissionKey in permissionMaps.Keys.ToList())
            {
                permissionMaps[permissionKey] = userRolePermission.
                    Where(w => w.PermissionKey == permissionKey).
                    Select(s => s.AuthorityType).ConvertToHashSet();
            }

            return permissionMaps;
        }

        private void CreateOperationLog(string actionName, string roleName)
        {
            CreateOperationLog(actionName, roleName, permissionContent: string.Empty);
        }

        private void CreateOperationLog(string actionName, string roleName, string permissionContent)
        {
            string content = string.Format(BWOperationLogElement.RoleManagementMessage,
                _permissionKey.Name,
                actionName,
                roleName,
                permissionContent);

            BWOperationLogService.CreateOperationLog(new CreateBWOperationLogParam
            {
                PermissionKey = _permissionKey,
                Content = content
            });
        }

        private Dictionary<PermissionKeyDetail, string> GetPermissionMap(List<RolePermissionInfo> allPermissionInfos, List<RolePermissionInfo> currentPermissionInfos)
        {
            Dictionary<PermissionKeyDetail, string> permissionMap = allPermissionInfos
                .Select(s => _permissionKeyDetailService.GetSingle(s.PermissionKey))
                .Where(w => w != null)
                .Distinct()
                .ToDictionary(d => d, d => string.Empty);

            foreach (PermissionKeyDetail permissionKey in permissionMap.Keys.ToList())
            {
                List<string> authorityTypeDetailNames = currentPermissionInfos.Where(w => w.PermissionKey == permissionKey.Value)
                    .Select(s => AuthorityTypeDetail.GetName(s.AuthorityType)).ToList();

                string authorityTypeText = string.Join(", ", authorityTypeDetailNames);
                permissionMap[permissionKey] = authorityTypeText.IsNullOrEmpty() ? MessageElement.NoPermission : authorityTypeText;
            }

            return permissionMap;
        }
    }
}