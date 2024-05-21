using JxBackendService.Model.Attributes;
using JxBackendService.Model.Entity.Base;
using JxBackendService.Resource.Element;
using MS.Core.MMModel.Models.IncomeExpense;
using System.ComponentModel.DataAnnotations;

namespace BackSideWeb.Models.ViewModel
{
    public class AdminIncomeInputModel : BaseEntityModel
    {
        /// <summary>
        /// 收益單Id
        /// </summary>
        public string IncomeId { get; set; }

        /// <summary>
        /// 評價編號
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 審核狀態 1. 入账, 2, 審核中, 4. 不入账,
        /// </summary>
        public IncomeExpenseStatusEnum Status { get; set; }

        /// <summary>
        /// 未通過原因
        /// </summary>
        [Display(Name = nameof(DisplayElement.Memo), ResourceType = typeof(DisplayElement))]
        [CustomizedRequired]
        public string Memo { get; set; }
    }
}