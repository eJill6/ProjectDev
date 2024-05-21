using MS.Core.MM.Model.Banner;
using MS.Core.MM.Models.Entities.BossShop;
using MS.Core.MM.Models.Entities.User;
using MS.Core.MMModel.Models.AdminUserManager;
using MS.Core.Models.Models;
using MS.Core.MM.Models.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS.Core.MM.Repos.interfaces
{
	public interface IBossShopRepo
	{
		Task<bool> Create(MMBossShop param);

		Task<string> GetSequenceIdentity();

		/// <summary>
		/// 查询店铺编辑列表
		/// </summary>
		/// <param name="param"></param>
		/// <returns></returns>
		Task<PageResultModel<MMBossShop>> List(AdminBossShopListParam param);

		/// <summary>
		/// 获取店铺编辑信息
		/// </summary>
		/// <param name="predicate"></param>
		/// <returns></returns>
		Task<MMBossShop> GetBossShopDetail(string id);

		/// <summary>
		/// 获取当前店铺信息
		/// </summary>
		/// <param name="predicate"></param>
		/// <returns></returns>
		Task<MMBoss> GetBossDetail(string id);

		/// <summary>
		/// 获取当前店铺信息用applyId
		/// </summary>
		/// <param name="predicate"></param>
		/// <returns></returns>
		Task<MMBoss> GetBossDetailByApplyId(string applyId);
		/// <summary>
		/// 修改店铺平台分成
		/// </summary>
		/// <param name="applyId"></param>
		/// <param name="platformSharing"></param>
		/// <returns></returns>
		Task<bool> UpdateBossPlatformSharing(MMBoss info);
		/// <summary>
		/// 新增店铺信息
		/// </summary>
		/// <param name="info"></param>
		/// <returns></returns>
		Task<bool> InsertBossInfo(MMBoss info);

        /// <summary>
        /// 获取当前店铺信息
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<MMBossHistory> GetBossHistoryDetail(string id);

		/// <summary>
		/// 审核店铺编辑信息
		/// </summary>
		/// <returns></returns>
		Task<bool> BossShopAudit(AdminUserBossParam param);

		Task<IEnumerable<MMBossShop>> GetBossShopByFilter(QuerySingleBossShopFilter query);

		Task<bool> Update(MMBossShop param);

		/// <summary>
		/// 查询会员店铺列表
		/// </summary>
		/// <param name="param"></param>
		/// <returns></returns>
		Task<PageResultModel<AdminStoreManageList>> GetStoreList(AdminStoreManageParam param);
	}
}