using MS.Core.MM.Models.Entities.Post;
using MS.Core.MMModel.Models.Post.Enums;

namespace MS.Core.MM.Repos.interfaces
{
    public interface IAdvertisingContentRepo
    {
        Task<MMAdvertisingContent[]> Get();

        Task<MMAdvertisingContent> Get(int id);

        Task<bool> Update(MMAdvertisingContent param);

        Task Delete(int id);

        /// <summary>
        /// 取得贴子類型的宣傳文字
        /// </summary>
        /// <param name="contentType">宣傳類型</param>
        /// <returns></returns>
        Task<MMAdvertisingContent[]> GetByPostType(AdvertisingContentType contentType);

        /// <summary>
        /// 取得管理員聯繫方式
        /// </summary>
        /// <returns></returns>
        Task<string> GetAdminContact();

        /// <summary>
        /// 取得提示訊息
        /// </summary>
        /// <returns></returns>
        Task<string> GetTip(PostType postType);
    }
}