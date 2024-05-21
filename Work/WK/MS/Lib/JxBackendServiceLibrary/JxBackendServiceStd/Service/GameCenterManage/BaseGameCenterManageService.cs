using FluentFTP.Helpers;
using JxBackendService.Common.Extensions;
using JxBackendService.Common.Util;
using JxBackendService.Interface.Service;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.BackSideWeb.Permission;
using JxBackendService.Model.Param.BackSideWeb;
using JxBackendService.Model.Param.Game;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using JxBackendService.Service.Base;
using System;
using System.Collections.Generic;

namespace JxBackendService.Service.GameCenterManage
{
    public abstract class BaseGameCenterManageService : BaseBackSideService
    {
        private readonly Lazy<IFrontsideMenuService> _frontsideMenuService;

        public BaseGameCenterManageService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _frontsideMenuService = ResolveJxBackendService<IFrontsideMenuService>();
        }

        protected abstract BaseReturnDataModel<string> UpdateSingle(GameCenterUpdateParam updateParam);

        public BaseReturnModel Update(List<GameCenterUpdateParam> updateParams)
        {
            BaseReturnModel result = new BaseReturnModel(ReturnCode.Success);

            List<string> rowsContent = new List<string>();

            foreach (GameCenterUpdateParam updateParam in updateParams)
            {
                BaseReturnDataModel<string> updateSingleResult = UpdateSingle(updateParam);

                if (!updateSingleResult.IsSuccess)
                {
                    result = updateSingleResult;
                }

                string singleRowContent = updateSingleResult.DataModel;

                if (!singleRowContent.IsNullOrEmpty())
                {
                    rowsContent.Add(singleRowContent);
                }
            }

            if (rowsContent.AnyAndNotNull())
            {
                BWOperationLogService.CreateOperationLog(new CreateBWOperationLogParam
                {
                    PermissionKey = PermissionKeyDetail.GameCenterManage,
                    Content = $"{PermissionKeyDetail.GameCenterManage.Name}, {rowsContent.Join("; ")}"
                });
            }

            _frontsideMenuService.Value.ForceRefreshFrontsideMenus();

            return result;
        }
    }
}