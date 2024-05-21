using MS.Core.MM.Models.Post;
using MS.Core.Models;

namespace MS.Core.MM.Services.interfaces
{
    public interface IPostTransactionService
    {
        /// <summary>
        /// 贴子解鎖(免費解鎖/花幣購買)
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="postId"></param>
        /// <returns></returns>
        Task<BaseReturnDataModel<ResPostTransaction>> PostTransaction(ReqPostTransaction res);
    }
}
