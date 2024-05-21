using JxBackendService.Common.Extensions;
using JxBackendService.Common.Util;
using JxBackendService.Interface.Repository;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.Game;
using JxBackendService.Model.BackSideWeb;
using JxBackendService.Model.Entity;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Param.Game;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.Game;
using JxBackendService.Resource.Element;
using System;
using System.Collections.Generic;

namespace JxBackendService.Service.GameCenterManage
{
    public class GameManageService : BaseGameCenterManageService, IGameManageService
    {
        private readonly Lazy<IFrontsideMenuRep> _frontsideMenuRep;

        public GameManageService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _frontsideMenuRep = ResolveJxBackendService<IFrontsideMenuRep>();
        }

        protected override BaseReturnDataModel<string> UpdateSingle(GameCenterUpdateParam updateParam)
        {
            FrontsideMenu originModel = _frontsideMenuRep.Value.GetSingleByKey(InlodbType.Inlodb, new FrontsideMenu { No = updateParam.No });

            FrontsideMenu newModel = originModel.CloneByJson();
            newModel.IsActive = updateParam.IsActive ?? originModel.IsActive;
            newModel.Sort = updateParam.Sort ?? originModel.Sort;

            if (newModel.Sort > GlobalVariables.MaxSortSerialLimit || newModel.Sort < 0)
            {
                return new BaseReturnDataModel<string>(ReturnCode.UpdateFailed);
            }

            string compareContent = GetOperationCompareContent(
                GetRecordCompareParams(newModel, originModel),
                ActTypes.Update);

            if (compareContent.IsNullOrEmpty())
            {
                return new BaseReturnDataModel<string>(ReturnCode.Success);
            }

            QueryFrontsideMenuModel viewModel = newModel.CastByJson<QueryFrontsideMenuModel>();
            compareContent = $"{viewModel.ThirdPartyName}, {compareContent}";

            bool isSuccess = _frontsideMenuRep.Value.UpdateByProcedure(newModel);

            if (!isSuccess)
            {
                return new BaseReturnDataModel<string>(ReturnCode.UpdateFailed);
            }

            return new BaseReturnDataModel<string>(ReturnCode.Success, compareContent);
        }

        private List<RecordCompareParam> GetRecordCompareParams(FrontsideMenu newModel, FrontsideMenu originModel)
            => new List<RecordCompareParam>
            {
                new RecordCompareParam
                {
                    Title = DisplayElement.Sort,
                    OriginValue = originModel.Sort.ToString(),
                    NewValue = newModel.Sort.ToString(),
                },
                new RecordCompareParam
                {
                    Title = DisplayElement.IsActiveStatus,
                    OriginValue = originModel.IsActive.GetActionText(),
                    NewValue = newModel.IsActive.GetActionText(),
                },
            };
    }
}