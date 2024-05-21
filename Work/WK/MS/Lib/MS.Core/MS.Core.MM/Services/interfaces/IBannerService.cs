using MS.Core.MM.Model.Banner;
using MS.Core.Models;

namespace MS.Core.MM.Services.interfaces
{
    /// <summary>
    /// Banner的服務
    /// </summary>
    public interface IBannerService
    {
        /// <summary>
        /// 創建Banner資料
        /// </summary>
        /// <param name="param">新增Banner的參數</param>
        /// <returns>創建後的Banner資料</returns>
        Task<BaseReturnModel> Create(SaveBannerParam param);

        /// <summary>
        /// 刪除Banner資料
        /// </summary>
        /// <param name="seqId">Banner流水號</param>
        /// <returns>刪除的結果</returns>
        Task<BaseReturnModel> Delete(string seqId);

        /// <summary>
        /// 更新Banner資料
        /// </summary>
        /// <param name="param">更新Banner資料的參數</param>
        /// <returns>更新後的Banner資料</returns>
        Task<BaseReturnModel> Update(SaveBannerParam param);

        /// <summary>
        /// 後台取得Banner資料
        /// </summary>
        /// <returns>Banner資料</returns>
        Task<BaseReturnDataModel<BannerInfo[]>> Get();

        /// <summary>
        /// 前端取得Banner資料
        /// </summary>
        /// <param name="dateTime">目前時間</param>
        /// <returns>Banner資料</returns>
        Task<BaseReturnDataModel<BannerInfo[]>> Get(DateTime dateTime);
    }
}