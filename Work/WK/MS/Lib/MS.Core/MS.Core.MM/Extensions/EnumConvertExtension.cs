using MS.Core.MM.Models.Entities.User;
using MS.Core.MMModel.Models.IncomeExpense;
using MS.Core.MMModel.Models.Post.Enums;
using MS.Core.MMModel.Models.Vip.Enums;
using System.ComponentModel;
using System.Reflection;

namespace MS.Core.MM.Extensions;

public static class EnumConvertExtension
{
    public static UserSummaryCategoryEnum ConvertToUserSummaryCategory(this VipType vipType)
    {
        switch (vipType)
        {
            case VipType.Silver: 
                return UserSummaryCategoryEnum.Silver;
            case VipType.Gold : 
                return UserSummaryCategoryEnum.Gold;
            case VipType.Diamond:
                return UserSummaryCategoryEnum.Diamond;
            default:
                throw new NotImplementedException();
        }
    }
    /// <summary>
    /// 发帖统计类型
    /// </summary>
    /// <param name="postType"></param>
    /// <returns></returns>
    public static UserSummaryCategoryEnum ConvertToUserSummaryCategory(this PostType postType)
    {
        return (UserSummaryCategoryEnum)postType;
    }

    public static VIPWelfareCategoryEnum ConvertToVIPWelfareCategory(this PostType postType)
    {
        return (VIPWelfareCategoryEnum)postType;
    }
    /// <summary>
    /// 用户收藏统计类型
    /// </summary>
    /// <param name="userFavoriteCategory"></param>
    /// <returns></returns>
    public static UserSummaryCategoryEnum ConvertToUserSummaryCategory(this UserFavoriteCategoryEnum userFavoriteCategory)
    {
        return (UserSummaryCategoryEnum)userFavoriteCategory;
    }

    public static PostType? ConvertToPostType(this UserSummaryCategoryEnum category)
    {
        PostType? result = null;
        if (Enum.TryParse(typeof(PostType), category.ToString(), out var postType))
        {
            if (postType != null)
            {
                result = (PostType)postType;
            }
        }
        return result;
    }

    public static bool IsPostType(this UserSummaryCategoryEnum category)
    {
        return Enum.IsDefined(typeof(PostType), (byte)category);
    }


    public static PostType? ConvertToPostType(this IncomeExpenseCategoryEnum category)
    {
        PostType? result = null;
        if (Enum.TryParse<PostType>(category.ToString(), out var postType))
        {
            if (postType != default)
            {
                result = postType;
            }
        }
        return result;
    }

    public static UserSummaryCategoryEnum? ConvertToUserSummaryCategory(this IncomeExpenseCategoryEnum category)
    {
        UserSummaryCategoryEnum? result = null;
        if (Enum.TryParse<UserSummaryCategoryEnum>(category.ToString(), out var postType))
        {
            if (postType != default(UserSummaryCategoryEnum))
            {
                result = postType;
            }
        }
        return result;
    }

    public static bool IsUserSummaryCategory(this IncomeExpenseCategoryEnum category)
    {
        return Enum.IsDefined(typeof(UserSummaryCategoryEnum), (byte)category);
    }
    public static string GetEnumDescription(this Enum value)
    {
        var memberInfo = value.GetType().GetMember(value.ToString()).FirstOrDefault();
        var descriptionAttribute = memberInfo?.GetCustomAttribute<DescriptionAttribute>();

        return descriptionAttribute != null ? descriptionAttribute.Description : value.ToString();
    }
}
