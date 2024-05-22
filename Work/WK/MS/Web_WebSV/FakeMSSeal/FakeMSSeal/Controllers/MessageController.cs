using FakeMSSeal.Models;
using FakeMSSeal.Models.Messages;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace FakeMSSeal.Controllers
{
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly ILogger<MessageController> _logger = null;
        public MessageController(ILogger<MessageController> logger)
        {
            _logger = logger;
        }

        [HttpPost("/dapi/message")]
        public async Task<BalanceResult> Index([FromBody] BaseMessageRequest request)
        {
            if (Request.Headers.ContainsKey("sign") && Request.Headers.ContainsKey("ts"))
            {
                var sign = Request.Headers["sign"].ToString();
                request.Ts = Convert.ToInt64(Request.Headers["ts"].ToString());
                request.Salt = GlobalValue.Salt;
                if (string.Equals(request.GetSign(), sign))
                {
                    var body = JsonSerializer.Serialize(request);
                    _logger.LogInformation($"{body}");

                    return new BalanceResult()
                    {
                        Success = true,
                    };
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
