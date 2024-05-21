using MS.Core.MM.Models.Entities.Post;
using MS.Core.MM.Models.Post;
using MS.Core.MMModel.Models.Post.Enums;
using MS.Core.Models;

namespace MS.Core.MM.Repos.interfaces
{
    public interface IOptionItemRepo
    {
        /// <summary>
        /// 從頁面獲取選項
        /// </summary>
        /// <param name="postType"></param>
        /// <returns></returns>
        Task<MMOptions[]> GetOptionByPostType(int postType);

        /// <summary>
        /// 從頁面獲取選項(後台使用)
        /// </summary>
        /// <returns></returns>
        Task<MMOptions[]> GetOptionsByPostTypeAndOptionType(PostType postType, OptionType optionType, int? OptionId);

        /// <summary>
        /// 新增項目
        /// </summary>
        /// <returns></returns>
        Task<DBResult> Create(CreateOptionsParam param);

        /// <summary>
        /// 刪除項目
        /// </summary>
        /// <returns></returns>
        Task Delete(int optionId);

        /// <summary>
        /// 修改項目
        /// </summary>
        /// <returns></returns>
        Task<DBResult> Update(UpdateOptionsParam param);

        /// <summary>
        /// 取得選項內容
        /// </summary>
        /// <param name="postType">發贴類型</param>
        /// <returns></returns>
        Task<Dictionary<int, string>> GetPostTypeOptions(PostType? postType);

        /// <summary>
        /// 取得選項內容
        /// </summary>
        /// <param name="postTypes">發贴類型集合</param>
        /// <returns></returns>
        Task<MMOptions[]> GetOptionsByPostTypes(List<int> postTypes);
    }
}