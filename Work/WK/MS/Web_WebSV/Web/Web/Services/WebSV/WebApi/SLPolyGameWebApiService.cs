using JxBackendService.Model.ViewModel;
using SLPolyGame.Web.Interface;
using SLPolyGame.Web.Model;
using System;
using System.Collections.Generic;
using System.Web;
using Web.Services.WebSV.Base;
using JxBackendService.Common.Util;
using System.Threading.Tasks;

namespace Web.Services.WebSV.WebApi
{
    public class SLPolyGameWebApiService : BaseWebSVService, ISLPolyGameWebSVService
    {
        protected override string RemoteControllerName => "SLPolyGameService";

        public string CancelOrder(PalyInfo model)
        {
            return GetHttpPostResponseString(model);
        }

        public CursorPagination<CurrentLotteryInfo> GetCursorPaginationDrawResult(int lotteryId, DateTime start, DateTime end, int count, string cursor)
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

            return GetHttpGetResponse<CursorPagination<CurrentLotteryInfo>>(queryStringParts);
        }

        public CursorPagination<PalyInfo> GetFollowBet(string palyId, int lottertId)
        {
            var queryStringMap = new Dictionary<string, string>
            {
                { "palyId", palyId },
                { "lottertId", lottertId.ToString() },
            };

            string[] queryStringParts = GetQueryStringParts(queryStringMap);

            return GetHttpGetResponse<CursorPagination<PalyInfo>>(queryStringParts);
        }

        public FrontsideMenuViewModel GetFrontsideMenuViewModel()
        {
            return GetHttpGetResponse<FrontsideMenuViewModel>();
        }

        //public async Task<FrontsideMenuViewModel> GetFrontsideMenuViewModelAsync()
        //{
        //    ResponseInfo responseInfo = await GetHttpGetResponseStringAsync("GetFrontsideMenuViewModel");

        //    return responseInfo.Content.Deserialize<FrontsideMenuViewModel>();
        //}

        public List<string> GetLatestWinningList(string period)
        {
            return GetHttpGetResponse<List<string>>($"{nameof(period)}={HttpUtility.UrlEncode(period)}");
        }

        public List<WinningListItem> GetLatestWinningListItems(string period)
        {
            return GetHttpGetResponse<List<WinningListItem>>($"{nameof(period)}={HttpUtility.UrlEncode(period)}");
        }

        public PalyInfo GetPalyIDPalyBet(string value)
        {
            return GetHttpGetResponse<PalyInfo>($"{nameof(value)}={HttpUtility.UrlEncode(value)}");
        }

        public PalyInfo GetPlayBetByAnonymous(string playId)
        {
            return GetHttpGetResponse<PalyInfo>($"{nameof(playId)}={HttpUtility.UrlEncode(playId)}");
        }

        public PalyInfo[] GetPlayBetsByAnonymous(string startTime, string endTime, string gameId)
        {
            var queryStringMap = new Dictionary<string, string>
            {
                { "startTime", startTime },
                { "endTime", endTime },
                { "gameId", gameId },
            };

            string[] queryStringParts = GetQueryStringParts(queryStringMap);

            return GetHttpGetResponse<PalyInfo[]>(queryStringParts);
        }

        public DateTime GetServerCurrentTime()
        {
            return GetHttpGetResponse<DateTime>();
        }

        public CursorPagination<PalyInfo> GetSpecifyOrderList(int userId, int lotteryId, string status, DateTime searchDate, string cursor, int pageSize)
        {
            var queryStringMap = new Dictionary<string, string>
            {
                { "userId", userId.ToString() },
                { "lotteryId", lotteryId.ToString() },
                { "status", status },
                { "searchDate", searchDate.ToFormatDateTimeMillisecondsString() },
                { "cursor", cursor },
                { "pageSize", pageSize.ToString() },
            };

            string[] queryStringParts = GetQueryStringParts(queryStringMap);

            return GetHttpGetResponse<CursorPagination<PalyInfo>>(queryStringParts);
        }

        public string GetStatus()
        {
            return GetHttpGetResponseString();
        }

        public SysSettings GetSysSettings()
        {
            return GetHttpGetResponse<SysSettings>();
        }

        public UserInfo GetUserInfo()
        {
            return GetHttpGetResponse<UserInfo>();
        }

        public UserInfo GetUserInfoByUserID(int UserID)
        {
            return GetHttpGetResponse<UserInfo>($"{nameof(UserID)}={HttpUtility.UrlEncode(UserID.ToString())}");
        }

        public PalyInfo InsertPlayInfo(PalyInfo model)
        {
            return GetHttpPostResponse<PalyInfo>(model);
        }

        public bool IsFrontsideMenuActive(string productCode)
        {
            return GetHttpGetResponseString($"{nameof(productCode)}={HttpUtility.UrlEncode(productCode)}") == "true";
        }

        public MessageEntity<UserAuthInformation> ValidateLogin(LoginRequestParam param)
        {
            return GetHttpPostResponse<MessageEntity<UserAuthInformation>>(param);
        }
    }
}