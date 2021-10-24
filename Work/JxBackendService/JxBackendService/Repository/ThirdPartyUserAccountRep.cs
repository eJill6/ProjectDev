using Dapper;
using JxBackendService.Interface.Repository;
using JxBackendService.Model.Entity;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Repository.Base;
using JxBackendService.Repository.Extensions;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace JxBackendService.Repository
{
    public class ThirdPartyUserAccountRep : BaseDbRepository<ThirdPartyUserAccount>, IThirdPartyUserAccountRep
    {
        public ThirdPartyUserAccountRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType) { }

        public List<ThirdPartyUserAccount> GetListByUserId(int userId)
        {
            string sql = GetAllQuerySQL(InlodbType.Inlodb) + "WHERE UserID = @userId ";
            return DbHelper.QueryList<ThirdPartyUserAccount>(sql, new { userId });
        }

        public ThirdPartyUserAccount GetSingleByUserId(int userId, PlatformProduct platformProduct)
        {
            return GetListByUserId(userId).FirstOrDefault(w => w.ThirdPartyType == platformProduct.Value);
        }

        public List<ThirdPartyUserAccount> GetListByTPGameAccount(PlatformProduct product, HashSet<string> tpGameAccounts)
        {
            string sql = GetAllQuerySQL(InlodbType.Inlodb) + "WHERE Account IN @Accounts ";
            DbString thirdPartyType = null;

            if (product != null)
            {
                sql += "AND ThirdPartyType = @ThirdPartyType ";
                thirdPartyType = product.Value.ToVarchar(10);
            }

            return DbHelper.QueryList<ThirdPartyUserAccount>(sql,
                new
                {
                    Accounts = tpGameAccounts.Select(s => s.ToNVarchar(50)),
                    thirdPartyType
                });
        }

        public List<ThirdPartyUserAccount> GetListByTPGameAccount(HashSet<string> tpGameAccounts)
        {
            return GetListByTPGameAccount(null, tpGameAccounts);
        }

        public int GetUserIdByTPGameAccount(string tpGameAccount)
        {
            string sql = GetAllQuerySQL(InlodbType.Inlodb, new List<string> { nameof(ThirdPartyUserAccount.UserID) }) + "WHERE Account = @Account ";
            return DbHelper.QueryFirstOrDefault<int>(sql,
                new
                {
                    Accounts = tpGameAccount.ToNVarchar(50),
                });
        }

    }
}
