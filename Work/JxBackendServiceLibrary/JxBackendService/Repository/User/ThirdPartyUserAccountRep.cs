using Dapper;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Repository;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.Enums;
using JxBackendService.Model.Common;
using JxBackendService.Model.Entity;
using JxBackendService.Model.Entity.Base;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Repository.Base;
using JxBackendService.Repository.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using static Dapper.SqlMapper;

namespace JxBackendService.Repository.User
{
    public class ThirdPartyUserAccountRep : BaseDbRepository<ThirdPartyUserAccount>, IThirdPartyUserAccountRep
    {
        private readonly IPlatformProductService _platformProductService;

        public ThirdPartyUserAccountRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _platformProductService = DependencyUtil.ResolveKeyed<IPlatformProductService>(envLoginUser.Application, SharedAppSettings.PlatformMerchant);
        }

        public List<ThirdPartyUserAccount> GetListByUserId(int userId)
        {
            return SearchListByUserId(userId, platformProduct: null);
        }

        public ThirdPartyUserAccount GetSingleByUserId(int userId, PlatformProduct platformProduct)
        {
            return SearchListByUserId(userId, platformProduct).SingleOrDefault();
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

        public Dictionary<PlatformProduct, BaseTPGameUserInfo> GetAllTPGameUserInfoMap(int userId)
        {
            List<PlatformProduct> platformProducts = _platformProductService.GetNonSelfProduct();

            var sqls = new List<string>();
            //因為PlatformProduct有加入彩票，所以這裡只取第三方的產品列表
            foreach (PlatformProduct product in platformProducts)
            {
                ITPGameUserInfoService tpGameUserInfoService = GetTPGameUserInfoService(product);
                sqls.Add(tpGameUserInfoService.GetQuerySingleSQL());
            }

            string allSql = string.Join(Environment.NewLine, sqls);
            var productUserMap = new Dictionary<PlatformProduct, BaseTPGameUserInfo>();

            DbHelper.QueryMultiple(
                allSql,
                new { userId },
                (GridReader gridReader) =>
                {
                    foreach (PlatformProduct product in platformProducts)
                    {
                        ITPGameUserInfoService tpGameUserInfoService = GetTPGameUserInfoService(product);
                        BaseTPGameUserInfo baseTPGameUserInfo = gridReader.ReadSingleOrDefault(tpGameUserInfoService.GetUserInfoType()) as BaseTPGameUserInfo;

                        if (baseTPGameUserInfo != null)
                        {
                            productUserMap.Add(product, baseTPGameUserInfo);
                        }
                    }
                });

            return productUserMap;
        }

        private ITPGameUserInfoService GetTPGameUserInfoService(PlatformProduct product)
        {
            ITPGameUserInfoService tpGameUserInfoService = DependencyUtil
                .ResolveJxBackendService<ITPGameUserInfoService>(product, SharedAppSettings.PlatformMerchant, EnvLoginUser, DBConnectionType);

            return tpGameUserInfoService;
        }

        public List<ThirdPartyUserAccount> GetListByUserIdsAndThirdPartyType(string thirdPartyType, IEnumerable<int> userIds)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("userIds", userIds);
            parameters.Add("thirdPartyType", thirdPartyType.ToVarchar(10));

            string whereSql = @"WHERE   UserID          IN  @userIds
                                AND     ThirdPartyType  =   @thirdPartyType";

            string sql = GetAllQuerySQL(InlodbType.Inlodb) + whereSql;

            return DbHelper.QueryList<ThirdPartyUserAccount>(sql, parameters);
        }

        private List<ThirdPartyUserAccount> SearchListByUserId(int userId, PlatformProduct platformProduct)
        {
            string sql = GetAllQuerySQL(InlodbType.Inlodb) + "WHERE UserID = @userId ";
            string thirdPartyType = null;

            if (platformProduct != null)
            {
                sql += "AND ThirdPartyType = @ThirdPartyType ";
                thirdPartyType = platformProduct.Value;
            }

            return DbHelper.QueryList<ThirdPartyUserAccount>(
                sql,
                new
                {
                    userId,
                    ThirdPartyType = thirdPartyType.ToVarchar(10)
                });
        }
    }
}