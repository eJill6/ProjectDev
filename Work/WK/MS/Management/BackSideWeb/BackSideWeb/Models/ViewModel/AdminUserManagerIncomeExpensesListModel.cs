using MS.Core.MMModel.Models.IncomeExpense;
using MS.Core.Models.Models;
using MS.Core.MM.Extensions;
using MS.Core.MMModel.Extensions;
using JxBackendService.Resource.Element;
using JxBackendService.Model.Attributes;

namespace BackSideWeb.Models.ViewModel
{
	public class AdminUserManagerIncomeExpensesListModel
	{
		/// <summary>
		/// 订单号
		/// </summary>
		[Export(ResourcePropertyName = nameof(DisplayElement.IncomeExpenseId), ResourceType = typeof(DisplayElement), Sort = 1)]
		public string Id { get; set; }

		/// <summary>
		/// 账变时间
		/// </summary>
		public DateTime CreateTime { get; set; }


		/// <summary>
		/// 账变时间
		/// </summary>
		[Export(ResourcePropertyName = nameof(DisplayElement.IncomeExpenseCreateTime), ResourceType = typeof(DisplayElement), Sort = 2)]
		public string CreateTimeText { get; set; }

		/// <summary>
		/// 收付行為
		/// </summary>
		public IncomeExpenseCategoryEnum Category { get; set; }

		/// <summary>
		/// 收付行為
		/// </summary>
		[Export(ResourcePropertyName = nameof(DisplayElement.IncomeExpenseCategoryText), ResourceType = typeof(DisplayElement), Sort = 3)]
		public string CategoryText { get; set; }

		/// <summary>
		/// 贴子区域
		/// </summary>
		[Export(ResourcePropertyName = nameof(DisplayElement.IncomeExpensePostType), ResourceType = typeof(DisplayElement), Sort = 4)]
		public string PostType { get; set; }

		/// <summary>
		/// 会员ID
		/// </summary>
		[Export(ResourcePropertyName = nameof(DisplayElement.IncomeExpenseUserId), ResourceType = typeof(DisplayElement), Sort = 5)]
		public int UserId { get; set; }

		/// <summary>
		/// 余额账变
		/// </summary>
		[Export(ResourcePropertyName = nameof(DisplayElement.IncomeExpenseAmountText), ResourceType = typeof(DisplayElement), Sort = 6)]
		public string AmountText { get; set; }

		/// <summary>
		/// 钻石账变
		/// </summary>
		[Export(ResourcePropertyName = nameof(DisplayElement.IncomeExpensePointText), ResourceType = typeof(DisplayElement), Sort = 7)]
		public string PointText { get; set; }

		/// <summary>
		/// 帳變
		/// </summary>
		//[Export(ResourcePropertyName = nameof(DisplayElement.IncomeExpenseAmount), ResourceType = typeof(DisplayElement), Sort = 8)]
		public decimal Amount { get; set; }

		/// <summary>
		/// 支付方式
		/// </summary>
		public IncomeExpensePayType PayType { get; set; }


		/// <summary>
		/// 支付方式
		/// </summary>
		public string PayTypeText { get; set; }

		/// <summary>
		/// 备注
		/// </summary>
		[Export(ResourcePropertyName = nameof(DisplayElement.IncomeExpenseMemo), ResourceType = typeof(DisplayElement), Sort = 10)]
		public string Memo { get; set; } = string.Empty;

		/// <summary>
		/// 交易類型(收入/支出)
		/// </summary>
		//[Export(ResourcePropertyName = nameof(DisplayElement.IncomeExpenseTransactionType), ResourceType = typeof(DisplayElement), Sort = 11)]
		public IncomeExpenseTransactionTypeEnum TransactionType { get; set; }
	}
}
