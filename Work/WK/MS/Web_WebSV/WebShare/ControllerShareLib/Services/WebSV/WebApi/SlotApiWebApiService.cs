using ControllerShareLib.Services.WebSV.Base;
using JxBackendService.Model.ViewModel.ThirdParty;
using SLPolyGame.Web.Interface;

namespace ControllerShareLib.Services.WebSV.WebApi
{
    public class SlotApiWebApiService : BaseWebSVService, ISlotApiWebSVService
    {
        protected override string RemoteControllerName => "SlotApiService";

        public List<GameLobbyInfo> GetGameList(string gameLobbyType)
        {
            var queryStringMap = new Dictionary<string, string>
            {
                { "gameLobbyType", gameLobbyType },
            };

            string[] queryStringParts = GetQueryStringParts(queryStringMap);

            return GetHttpGetResponse<List<GameLobbyInfo>>(nameof(GetGameList), queryStringParts);
        }

        public string GetJackpotAmount(string gameLobbyType)
        {
            var queryStringMap = new Dictionary<string, string>
            {
                { "gameLobbyType", gameLobbyType },
            };

            string[] queryStringParts = GetQueryStringParts(queryStringMap);

            return GetHttpGetResponseString(nameof(GetJackpotAmount), queryStringParts);
        }
    }
}