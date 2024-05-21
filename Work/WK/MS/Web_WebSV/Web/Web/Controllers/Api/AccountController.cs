using SLPolyGame.Web.Model;
using System.Net;
using System.Web.Http;
using Web.Helpers;
using Web.Helpers.Security;
using Web.Infrastructure.Filters;
using Web.Models.Base;
using Web.Services;

namespace Web.Controllers.Api
{
    /// <summary>
    /// 處理登入相關
    /// </summary>
    public class AccountController : BaseApiController
    {
        protected readonly IPlayInfoService _playInfoService = null;

        public AccountController(IUserService userService,
            ICacheService cacheService,
            IPlayInfoService playInfoService) : base(cacheService, userService)
        {
            _playInfoService = playInfoService;
        }

        /// <summary>
        /// Logon view post.
        /// </summary>
        /// <returns>View after logon.</returns>
        [HttpGet]
        [Anonymous]
        public IHttpActionResult LogOn(int userId, string userName, string roomNo, string gameID, string depositUrl)
        {
            if (userId == 0 ||
                string.IsNullOrWhiteSpace(userName) ||
                string.IsNullOrWhiteSpace(roomNo) ||
                string.IsNullOrWhiteSpace(gameID) ||
                string.IsNullOrWhiteSpace(depositUrl))
            {
                return Json(HttpStatusCode.BadRequest);
            }

            var result = _userService.ValidateLogin(new LoginRequestParam
            {
                UserId = userId,
                UserName = userName,
                RoomNo = roomNo,
                GameID = gameID,
                DepositUrl = depositUrl
            });

            var ticketUserData = new TicketUserData()
            {
                UserId = userId,
                UserName = userName,
                Key = result.Key,
                GameID = gameID,
                DepositUrl = depositUrl,
                RoomNo = roomNo
            };

            return Ok(new
            {
                UserId = userId,
                UserName = userName,
                UserKey = TokenProvider.GenerateTokenString(result.Key, result.UserName),
                GameID = gameID,
                DepositUrl = depositUrl,
                RoomNo = roomNo
            });
        }
    }
}