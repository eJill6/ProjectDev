using JxBackendService.Interface.Service.Web.BackSideWeb;

namespace JxBackendService.Model.ViewModel.Game
{
    public class GameCenterManageModel : IDataKey
    {
        public int No { get; set; }

        public string MenuName { get; set; }

        public int Sort { get; set; }

        public bool IsActive { get; set; }

        public string KeyContent => No.ToString();
    }
}
