using MMService.Models.Auth;
using MS.Core.MM.Models.Auth;
using MS.Core.MM.Models.Auth.Enums;
using MS.Core.MM.Models.Auth.ServiceReq;
using MS.Core.MMModel.Models.Auth;
using MS.Core.MMModel.Models.Post;
using MS.Core.Models;

namespace MMService.Services
{
    /// <summary>
    /// 權限相關
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// 產生token
        /// </summary>
        /// <param name="param">登入資訊</param>
        /// <param name="loginType">登入類型 *前台、後台</param>
        /// <returns></returns>
        Task<BaseReturnDataModel<SignInResponse>> GenerateToken(SignInData param, LoginType loginType);

        /// <summary>
        /// 取得認證資訊
        /// </summary>
        /// <param name="user">用戶id</param>
        /// <returns></returns>
        Task<BaseReturnDataModel<CertificationResponse>> CertificationInfo(ReqUserId user);

		/// <summary>
		/// 覓經紀申請
		/// </summary>
		/// <param name="model">申請資料</param>
		/// <returns></returns>
		Task<BaseReturnModel> AgentIdentityApply(ReqAgentIdentityApplyData model);

        /// <summary>
        /// 覓老闆申請
        /// </summary>
        /// <param name="model">申請資料</param>
        /// <returns></returns>
        Task<BaseReturnModel> BossIdentityApply(ReqBossIdentityApplyData model);
        /// <summary>
        /// 觅老板申请或者更新资料
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<BaseReturnModel> BossIdentityApplyOrUpdate(ReqBossApplyOrUpdateData model);

        /// <summary>
        /// 覓女郎申請
        /// </summary>
        /// <param name="user">用戶id</param>
        /// <returns></returns>
        Task<BaseReturnModel> GirlIdentityApply(ReqUserId user);
    }
}