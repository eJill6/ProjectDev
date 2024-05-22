using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MS.Core.Infrastructures.ZoneOne;

namespace FakeMSSeal.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UrlController : ControllerBase
    {
        private readonly ILogger<UrlController> _logger = null;

        private readonly IOptionsMonitor<LuoznzUrlModel> _setting = null;

        public UrlController(
            IOptionsMonitor<LuoznzUrlModel> setting,
            ILogger<UrlController> logger)
        {
            _setting = setting;
            _logger = logger;
        }

        [HttpGet("")]
        public LuoznzUrlModel Index()
        {
            if (Request.Headers.ContainsKey("x-id"))
            {
                if (Request.Headers["x-id"] == "Seal20kdwU29p20K")
                {
                    return _setting.CurrentValue;
                }
            }

            return new LuoznzUrlModel();
        }
    }
}