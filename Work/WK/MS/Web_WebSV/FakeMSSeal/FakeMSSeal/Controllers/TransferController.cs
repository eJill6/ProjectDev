using FakeMSSeal.Mock;
using FakeMSSeal.Models;
using FakeMSSeal.Util;
using JxBackendService.Common.Util;
using JxBackendService.Model.Enums.Finance;
using JxBackendService.Model.MiseLive.Request;
using JxBackendService.Model.MiseLive.Response;
using Microsoft.AspNetCore.Mvc;

namespace FakeMSSeal.Controllers
{
    [Route("dapi/[controller]")]
    [ApiController]
    public class TransferController : ControllerBase
    {
        private readonly IBalanceMockServcie _balanceMockServcie;

        public TransferController(IBalanceMockServcie balanceMockServcie)
        {
            _balanceMockServcie = balanceMockServcie;
        }

        [HttpPost("in")]
        public BalanceResult In([FromBody] MiseLiveTransferRequest request)
        {
            if (Request.Headers.ContainsKey("sign") && Request.Headers.ContainsKey("ts"))
            {
                string sign = Request.Headers["sign"].ToString();
                request.Ts = Convert.ToInt64(Request.Headers["ts"].ToString());
                request.Salt = GlobalValue.Salt;

                if (string.Equals(Util.ValidSignUtil.CreateSign(request), sign))
                {
                    BalanceResult balanceResult = _balanceMockServcie.AddBalance(request.UserId, request.Amount);

                    return balanceResult;
                }
            }

            return new BalanceResult()
            {
                Success = false,
                Error = "签名不正确"
            };
        }

        [HttpPost("out")]
        public BalanceResult Out([FromBody] MiseLiveTransferRequest request)
        {
            if (Request.Headers.ContainsKey("sign") && Request.Headers.ContainsKey("ts"))
            {
                string sign = Request.Headers["sign"].ToString();
                request.Ts = Convert.ToInt64(Request.Headers["ts"].ToString());
                request.Salt = GlobalValue.Salt;

                if (string.Equals(Util.ValidSignUtil.CreateSign(request), sign))
                {
                    BalanceResult balanceResult = _balanceMockServcie.SubstractBalance(request.UserId, request.Amount);

                    return balanceResult;
                }
            }

            return new BalanceResult()
            {
                Success = false,
                Error = "签名不正确"
            };
        }

        [HttpGet("result")]
        public MiseLiveResponse<MiseLiveTransferOrder> Result([FromQuery] MiseLiveTransferOrderRequest request)
        {
            if (Request.Headers.ContainsKey("sign") && Request.Headers.ContainsKey("ts"))
            {
                string sign = Request.Headers["sign"].ToString();
                request.Ts = Convert.ToInt64(Request.Headers["ts"].ToString());
                request.Salt = GlobalValue.Salt;

                if (string.Equals(Util.ValidSignUtil.CreateSign(request), sign))
                {
                    return new MiseLiveResponse<MiseLiveTransferOrder>()
                    {
                        Success = true,
                        Data = new MiseLiveTransferOrder()
                        {
                            Success = true,
                            UserId = 6315,
                            TransferType = MiseTransferType.In.Value,
                            Amount = 100.05m,
                            CreateTime = DateTime.Now.ToFormatDateTimeString()
                        }
                    };
                }
            }

            return new MiseLiveResponse<MiseLiveTransferOrder>
            {
                Success = false,
                Error = "签名不正确"
            };
        }
    }
}