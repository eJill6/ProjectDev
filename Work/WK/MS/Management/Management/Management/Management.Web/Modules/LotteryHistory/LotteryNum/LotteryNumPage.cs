using Microsoft.AspNetCore.Mvc;
using Serenity.Web;

namespace Management.LotteryHistory.Pages 
{

    [PageAuthorize(typeof(LotteryNumRow))]
    public class LotteryNumController : Controller
    {
        [Route("LotteryHistory/LotteryNum")]
        public ActionResult Index()
        {
            return this.GridPage("@/LotteryHistory/LotteryNum/LotteryNumPage",
                LotteryNumRow.Fields.PageTitle());
        }
    }
}