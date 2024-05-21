using MS.Core.MM.Models.Entities.PostTransaction;
using MS.Core.Models;

namespace MS.Core.MM.Repos.interfaces
{
    public interface IPostTransactionRepo
    {
        Task UnlockPost(UnlockPostInfoModel unlockPost);

        Task UseUserPointUnlockPost(UseUserPointUnlockPostModel unlockPost);

        Task<IEnumerable<string>> GetExpensePosts(int userId);

        Task<string> GetSequenceIdentity<T>() where T : BaseDBModel;

        /// <summary>
        /// 取得解鎖數量
        /// </summary>
        /// <param name="userId">使用者編號</param>
        /// <returns>解鎖數量</returns>
        Task<int> GetUnlockCount(int userId);
        Task<IEnumerable<string>> GetUnlockPostId(int userId);

        /// <summary>
        /// 用戶是否購買贴子
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="postId"></param>
        /// <returns></returns>
        Task<bool> IsUserBuyPost(int userId, string postId);

        /// <summary>
        /// 由特定編號來取得解鎖紀錄
        /// </summary>
        /// <param name="ids">解鎖編號</param>
        /// <param name="idType">編號的類別 0:解鎖編號, 1:贴子編號, 2:投訴編號</param>
        /// <returns>解鎖紀錄</returns>
        Task<MMPostTransactionModel[]> List(string[] ids, int idType);

        /// <summary>
        /// User 發贴數
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<IEnumerable<QueryUserPostUnlockCount>> PostUnlockCountByUser(int userId);
        Task<MMPostTransactionModel?> GetUserPostTran(int userId, string postId);
    }
}