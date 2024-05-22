using FakeMSSeal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace FakeMSSeal.Controllers
{
    [Route("dapi/[controller]")]
    [ApiController]
    public class AnchorController : ControllerBase
    {
        private readonly ILogger<AnchorController> _logger = null;

        /// <summary>
        /// 秘色主播列表
        /// </summary>
        private readonly IOptionsMonitor<AnchorResult> _anchors = null;

        public AnchorController(IOptionsMonitor<AnchorResult> anchors,
            ILogger<AnchorController> logger)
        {
            _anchors = anchors;
            _logger = logger;
        }

        [HttpGet("live")]
        public AnchorResult Robot([FromQuery] SimpleRequest request)
        {
            if (Request.Headers.ContainsKey("sign") && Request.Headers.ContainsKey("ts"))
            {
                var sign = Request.Headers["sign"].ToString();
                request.Ts = Convert.ToInt64(Request.Headers["ts"].ToString());
                request.Salt = GlobalValue.Salt;
                if (string.Equals(request.GetSign(), sign))
                {
                    _logger.LogInformation($"{JsonConvert.SerializeObject(request)}");

                    return new AnchorResult()
                    {
                        Success = true,
                        Data = _anchors.CurrentValue.Data,
                    };
                }
            }

            return new AnchorResult()
            {
                Success = false,
                Error = "签名不正确"
            };
        }
    }
}