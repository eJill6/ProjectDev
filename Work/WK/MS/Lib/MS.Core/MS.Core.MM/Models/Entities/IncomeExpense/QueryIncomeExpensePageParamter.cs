using MS.Core.MMModel.Models.AdminUserManager;
using MS.Core.MMModel.Models.Post.Enums;
using MS.Core.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS.Core.MM.Models.Entities.IncomeExpense
{
    public class QueryIncomeExpensePageParamter: PageParam
    {
        public string? Id { get; set; }
        public int? UserId { get; set; }

        public PostType? PostType { get; set; }
        public AdminIncomeExpensesCategory? Category { get; set; }
        public DateTime? BeginDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
