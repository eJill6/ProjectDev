using JxBackendService.Model.ViewModel.Menu;

namespace ControllerShareLib.Models.Game.Menu
{
    public class MobileApiMenuTypeViewModel : BaseFrontsideMenuTypeViewModel
    {
        public List<MobileApiProductMenu> MobileApiProductMenus { get; set; } = new List<MobileApiProductMenu>();
    }
}