using Microsoft.AspNetCore.Mvc.Rendering;

namespace BackSideWeb.Models.ViewModel
{
    public class AdminPostTransactionViewModel
    {
        public List<SelectListItem> PostTypeItems { get; set; }
        public List<SelectListItem> UnlockMethodItems { get; set; }
    }
}
