using FakeMSSeal.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FakeMSSeal.Controllers
{
    [Route("dapi/[controller]")]
    [ApiController]
    public class DeductController : ControllerBase
    {
        private readonly ILogger<DeductController> _logger = null;
        public DeductController(ILogger<DeductController> logger)
        {
            _logger = logger;
        }

        [HttpPost("amount")]
        public BalanceResult Amount([FromBody] DeductRequest request)
        {
            if (Request.Headers.ContainsKey("sign") && Request.Headers.ContainsKey("ts"))
            {
                var sign = Request.Headers["sign"].ToString();
                request.Ts = Convert.ToInt64(Request.Headers["ts"].ToString());
                request.Salt = GlobalValue.Salt;
                if (string.Equals(request.GetSign(), sign))
                {
                    _logger.LogInformation($"{JsonConvert.SerializeObject(request)}");
                    if (Convert.ToDecimal(request.Amount) <= 100)
                    {
                        return new BalanceResult()
                        {
                            Success = true,
                            Data = new BalanceDetail()
                            {
                                Balance = "100.000"
                            }
                        };
                    }
                    else
                    {
                        return new BalanceResult()
                        {
                            Success = false,
                            Error = "用户余额不足"
                        };
                    }
                }
            }

            return new BalanceResult()
            {
                Success = false,
                Error = "签名不正确"
            };
        }

        [HttpPost("amount/batch")]
        public BalanceResult AmountBatch([FromBody] DeductBatchRequest request)
        {
            if (Request.Headers.ContainsKey("sign") && Request.Headers.ContainsKey("ts"))
            {
                var sign = Request.Headers["sign"].ToString();
                request.Ts = Convert.ToInt64(Request.Headers["ts"].ToString());
                request.Salt = GlobalValue.Salt;
                if (string.Equals(request.GetSign(), sign))
                {
                    _logger.LogInformation($"{JsonConvert.SerializeObject(request)}");
                    if (request.TotalAmount <= 100.0)
                    {
                        return new BalanceResult()
                        {
                            Success = true,
                            Data = new BalanceDetail()
                            {
                                Balance = "100.000"
                            }
                        };
                    }
                    else
                    {
                        return new BalanceResult()
                        {
                            Success = false,
                            Error = "用户余额不足"
                        };
                    }
                }
            }

            return new BalanceResult()
            {
                Success = false,
                Error = "签名不正确"
            };
        }
    }
}