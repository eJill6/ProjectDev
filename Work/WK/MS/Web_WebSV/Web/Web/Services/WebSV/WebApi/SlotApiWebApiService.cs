using JxBackendService.Model.ViewModel.ThirdParty;
using SLPolyGame.Web.Interface;
using System.Collections.Generic;
using Web.Services.WebSV.Base;

namespace Web.Services.WebSV.WebApi
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

            return GetHttpGetResponse<List<GameLobbyInfo>>(queryStringParts);
        }

        public string GetJackpotAmount(string gameLobbyType)
        {
            var queryStringMap = new Dictionary<string, string>
            {
                { "gameLobbyType", gameLobbyType },
            };

            string[] queryStringParts = GetQueryStringParts(queryStringMap);

            return GetHttpGetResponseString(queryStringParts);
        }
    }
}