using MS.Core.MM.Model.Banner;
using MS.Core.MM.Models.Entities.Post;
using MS.Core.Models;

namespace MS.Core.MM.Services.interfaces
{
    /// <summary>
    /// Banner的服務
    /// </summary>
    public interface IAdvertisingContentService
    {
        /// <summary>
        /// 更新宣传文字資料
        /// </summary>
        /// <param name="param">更新宣传文字資料的參數</param>
        /// <returns>更新後的宣传文字資料</returns>
        Task<BaseReturnModel> Update(MMAdvertisingContent param);

        /// <summary>
        /// 刪除宣传文字資料
        /// </summary>
        Task<BaseReturnModel> Delete(int id);

        /// <summary>
        /// 後台取得宣传文字資料
        /// </summary>
        /// <returns>後台取得宣传文字資料</returns>
        Task<BaseReturnDataModel<MMAdvertisingContent[]>> Get();

        /// <summary>
        /// 後台取得單筆宣传文字資料
        /// </summary>
        /// <returns>後台取得單筆宣传文字資料</returns>
        Task<BaseReturnDataModel<MMAdvertisingContent>> Get(int id);
    }
}