using FakeMSSeal.Models.ZeroOne;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using MSResponse = MS.Core.Infrastructures.ZeroOne.Models.Responses;

namespace FakeMSSeal.Controllers
{
    [Route("/dapi/[controller]")]
    [ApiController]
    public class AmountController : ControllerBase
    {
        private readonly ILogger<PointController> _logger = null;

        public AmountController(
            ILogger<PointController> logger)
        {
            _logger = logger;
        }

        [HttpPost()]
        public MSResponse.ZenoOneResDataBase Index(ZOCashDapiReq request)
        {
            if (Request.Headers.ContainsKey("sign") && Request.Headers.ContainsKey("ts"))
            {
                var sign = Request.Headers["sign"].ToString();
                var ts = Convert.ToInt64(Request.Headers["ts"].ToString());
                request.Ts = ts;
                var actualSign = Util.ValidSignUtil.GetSign(request);
                if (string.Equals(actualSign, sign))
                {
                    _logger.LogInformation($"{JsonConvert.SerializeObject(request)}");

                    return new MSResponse.ZenoOneResDataBase()
                    {
                        Success = true,
                    };
                }
            }

            return new MSResponse.ZenoOneResDataBase()
            {
                Success = false,
                Error = "签名不正确"
            };
        }
    }
}