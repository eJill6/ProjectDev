using Microsoft.AspNetCore.Mvc;
using Serenity.Web;

namespace Management.SystemSettings.Pages 
{

    [PageAuthorize(typeof(LotteryInfoRow))]
    public class LotteryInfoController : Controller
    {
        [Route("SystemSettings/LotteryInfo")]
        public ActionResult Index()
        {
            return this.GridPage("@/SystemSettings/LotteryInfo/LotteryInfoPage",
                LotteryInfoRow.Fields.PageTitle());
        }
    }
}