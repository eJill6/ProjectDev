using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel.Game;
using System.Collections.Generic;

namespace JxBackendService.Interface.Service
{
    public interface IFrontsideMenuTypeService
    {
        List<FrontsideMenuTypeSetting> GetAll();

        List<JxBackendSelectListItem> GetGameTypesSelectListItems();

        List<GameCenterManageModel> GetModels();

        List<JxBackendSelectListItem> GetLiveGameTabTypesSelectListItems(bool hasBlankOption, string defaultValue, string defaultDisplayText);
    }
}