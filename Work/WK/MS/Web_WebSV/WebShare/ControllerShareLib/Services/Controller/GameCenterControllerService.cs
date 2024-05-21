using ControllerShareLib.Interfaces.Service.Controller;
using ControllerShareLib.Models.Game;
using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.Enums;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.MiseOrder;
using JxBackendService.Model.Enums.ThirdParty;
using JxBackendService.Model.Param.ThirdParty;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel.Menu;
using JxBackendService.Resource.Element;
using SLPolyGame.Web.Interface;

namespace ControllerShareLib.Service.Controller
{
    public class GameCenterControllerService : IGameCenterControllerService
    {
        private static readonly bool s_isMobile = true;

        private readonly Lazy<ISLPolyGameWebSVService> _slPolyGameWebSVService;

        private readonly Lazy<IThirdPartyApiWebSVService> _thirdPartyApiWebSVService;

        private readonly Lazy<IEnvironmentService> _environmentService;

        public GameCenterControllerService()
        {
            _slPolyGameWebSVService = DependencyUtil.ResolveService<ISLPolyGameWebSVService>();
            _thirdPartyApiWebSVService = DependencyUtil.ResolveService<IThirdPartyApiWebSVService>();
            _environmentService = DependencyUtil.ResolveService<IEnvironmentService>();
        }

        public BaseReturnDataModel<MobileApiEnterTPGameUrlInfo> GetMobileApiEnterThirdPartyGame(EnterThirdPartyGameParam param)
        {
            BaseReturnDataModel<EnterTPGameUrlInfo> baseReturnModel = GetWebEnterThirdPartyGame(param);
            var mobileReturnDataModel = baseReturnModel.CastByJson<BaseReturnDataModel<MobileApiEnterTPGameUrlInfo>>();

            if (!mobileReturnDataModel.IsSuccess)
            {
                return mobileReturnDataModel;
            }

            if (!mobileReturnDataModel.DataModel.GameLobbyTypeValue.IsNullOrEmpty())
            {
                mobileReturnDataModel.DataModel.Url = null;
            }

            return mobileReturnDataModel;
        }

        public BaseReturnDataModel<EnterTPGameUrlInfo> GetWebEnterThirdPartyGame(EnterThirdPartyGameParam param)
        {
            var orderGameId = MiseOrderGameId.GetSingle(param.GameId);

            if (orderGameId == null)
            {
                return new BaseReturnDataModel<EnterTPGameUrlInfo>($"MiseOrderGameId {param.GameId} not found");
            }

            WebGameCenterViewModel webGameCenterViewModel = _slPolyGameWebSVService.Value.GetWebGameCenterViewModel().ConfigureAwait(false).GetAwaiter().GetResult();
            FrontsideProductMenu? frontsideProductMenu = FindFrontsideProductMenu(webGameCenterViewModel.WebMenuTypeViewModels, orderGameId, param.RemoteCode);

            if (!param.RemoteCode.IsNullOrEmpty())
            {
                var baseGameCenterLogin = new BaseGameCenterLogin
                {
                    ProductCode = orderGameId.Product.Value,
                    GameCode = orderGameId.SubGameCode,
                    RemoteCode = param.RemoteCode,
                };

                BaseReturnDataModel<EnterTPGameUrlInfo> dataModelByRemoteCode = GetWebForwardGameUrl(baseGameCenterLogin).CastByJson<BaseReturnDataModel<EnterTPGameUrlInfo>>();
                MergeTitle(dataModelByRemoteCode.DataModel, frontsideProductMenu);

                return dataModelByRemoteCode;
            }

            //處理子遊戲大廳
            if (frontsideProductMenu != null && !frontsideProductMenu.GameLobbyTypeValue.IsNullOrEmpty())
            {
                return new BaseReturnDataModel<EnterTPGameUrlInfo>(
                    ReturnCode.Success,
                    new EnterTPGameUrlInfo()
                    {
                        GameLobbyTypeValue = frontsideProductMenu.GameLobbyTypeValue,
                        Url = frontsideProductMenu.GameLobbyUrl,
                        IsHideHeaderWithFullScreen = frontsideProductMenu.IsHideHeaderWithFullScreen,
                        OpenGameModeValue = OpenGameMode.Redirect.Value,
                        Title = frontsideProductMenu.Title
                    });
            }

            BaseReturnDataModel<EnterTPGameUrlInfo> dataModelWithoutRemoteCode = GetWebForwardGameUrl(
                new BaseGameCenterLogin
                {
                    ProductCode = orderGameId.Product.Value,
                    GameCode = orderGameId.SubGameCode,
                }).CastByJson<BaseReturnDataModel<EnterTPGameUrlInfo>>();

            MergeTitle(dataModelWithoutRemoteCode.DataModel, frontsideProductMenu);

            return dataModelWithoutRemoteCode;
        }

        public BaseReturnDataModel<AppOpenUrlInfo> GetMobileApiForwardGameUrl(BaseGameCenterLogin baseGameCenterLogin)
        {
            return GetWebForwardGameUrl(baseGameCenterLogin).CastByJson<BaseReturnDataModel<AppOpenUrlInfo>>();
        }

        public BaseReturnDataModel<CommonOpenUrlInfo> GetWebForwardGameUrl(BaseGameCenterLogin baseGameCenterLogin)
        {
            if (baseGameCenterLogin.GameCode.IsNullOrEmpty() && !baseGameCenterLogin.RemoteCode.IsNullOrEmpty())
            {
                ThirdPartySubGameCodes? thirdPartySubGameCodes = ThirdPartySubGameCodes.GetAll()
                    .SingleOrDefault(s => s.PlatformProduct.Value == baseGameCenterLogin.ProductCode && s.RemoteGameCode == baseGameCenterLogin.RemoteCode);

                if (thirdPartySubGameCodes != null)
                {
                    baseGameCenterLogin.GameCode = thirdPartySubGameCodes.Value;
                }
            }

            //檢查menu是否開啟
            var frontSideMainMenu = new FrontSideMainMenu()
            {
                ProductCode = baseGameCenterLogin.ProductCode,
                GameCode = baseGameCenterLogin.GameCode
            };

            if (!_slPolyGameWebSVService.Value.IsFrontsideMenuActive(frontSideMainMenu).ConfigureAwait(false).GetAwaiter().GetResult())
            {
                return new BaseReturnDataModel<CommonOpenUrlInfo>(ThirdPartyGameElement.GameMaintain);
            }

            string loginInfoJson = new LoginInfo()
            {
                GameCode = baseGameCenterLogin.GameCode,
                RemoteCode = baseGameCenterLogin.RemoteCode
            }
            .ToJsonString();

            //依照Code去取得登入網址
            BaseReturnDataModel<TPGameOpenParam> result = _thirdPartyApiWebSVService.Value.GetForwardGameUrl(
                new SLPolyGame.Web.Model.ForwardGameUrlSVApiParam()
                {
                    ProductCode = baseGameCenterLogin.ProductCode,
                    LoginInfoJson = loginInfoJson,
                    IsMobile = s_isMobile,
                    CorrelationId = Guid.NewGuid().ToString()
                });

            if (!result.IsSuccess)
            {
                return new BaseReturnDataModel<CommonOpenUrlInfo>(result.Message);
            }

            var platformProductService = DependencyUtil.ResolveKeyed<IPlatformProductService>(
                _environmentService.Value.Application,
                SharedAppSettings.PlatformMerchant).Value;

            PlatformProduct product = platformProductService.GetSingle(baseGameCenterLogin.ProductCode);
            bool isHideHeaderWithFullScreen = product.ProductType.IsHideHeaderWithFullScreen;

            var commonOpenUrlInfo = new CommonOpenUrlInfo()
            {
                Url = result.DataModel.Url,
                IsHideHeaderWithFullScreen = isHideHeaderWithFullScreen,
                OpenGameModeValue = result.DataModel.OpenGameModeValue,
            };

            return new BaseReturnDataModel<CommonOpenUrlInfo>(ReturnCode.Success, commonOpenUrlInfo);
        }

        private FrontsideProductMenu? FindFrontsideProductMenu(List<WebMenuTypeViewModel> webMenuTypeViewModels, MiseOrderGameId orderGameId, string? remoteCode)
        {
            foreach (WebMenuTypeViewModel? typeViewModel in webMenuTypeViewModels)
            {
                FrontsideProductMenu? frontsideProductMenu = typeViewModel.FrontsideProductMenus.SingleOrDefault(menu =>
                    menu.ProductCode == orderGameId.Product.Value &&
                    menu.GameCode == orderGameId.SubGameCode &&
                    menu.RemoteCode.ToTrimString() == remoteCode.ToTrimString());

                if (frontsideProductMenu != null)
                {
                    return frontsideProductMenu;
                }
            }

            return null;
        }

        private void MergeTitle(EnterTPGameUrlInfo enterTPGameUrlInfo, FrontsideProductMenu? frontsideProductMenu)
        {
            if (enterTPGameUrlInfo != null && frontsideProductMenu != null)
            {
                enterTPGameUrlInfo.Title = frontsideProductMenu.Title;
            }
        }
    }
}