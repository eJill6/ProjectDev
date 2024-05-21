using JxBackendService.Model.Attributes;
using JxBackendService.Resource.Element;

namespace BackSideWeb.Models.ViewModel.OperatingData
{
    /// <summary>
    /// 月人数
    /// </summary>
    public class MonthlyUsersViewModel
    {


        /// <summary>
        /// 月份
        /// </summary>
        public DateTime Month { get; set; }
        public int PV { get; set; }

        public int MAU { get; set; }


        [Export(ResourcePropertyName = nameof(DisplayElement.OperatingDataMonthTimeText), ResourceType = typeof(DisplayElement), Sort = 1)]
        public string MonthlyUsersTimeText => Month.ToString("yyyy MM 月");

        [Export(ResourcePropertyName = nameof(DisplayElement.OperatingDataPV), ResourceType = typeof(DisplayElement), Sort = 2)]
        public string PVText => PV.ToString("N0");
        [Export(ResourcePropertyName = nameof(DisplayElement.OperatingDataMAU), ResourceType = typeof(DisplayElement), Sort = 3)]
        public string MAUText => MAU.ToString("N0");
    }
}
