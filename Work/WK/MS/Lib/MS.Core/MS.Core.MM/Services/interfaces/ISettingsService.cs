using MS.Core.MM.Models.SystemSettings;
using MS.Core.MMModel.Models;
using MS.Core.MMModel.Models.Post.Enums;
using MS.Core.Models;

namespace MS.Core.MM.Services.interfaces
{
    /// <summary>
    /// 設定相關服務
    /// </summary>
    public interface ISettingsService
    {
        /// <summary>
        /// 取得選設項定，依發贴類型
        /// </summary>
        /// <param name="postType">發贴類型</param>
        /// <returns></returns>
        Task<BaseReturnDataModel<PostTypeOptions>> GetOptionItemByPostType(PostType postType);

        /// <summary>
        /// 取得價格設定
        /// </summary>
        /// <param name="postType">發贴類型</param>
        /// <returns></returns>
        Task<BaseReturnDataModel<OptionItem[]>> Price(PostType postType);

        /// <summary>
        /// 取得訊息種類設定
        /// </summary>
        /// <param name="postType">發贴類型</param>
        /// <returns></returns>
        Task<BaseReturnDataModel<OptionItem[]>> MessageType(PostType postType);

        /// <summary>
        /// 取得服務項目設定
        /// </summary>
        /// <param name="postType">發贴類型</param>
        /// <returns></returns>
        Task<BaseReturnDataModel<OptionItem[]>> Services(PostType postType);

        /// <summary>
        /// 取得年齡設定項
        /// </summary>
        /// <returns></returns>
        Task<BaseReturnDataModel<OptionItem[]>> Age();

        /// <summary>
        /// 身高設定項
        /// </summary>
        /// <returns></returns>
        Task<BaseReturnDataModel<OptionItem[]>> BodyHeight();

        /// <summary>
        /// Cup 設定項
        /// </summary>
        /// <returns></returns>
        Task<BaseReturnDataModel<OptionItem[]>> Cup();

        /// <summary>
        /// 取得管理員帳號
        /// </summary>
        /// <returns></returns>
        Task<BaseReturnDataModel<AdminContact>> GetAdminContact();

        /// <summary>
        /// 取得Tip
        /// </summary>
        /// <returns></returns>
        Task<BaseReturnDataModel<string>> GetTip(PostType postType);

        /// <summary>
        /// 取得篩選條件
        /// </summary>
        /// <param name="postType"></param>
        /// <returns></returns>
        Task<BaseReturnDataModel<PostFilterOptions>> GetPostFilterOptions(PostType? postType);
    }
}