using Microsoft.AspNetCore.Mvc;
using MS.Core.MM.Models.Entities.IncomeExpense;
using MS.Core.MM.Models.Entities.PostTransaction;
using MS.Core.MM.Models.Entities.User;
using MS.Core.MM.Models.Filters;
using MS.Core.MMModel.Models.AdminIncomeExpense;
using MS.Core.MMModel.Models.AdminPostTransaction;
using MS.Core.MMModel.Models.AdminUserManager;
using MS.Core.MMModel.Models.Post.Enums;
using MS.Core.Models;
using MS.Core.Models.Models;

namespace MS.Core.MM.Repos.interfaces
{
    public interface IIdentityApplyRepo
    {
        /// <summary>
        /// 查询身份申请列表
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<PageResultModel<MMIdentityApply>> List(AdminUserManagerIdentityApplyListParam param);

        /// <summary>
        /// 查询觅老板和超觅老板得申请记录
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<PageResultModel<MMIdentityApply>> QueryBossOrSuperBossIdentityApplyRecord(AdminUserManagerIdentityApplyListParam param);
        /// <summary>
        /// 查询身份申请信息
        /// </summary>
        /// <param name="id">审核id</param>
        /// <returns></returns>
        Task<MMIdentityApply> Detail(string id);

        /// <summary>
        /// 查询身份申请信息ByUserId
        /// </summary>
        /// <param name="id">审核id</param>
        /// <returns></returns>
        Task<MMIdentityApply> DetailByUserId(int id);

        Task<MMIdentityApply> DetailByUserId(int id, int? status);
        /// <summary>
        /// 查询觅老板或者超级觅老板
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        Task<MMIdentityApply> QueryBossOrSuperBoss(int id, int? status);

        /// <summary>
        /// 身份申请审核
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<DBResult> UserIdentityAudit(AdminUserIdentityAuditParam param);
        /// <summary>
        /// 修改申请身份
        /// </summary>
        /// <param name="applyId"></param>
        /// <param name="applyIdentity"></param>
        /// <returns></returns>
        Task<bool> UpdateIdentityApply(string applyId, int applyIdentity);
    }
}