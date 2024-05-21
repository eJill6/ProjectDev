using Microsoft.AspNetCore.Mvc.Rendering;

namespace BackSideWeb.Models.ViewModel
{
    public class IncomeExpenseViewModel
	{
        public List<SelectListItem> PayActionItems { get; set; }
        public List<SelectListItem> PostModuleItems { get; set; }
		public List<SelectListItem> PayTypeItems { get; set; }

        public List<SelectListItem> IdentityTypeItems { get; set; }

    }
}
