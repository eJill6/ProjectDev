using MS.Core.MM.Models.Entities.Post;
using MS.Core.MM.Models.Post;
using MS.Core.MMModel.Models.Post.Enums;
using MS.Core.Models;

namespace MS.Core.MM.Services.Bases
{
    public interface IOptionsService
    {
        /// <summary>
        /// 創建Options資料
        /// </summary>
        /// <param name="param">新增Options的參數</param>
        /// <returns>創建後的Options資料</returns>
        Task<BaseReturnModel> Create(CreateOptionsParam param);

        /// <summary>
        /// 刪除Options資料
        /// </summary>
        /// <param name="seqId">Options流水號</param>
        /// <returns>刪除的結果</returns>
        Task<BaseReturnModel> Delete(int OptionId);

        /// <summary>
        /// 更新Options資料
        /// </summary>
        /// <param name="param">更新Options資料的參數</param>
        /// <returns>更新後的Options資料</returns>
        Task<BaseReturnModel> Update(UpdateOptionsParam param);

        /// <summary>
        /// 從頁面獲取選項
        /// </summary>
        /// <param name="postType"></param>
        /// <returns></returns>
        Task<BaseReturnDataModel<MMOptions[]>> GetOptionByPostType(int postType);

        /// <summary>
        /// 從頁面獲取選項(後台使用)
        /// </summary>
        /// <returns></returns>
        Task<BaseReturnDataModel<MMOptions[]>> GetOptionsByPostTypeAndOptionType(PostType postType, OptionType optionType, int? OptionId);
    }
}