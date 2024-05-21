using Microsoft.AspNetCore.Mvc.Rendering;

namespace BackSideWeb.Models.ViewModel.BotBet
{
    public class SettingInfoContextViewModel
    {
        public List<SelectListItem> BotGroupItems { get; set; }
        public List<SelectListItem> TimeTypeItems { get; set; }
        public List<SelectListItem> SettingGroupItems { get; set; }
    }
}
