using JxBackendService.Model.Attributes;
using JxBackendService.Resource.Element;

namespace BackSideWeb.Models.ViewModel.OperatingData
{
    public class DailyUsersViewModel
    {
        /// <summary>
        /// 日期
        /// </summary>
        public DateTime Date { get; set; }
        public int PV { get; set; }
        public int DAU { get; set; }
        public int PCU { get; set; }
        public double ACU { get; set; }
        public double DUN { get; set; }

        [Export(ResourcePropertyName = nameof(DisplayElement.OperatingDataTimeText), ResourceType = typeof(DisplayElement), Sort = 1)]
        public string DailyUsersTimeText => Date.ToString("yyy-MM-dd");

        [Export(ResourcePropertyName = nameof(DisplayElement.OperatingDataPV), ResourceType = typeof(DisplayElement), Sort = 2)]
        public string PVText => PV.ToString("N0");

        [Export(ResourcePropertyName = nameof(DisplayElement.OperatingDataDAU), ResourceType = typeof(DisplayElement), Sort = 3)]
        public string DAUText => DAU.ToString("N0");

        [Export(ResourcePropertyName = nameof(DisplayElement.OperatingDataPCU), ResourceType = typeof(DisplayElement), Sort = 4)]
        public string PCUText => PCU.ToString("N0");

        [Export(ResourcePropertyName = nameof(DisplayElement.OperatingDataACU), ResourceType = typeof(DisplayElement), Sort = 5)]
        public string ACUText => ACU.ToString("N0");

        [Export(ResourcePropertyName = nameof(DisplayElement.OperatingDataDUN), ResourceType = typeof(DisplayElement), Sort = 6)]
        public string DUNText => DUN.ToString("N0");
    }
}
