using Microsoft.AspNetCore.Mvc;
using Serenity.Web;

namespace Management.BetHistory.Pages 
{

    [PageAuthorize(typeof(PalyInfoRow))]
    public class PalyInfoController : Controller
    {
        [Route("BetHistory/PalyInfo")]
        public ActionResult Index()
        {
            return this.GridPage("@/BetHistory/PalyInfo/PalyInfoPage",
                PalyInfoRow.Fields.PageTitle());
        }
    }
}