using FakeMSSeal.Models.ZeroOne;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using MSResponse = MS.Core.Infrastructures.ZeroOne.Models.Responses;

namespace FakeMSSeal.Controllers
{
    [Route("/dapi/[controller]")]
    [ApiController]
    public class PermissionController : ControllerBase
    {
        private readonly ILogger<PointController> _logger = null;

        public PermissionController(
            ILogger<PointController> logger)
        {
            _logger = logger;
        }
        [HttpGet()]
        public MSResponse.ZenoOneResDataBase<bool?> Index([FromQuery] ZOVipPermissionReq request)
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

                    return new MSResponse.ZenoOneResDataBase<bool?>()
                    {
                        Data = VipUsers.Contains(request.UserId),
                        Success = true,
                    };
                }
            }

            return new MSResponse.ZenoOneResDataBase<bool?>()
            {
                Data = null,
                Success = false,
                Error = "签名不正确"
            };
        }

        private HashSet<int> VipUsers => new HashSet<int> { 2834, 6252, 6253, 6777 };
    }
}