using ControllerShareLib.Services.WebSV.Base;
using JxBackendService.Common.Util;
using JxBackendService.Model.Param.ThirdParty;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.Menu;
using SLPolyGame.Web.Interface;
using SLPolyGame.Web.Model;
using System.Web;

namespace ControllerShareLib.Services.WebSV.WebApi
{
    public class SLPolyGameWebApiService : BaseWebSVService, ISLPolyGameWebSVService
    {
        protected override string RemoteControllerName => "SLPolyGameService";

        public async Task<string> CancelOrder(PalyInfo model)
        {
            return await Task.FromResult(GetHttpPostResponseString(nameof(CancelOrder), model));
        }

        public async Task<CursorPagination<CurrentLotteryInfo>> GetCursorPaginationDrawResult(int lotteryId, DateTime start, DateTime end, int count, string cursor)
        {
            var queryStringMap = new Dictionary<string, string>
            {
                { "lotteryId", lotteryId.ToString() },
                { "start", start.ToFormatDateTimeMillisecondsString() },
                { "end", end.ToFormatDateTimeMillisecondsString() },
                { "count", count.ToString() },
                { "cursor", cursor } ,
            };

            string[] queryStringParts = GetQueryStringParts(queryStringMap);

            return await Task.FromResult(GetHttpGetResponse<CursorPagination<CurrentLotteryInfo>>(nameof(GetCursorPaginationDrawResult), queryStringParts));
        }

        public async Task<CursorPagination<PalyInfo>> GetFollowBet(string palyId, int lottertId)
        {
            var queryStringMap = new Dictionary<string, string>
            {
                { "palyId", palyId },
                { "lottertId", lottertId.ToString() },
            };

            string[] queryStringParts = GetQueryStringParts(queryStringMap);

            return await Task.FromResult(GetHttpGetResponse<CursorPagination<PalyInfo>>(nameof(GetFollowBet), queryStringParts));
        }

        public async Task<List<MenuInnerInfo>> GetMenuInnerInfos()
        {
            return await Task.FromResult(GetHttpGetResponse<List<MenuInnerInfo>>(nameof(GetMenuInnerInfos)));
        }

        public async Task<WebGameCenterViewModel> GetWebGameCenterViewModel()
        {
            return await Task.FromResult(GetHttpGetResponse<WebGameCenterViewModel>(nameof(GetWebGameCenterViewModel)));
        }

        //public async Task<WebGameCenterViewModel> GetWebGameCenterViewModelAsync()
        //{
        //    ResponseInfo responseInfo = await GetHttpGetResponseStringAsync(nameof(GetWebGameCenterViewModel));
        //    return responseInfo.Content.Deserialize<WebGameCenterViewModel>();
        //}

        public async Task<List<string>> GetLatestWinningList(string period)
        {
            return await Task.FromResult(GetHttpGetResponse<List<string>>(nameof(GetLatestWinningList), $"{nameof(period)}={HttpUtility.UrlEncode(period)}"));
        }

        public async Task<List<WinningListItem>> GetLatestWinningListItems(string period)
        {
            return await Task.FromResult(GetHttpGetResponse<List<WinningListItem>>(nameof(GetLatestWinningListItems), $"{nameof(period)}={HttpUtility.UrlEncode(period)}"));
        }

        public async Task<PalyInfo> GetPalyIDPalyBet(string value)
        {
            return await Task.FromResult(GetHttpGetResponse<PalyInfo>(nameof(GetPalyIDPalyBet), $"{nameof(value)}={HttpUtility.UrlEncode(value)}"));
        }

        public async Task<PalyInfo> GetPlayBetByAnonymous(string playId)
        {
            return await Task.FromResult(GetHttpGetResponse<PalyInfo>(nameof(GetPlayBetByAnonymous), $"{nameof(playId)}={HttpUtility.UrlEncode(playId)}"));
        }

        public async Task<PalyInfo[]> GetPlayBetsByAnonymous(string startTime, string endTime, string gameId)
        {
            var queryStringMap = new Dictionary<string, string>
            {
                { "startTime", startTime },
                { "endTime", endTime },
                { "gameId", gameId },
            };

            string[] queryStringParts = GetQueryStringParts(queryStringMap);

            return await Task.FromResult(GetHttpGetResponse<PalyInfo[]>(nameof(GetPlayBetsByAnonymous), queryStringParts));
        }

        public async Task<DateTime> GetServerCurrentTime()
        {
            return await Task.FromResult(GetHttpGetResponse<DateTime>(nameof(GetServerCurrentTime)));
        }

        public async Task<CursorPaginationTotalData<PalyInfo>> GetSpecifyOrderList(int userId, int? lotteryId, string status, DateTime searchDate, string cursor, int pageSize, string roomId)
        {
            var queryStringMap = new Dictionary<string, string>
            {
                { "userId", userId.ToString() },
                { "status", status },
                { "searchDate", searchDate.ToFormatDateTimeMillisecondsString() },
                { "cursor", cursor },
                { "pageSize", pageSize.ToString() },
            };

            if (lotteryId.HasValue)
            {
                queryStringMap.Add("lotteryId", lotteryId.Value.ToString());
            }

            if (!string.IsNullOrWhiteSpace(roomId))
            {
                queryStringMap.Add("roomId", roomId);
            }

            string[] queryStringParts = GetQueryStringParts(queryStringMap);

            return await GetHttpGetResponseStringAsync<CursorPaginationTotalData<PalyInfo>>(nameof(GetSpecifyOrderList), queryStringParts);
        }

        public async Task<string> GetStatus()
        {
            return await Task.FromResult(GetHttpGetResponseString(nameof(GetStatus)));
        }

        public async Task<SysSettings> GetSysSettings()
        {
            return await Task.FromResult(GetHttpGetResponse<SysSettings>(nameof(GetSysSettings)));
        }

        public async Task<UserInfo> GetUserInfo()
        {
            return await Task.FromResult(GetHttpGetResponse<UserInfo>(nameof(GetUserInfo)));
        }

        public async Task<UserInfo> GetUserInfoByUserID(int UserID)
        {
            return await Task.FromResult(GetHttpGetResponse<UserInfo>(nameof(GetUserInfoByUserID), $"{nameof(UserID)}={HttpUtility.UrlEncode(UserID.ToString())}"));
        }

        public async Task<PalyInfo> InsertPlayInfo(PalyInfo model)
        {
            return await Task.FromResult(GetHttpPostResponse<PalyInfo>(nameof(InsertPlayInfo), model));
        }

        public async Task<bool> IsFrontsideMenuActive(FrontSideMainMenu frontSideMainMenu)
        {
            return await Task.FromResult(GetHttpPostResponseString(nameof(IsFrontsideMenuActive), frontSideMainMenu) == "true");
        }

        public async Task<MessageEntity<UserAuthInformation>> ValidateLogin(LoginRequestParam param)
        {
            return await Task.FromResult(GetHttpPostResponse<MessageEntity<UserAuthInformation>>(nameof(ValidateLogin), param));
        }

        public async Task<int> GetOrCreateGeneratorId(string machineName)
        {
            return await Task.FromResult(GetHttpGetResponse<int>(nameof(GetOrCreateGeneratorId), $"{nameof(machineName)}={HttpUtility.UrlEncode(machineName)}"));
        }

        public async Task<UserInfo> GetUserInfoWithoutAvailable(int userId)
        {
            return await Task.FromResult(GetHttpGetResponse<UserInfo>(nameof(GetUserInfoWithoutAvailable), $"{nameof(userId)}={userId}"));
        }
    }
}