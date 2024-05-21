using Microsoft.AspNetCore.Mvc;
using Serenity.Web;

namespace Management.ProfitlossTest.Pages 
{

    [PageAuthorize(typeof(ProfitLossRow))]
    public class ProfitLossController : Controller
    {
        [Route("ProfitlossTest/ProfitLoss")]
        public ActionResult Index()
        {
            return this.GridPage("@/ProfitlossTest/ProfitLoss/ProfitLossPage",
                ProfitLossRow.Fields.PageTitle());
        }
    }
}