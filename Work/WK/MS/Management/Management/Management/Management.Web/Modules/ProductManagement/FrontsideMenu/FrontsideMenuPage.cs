using Microsoft.AspNetCore.Mvc;
using Serenity.Web;

namespace Management.ProductManagement.Pages 
{

    [PageAuthorize(typeof(FrontsideMenuRow))]
    public class FrontsideMenuController : Controller
    {
        [Route("ProductManagement/FrontsideMenu")]
        public ActionResult Index()
        {
            return this.GridPage("@/ProductManagement/FrontsideMenu/FrontsideMenuPage",
                FrontsideMenuRow.Fields.PageTitle());
        }
    }
}