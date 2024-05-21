using ControllerShareLib.Interfaces.Service.Controller;
using ControllerShareLib.Models.Game;
using ControllerShareLib.Models.Game.GameLobby;
using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Param.ThirdParty;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel.ThirdParty;
using SLPolyGame.Web.Interface;

namespace ControllerShareLib.Service.Controller
{
    public class GameLobbyControllerService : IGameLobbyControllerService
    {
        private static readonly int s_gamePageSize = 20;

        private readonly Lazy<ISlotApiWebSVService> _slotApiWebSVService;

        private readonly Lazy<IGameCenterControllerService> _gameCenterControllerService;

        public GameLobbyControllerService()
        {
            _slotApiWebSVService = DependencyUtil.ResolveService<ISlotApiWebSVService>();
            _gameCenterControllerService = DependencyUtil.ResolveService<IGameCenterControllerService>();
        }

        public WebThirdPartyGamesViewModel GetWebThirdPartyGamesViewModel(GameLobbySubGameRequest request)
        {
            GameLobbyType gameLobbyType = GameLobbyType.GetSingle(request.GameLobbyTypeValue);

            return GetSlotSubGameList(request, gameLobbyType);
        }

        public MobileApiThirdPartyGamesViewModel GetMobileApiThirdPartyGamesViewModel(GameLobbySubGameRequest request)
        {
            GameLobbyType gameLobbyType = GameLobbyType.GetSingle(request.GameLobbyTypeValue);

            WebThirdPartyGamesViewModel webThirdPartyGamesViewModel = GetSlotSubGameList(request, gameLobbyType);

            return new MobileApiThirdPartyGamesViewModel()
            {
                IsSquareGameImage = webThirdPartyGamesViewModel.IsSquareGameImage,
                GameLobbyInfos = webThirdPartyGamesViewModel.GameLobbyInfos,
            };
        }

        public string? GetJackpotAmount(GameLobbyType gameLobbyType)
        {
            if (gameLobbyType.IsLobbyShowJackpot)
            {
                return _slotApiWebSVService.Value.GetJackpotAmount(gameLobbyType.Value);
            }

            return null;
        }

        public BaseReturnDataModel<AppOpenUrlInfo> LoginGameLobbyGame(LoginGameLobbyGameParam param)
        {
            return DoLogin(GameLobbyType.GetSingle(param.GameLobbyTypeValue).Product, param.MobileGameCode);
        }

        private BaseReturnDataModel<AppOpenUrlInfo> DoLogin(PlatformProduct platformProduct, string mobileGameCode)
        {
            var baseGameCenterLogin = new BaseGameCenterLogin()
            {
                ProductCode = platformProduct.Value,
                RemoteCode = mobileGameCode
            };

            BaseReturnDataModel<CommonOpenUrlInfo> baseReturnDataModel = _gameCenterControllerService.Value.GetWebForwardGameUrl(baseGameCenterLogin);

            return baseReturnDataModel.CastByJson<BaseReturnDataModel<AppOpenUrlInfo>>();
        }

        private WebThirdPartyGamesViewModel GetSlotSubGameList(SubGameRequest request, GameLobbyType gameLobbyType)
        {
            var slotSubGames = new List<GameLobbyInfo>();

            if (gameLobbyType != null)
            {
                slotSubGames = _slotApiWebSVService.Value.GetGameList(gameLobbyType.Value);
            }

            WebThirdPartyGamesViewModel viewModel = GamesListToViewModel(slotSubGames, request, gameLobbyType);

            return viewModel;
        }

        private WebThirdPartyGamesViewModel GamesListToViewModel(List<GameLobbyInfo> gameLobbyInfos, SubGameRequest request, GameLobbyType gameLobbyType)
        {
            if (request.GameTabType == GameTabTypeValue.Hot)
            {
                gameLobbyInfos = gameLobbyInfos.FindAll(game => game.IsHot);
            }
            else if (request.GameTabType == GameTabTypeValue.Favorite)
            {
                if (request.FilterNos != null)
                {
                    HashSet<int> filterSet = request.FilterNos.ToHashSet();
                    gameLobbyInfos = gameLobbyInfos.FindAll(game => filterSet.Contains(game.No));
                }
                else
                {
                    gameLobbyInfos.Clear();
                }
            }

            if (!request.SearchGameName.IsNullOrEmpty())
            {
                gameLobbyInfos = gameLobbyInfos.FindAll(game => game.ChineseName.Contains(request.SearchGameName));
            }

            // TODO 舊邏輯 待優化
            gameLobbyInfos = gameLobbyInfos.FindAll(game => !string.IsNullOrEmpty(game.MobileGameCode));
            int skip = 0;

            if (request.LastNo.HasValue)
            {
                for (int i = 0; i < gameLobbyInfos.Count; i++)
                {
                    if (gameLobbyInfos[i].No == request.LastNo.Value)
                    {
                        skip = i + 1;

                        break;
                    }
                }
            }

            List<GameLobbyInfo> paginationGames = gameLobbyInfos.Skip(skip).Take(s_gamePageSize).ToList();

            return new WebThirdPartyGamesViewModel
            {
                GameLobbyInfos = paginationGames,
                GameTabType = request.GameTabType,
                SearchGameName = request.SearchGameName,
                IsSelfOpenPage = gameLobbyType.IsSelfOpenPage,
                IsSquareGameImage = gameLobbyType.IsSquareGameImage,
            };
        }
    }
}