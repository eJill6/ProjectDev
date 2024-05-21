using MS.Core.MMModel.Models.IncomeExpense;
using MS.Core.MMModel.Models.Post.Enums;
using MS.Core.MMModel.Models.Vip.Enums;
using System;

namespace MS.Core.MM.Extensions
{
    public static class EnumConvertExtension
    {

        public static IncomeExpenseCategoryEnum ConvertToIncomeExpenseCategory(this PostType postType)
        {
            return (IncomeExpenseCategoryEnum)postType;
        }

        public static PostType? ConvertToPostType(this IncomeExpenseCategoryEnum category)
        {
            PostType? result = null;
            if (Enum.TryParse<PostType>(category.ToString(), out var postType))
            {
                if (postType != default(PostType))
                {
                    result = postType;
                }
            }
            return result;
        }
    }
}
