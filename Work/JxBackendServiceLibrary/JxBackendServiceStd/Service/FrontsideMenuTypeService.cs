using JxBackendService.Common.Util;
using JxBackendService.Interface.Repository;
using JxBackendService.Interface.Service;
using JxBackendService.Model.Common;
using JxBackendService.Model.Entity;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.Game;
using JxBackendService.Service.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JxBackendService.Service
{
    public class FrontsideMenuTypeService : BaseService, IFrontsideMenuTypeService
    {
        private readonly IFrontsideMenuTypeRep _frontsideMenuTypeRep;

        public FrontsideMenuTypeService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _frontsideMenuTypeRep = ResolveJxBackendService<IFrontsideMenuTypeRep>();
        }

        public List<FrontsideMenuTypeSetting> GetAll()
        {
            List<FrontsideMenuType> entities = _frontsideMenuTypeRep.GetAll();
            List<FrontsideMenuTypeSetting> frontsideMenuTypeSettings = FrontsideMenuTypeSetting.GetAll();

            return frontsideMenuTypeSettings.OrderBy(setting => entities.Single(entity => entity.Id == setting.Value).Sort).ToList();
        }

        public List<JxBackendSelectListItem> GetGameTypesSelectListItems()
        {
            List<FrontsideMenuTypeSetting> frontsideMenuTypeSettings = GetAll()
                .Where(setting => !setting.IsThirdPartySubGame).ToList();

            return FrontsideMenuTypeSetting.GetSelectListItems(frontsideMenuTypeSettings, hasBlankOption: false);
        }

        public List<GameCenterManageModel> GetModels()
        {
            List<FrontsideMenuType> entities = _frontsideMenuTypeRep.GetAll();
            Dictionary<int, FrontsideMenuType> entitiesMap = entities.ToDictionary(e => e.Id);

            List<FrontsideMenuTypeSetting> frontsideMenuTypeSettings = GetAll();

            return frontsideMenuTypeSettings.Select(setting => {
                if (!entitiesMap.TryGetValue(setting.Value, out var entity))
                {
                    ErrorMsgUtil.ErrorHandle(
                        new InvalidProgramException("在FrontsideMenuType（DB）找不到此设定 FrontsideMenuTypeSetting（程式）： " +
                            $"{setting.ToJsonString()} ")
                        , EnvLoginUser);

                    return null;
                }

                return new GameCenterManageModel
                {
                    No = setting.Value,
                    MenuName = setting.Name,
                    Sort = entity.Sort,
                    IsActive = entity.IsActive,
                };
            }).ToList();
        }
    }
}