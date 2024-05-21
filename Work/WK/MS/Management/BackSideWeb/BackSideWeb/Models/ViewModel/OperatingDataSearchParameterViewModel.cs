using MS.Core.Models.Models;
using System.ComponentModel.DataAnnotations;

namespace BackSideWeb.Models.ViewModel
{
    public class OperatingDataSearchParameterViewModel: PageParam
    {
        [Required(ErrorMessage ="请选择开始时间")]
        public string BeginTime { get; set; }
        [Required(ErrorMessage = "请选择结束时间")]
        public string EndTime { get; set; }
    }
}
