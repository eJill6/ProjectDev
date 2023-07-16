using FluentFTP.Helpers;
using JxBackendService.Common.Extensions;
using JxBackendService.Common.Util;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.BackSideWeb.Permission;
using JxBackendService.Model.Param.BackSideWeb;
using JxBackendService.Model.Param.Game;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using JxBackendService.Service.Base;
using System.Collections.Generic;

namespace JxBackendService.Service.GameCenterManage
{
    public abstract class BaseGameCenterManageService : BaseBackSideService
    {
        public BaseGameCenterManageService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
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

            return result;
        }
    }
}