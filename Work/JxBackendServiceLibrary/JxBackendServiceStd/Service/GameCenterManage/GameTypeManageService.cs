using JxBackendService.Common.Util;
using JxBackendService.Interface.Repository;
using JxBackendService.Interface.Service.Game;
using JxBackendService.Model.BackSideWeb;
using JxBackendService.Model.Entity;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Param.Game;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using JxBackendService.Resource.Element;
using System.Collections.Generic;

namespace JxBackendService.Service.GameCenterManage
{
    public class GameTypeManageService : BaseGameCenterManageService, IGameTypeManageService
    {
        private readonly IFrontsideMenuTypeRep _frontsideMenuTypeRep;

        public GameTypeManageService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _frontsideMenuTypeRep = ResolveJxBackendService<IFrontsideMenuTypeRep>();
        }

        protected override BaseReturnDataModel<string> UpdateSingle(GameCenterUpdateParam updateParam)
        {
            FrontsideMenuType originModel = _frontsideMenuTypeRep.GetSingleByKey(InlodbType.Inlodb, new FrontsideMenuType { Id = updateParam.No });

            FrontsideMenuType newModel = originModel.CloneByJson();
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

            FrontsideMenuTypeSetting setting = FrontsideMenuTypeSetting.GetSingle(newModel.Id);
            compareContent = $"{setting.Name}, {compareContent}";

            bool isSuccess = _frontsideMenuTypeRep.UpdateByProcedure(newModel);

            if (!isSuccess)
            {
                return new BaseReturnDataModel<string>(ReturnCode.UpdateFailed);
            }

            return new BaseReturnDataModel<string>(ReturnCode.Success, compareContent);
        }

        private List<RecordCompareParam> GetRecordCompareParams(FrontsideMenuType newModel, FrontsideMenuType originModel)
            => new List<RecordCompareParam>
            {
                new RecordCompareParam
                {
                    Title = DisplayElement.Sort,
                    OriginValue = originModel.Sort.ToString(),
                    NewValue = newModel.Sort.ToString(),
                }
            };
    }
}