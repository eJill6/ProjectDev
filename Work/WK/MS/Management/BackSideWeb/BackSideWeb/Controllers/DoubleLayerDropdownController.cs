using BackSideWeb.Controllers.Base;
using BackSideWeb.Models;
using Castle.Core.Internal;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.Enums;
using JxBackendService.Interface.Service.Game;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.ThirdParty;
using JxBackendService.Model.Param.Game;
using JxBackendService.Model.ViewModel.Game;
using JxBackendService.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BackSideWeb.Controllers
{
    public class DoubleLayerDropdownController : BaseController
    {
        private readonly Lazy<IPlatformProductService> _platformProductService;
        private readonly Lazy<IFrontsideMenuService> _frontsideMenuService;

        public DoubleLayerDropdownController()
        {
            _platformProductService = DependencyUtil.ResolveKeyed<IPlatformProductService>(Application, EnvLoginUser.PlatformMerchant);
            _frontsideMenuService = DependencyUtil.ResolveJxBackendService<IFrontsideMenuService>(EnvLoginUser, DbConnectionTypes.Slave);
        }

        public IActionResult GetHotGameSubGamesDropdown(string productCode, DropdownMenuSetting dropdownMenuSetting)
        {
            PlatformProduct product = PlatformProduct.GetSingle(productCode);

            var hotGameManageReadService = DependencyUtil.ResolveJxBackendService<IHotGameManageReadService>(EnvLoginUser, DbConnectionTypes.Slave).Value;
            List<JxBackendSelectListItem> selectListItems = hotGameManageReadService.GetHotGameSubGames(product);

            return GetDropdownPartial(selectListItems, dropdownMenuSetting);
        }

        public IActionResult GetGameDropdown(int liveGameManageDataTypeValue, DropdownMenuSetting dropdownMenuSetting)
        {
            LiveGameManageDataType dataType = LiveGameManageDataType.GetSingle(liveGameManageDataTypeValue);
            List<JxBackendSelectListItem> selectListItems;

            if (dataType == LiveGameManageDataType.GameCenter)
            {
                selectListItems = _platformProductService.Value.GetContractSelectListItems(hasBlankOption: true, isSupportHotGame: null);
            }
            else if (dataType == LiveGameManageDataType.DirectPlay)
            {
                selectListItems = _platformProductService.Value.GetContractSelectListItems(hasBlankOption: true, isSupportHotGame: true);
            }
            else
            {
                throw new NotSupportedException();
            }

            return GetDropdownPartial(selectListItems, dropdownMenuSetting);
        }

        public IActionResult GetSubGameDropdown(int liveGameManageDataTypeValue, string productCode, DropdownMenuSetting dropdownMenuSetting)
        {
            LiveGameManageDataType dataType = LiveGameManageDataType.GetSingle(liveGameManageDataTypeValue);
            PlatformProduct product = PlatformProduct.GetSingle(productCode);
            var selectListItems = new List<JxBackendSelectListItem>();

            if (dataType == LiveGameManageDataType.DirectPlay)
            {
                selectListItems = ThirdPartySubGameCodes.GetSelectListItems(
                    product,
                    hasBlankOption: false,
                    isSubGameOptionOfHotGameVislble: true);

            }
            else if (dataType == LiveGameManageDataType.GameCenter)
            {
                List<GameCenterManageDetail> gameCenterManageDetails = _frontsideMenuService.Value.GetAllByProduct(product);
                //超過一個或只有一個並且gamecode不為空才給選項
                if (gameCenterManageDetails.Count > 1 || 
                    (gameCenterManageDetails.Count == 1 && !gameCenterManageDetails.First().GameCode.IsNullOrEmpty()))
                {
                    selectListItems = gameCenterManageDetails
                        .Select(s => new JxBackendSelectListItem() { Value = s.GameCode, Text = s.MenuName })
                        .ToList();
                }                
            }
            else
            {
                throw new NotSupportedException();
            }

            return GetDropdownPartial(selectListItems, dropdownMenuSetting);
        }

        private IActionResult GetDropdownPartial(List<JxBackendSelectListItem> selectListItems, DropdownMenuSetting? dropdownMenuSetting = null)
        {
            if (selectListItems == null || selectListItems.Count <= 0)
            {
                return Content(string.Empty);
            }

            DropdownMenuSetting viewModel = new DropdownMenuSetting(selectListItems)
            {
                SettingId = dropdownMenuSetting?.SettingId,
                Callback = dropdownMenuSetting?.Callback,
            };

            return PartialView("/Views/Shared/Partial/_DropdownMenu.cshtml", viewModel);
        }
    }
}