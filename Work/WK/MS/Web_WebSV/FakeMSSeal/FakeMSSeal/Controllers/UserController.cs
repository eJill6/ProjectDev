using FakeMSSeal.Mock;
using FakeMSSeal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MS.Core.Infrastructures.ZeroOne.Models.Responses;
using Newtonsoft.Json;

namespace FakeMSSeal.Controllers
{
    [Route("dapi/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IBalanceMockServcie _mockBalanceServcie;

        private readonly ILogger<UserController> _logger = null;

        /// <summary>
        /// 機器人列表
        /// </summary>
        private readonly IOptionsMonitor<RobotResult> _bots = null;

        /// <summary>
        /// 使用者列表
        /// </summary>
        private readonly IOptionsMonitor<List<ZOUserInfoRes>> _userInfos = null;

        public UserController(IBalanceMockServcie mockBalanceServcie,
            IOptionsMonitor<RobotResult> bots,
            IOptionsMonitor<List<ZOUserInfoRes>> userInfos,
            ILogger<UserController> logger)
        {
            _mockBalanceServcie = mockBalanceServcie;
            _bots = bots;
            _userInfos = userInfos;
            _logger = logger;
        }

        [HttpGet("balance")]
        public BalanceResult Balance([FromQuery] BalanceRequest request)
        {
            if (Request.Headers.ContainsKey("sign") && Request.Headers.ContainsKey("ts"))
            {
                var sign = Request.Headers["sign"].ToString();
                request.Ts = Convert.ToInt64(Request.Headers["ts"].ToString());
                request.Salt = GlobalValue.Salt;
                if (string.Equals(request.GetSign(), sign))
                {
                    _logger.LogInformation($"{JsonConvert.SerializeObject(request)}");
                    BalanceResult balanceResult = _mockBalanceServcie.GetBalance(request.UserId);

                    return balanceResult;
                }
            }

            return new BalanceResult()
            {
                Success = false,
                Error = "签名不正确"
            };
        }

        [HttpGet("robot")]
        public RobotResult Robot([FromQuery] SimpleRequest request)
        {
            if (Request.Headers.ContainsKey("sign") && Request.Headers.ContainsKey("ts"))
            {
                var sign = Request.Headers["sign"].ToString();
                request.Ts = Convert.ToInt64(Request.Headers["ts"].ToString());
                request.Salt = GlobalValue.Salt;
                if (string.Equals(request.GetSign(), sign))
                {
                    _logger.LogInformation($"{JsonConvert.SerializeObject(request)}");

                    return new RobotResult()
                    {
                        Success = true,
                        Data = _bots.CurrentValue.Data
                    };
                }
            }

            return new RobotResult()
            {
                Success = false,
                Error = "签名不正确"
            };
        }

        [HttpGet()]
        public ResultModel<ZOUserInfoRes> User(int userId)
        {
            if (Request.Headers.ContainsKey("sign") && Request.Headers.ContainsKey("ts"))
            {
                var sign = Request.Headers["sign"].ToString();
                var ts = Convert.ToInt64(Request.Headers["ts"].ToString());
                var actualSign = Util.ValidSignUtil.GetSign(new
                {
                    userId,
                    ts
                });
                if (string.Equals(actualSign, sign))
                {
                    _logger.LogInformation($"{JsonConvert.SerializeObject(userId)}");

                    var result = _userInfos.CurrentValue.FirstOrDefault(x => x.UserId == userId);
                    if (result == null)
                    {
                        result = _userInfos.CurrentValue.First();
                    }
                    return new ResultModel<ZOUserInfoRes>()
                    {
                        Success = true,
                        Data = result,
                    };
                }
            }

            return new ResultModel<ZOUserInfoRes>()
            {
                Success = false,
                Error = "签名不正确"
            };
        }
    }
}