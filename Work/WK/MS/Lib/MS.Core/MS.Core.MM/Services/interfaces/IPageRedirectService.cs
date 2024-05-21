using MS.Core.MM.Model.Banner;
using MS.Core.MM.Models.Entities.Post;
using MS.Core.Models;

namespace MS.Core.MM.Services.interfaces
{
    /// <summary>
    /// 秘色廣告轉導的服務
    /// </summary>
    public interface IPageRedirectService
    {
        /// <summary>
        /// 更新秘色廣告轉導
        /// </summary>
        /// <param name="param">更新秘色廣告轉導的參數</param>
        /// <returns>更新後的秘色廣告轉導</returns>
        Task<BaseReturnModel> Update(MMPageRedirect param);

        /// <summary>
        /// 後台秘色廣告轉導資料
        /// </summary>
        /// <returns>後台秘色廣告轉導資料</returns>
        Task<BaseReturnDataModel<MMPageRedirect[]>> Get();

        /// <summary>
        /// 後台取得單筆秘色廣告轉導資料
        /// </summary>
        /// <returns>後台取得單筆秘色廣告轉導資料</returns>
        Task<BaseReturnDataModel<MMPageRedirect>> Get(int id);
    }
}