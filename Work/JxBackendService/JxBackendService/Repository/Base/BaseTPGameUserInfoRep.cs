using JxBackendService.Common.Util;
using JxBackendService.Interface.Repository;
using JxBackendService.Model.Entity;
using JxBackendService.Model.Entity.Base;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Repository.Base;
using JxBackendService.Repository.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Repository.Base
{
    public abstract class BaseTPGameUserInfoRep<T> : BaseDbRepository<T>, ITPGameUserInfoRep<T> where T : BaseTPGameUserInfo
    {
        public BaseTPGameUserInfoRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType) { }

        public T GetDetail(int userId)
        {
            T tpGameUserInfo = (T)Activator.CreateInstance(typeof(T));
            tpGameUserInfo.UserID = userId;
            return GetSingleByKey(InlodbType.Inlodb, tpGameUserInfo);
        }

        public bool IsUserExists(int userId)
        {
            string sql = $"SELECT TOP 1 1 {GetFromTableSQL(InlodbType.Inlodb)} WHERE userId = @userId ";
            return DbHelper.QuerySingleOrDefault<int?>(sql, new { userId }).HasValue;
        }

        public bool CreateUser(int userId, string userName)
        {
            string insertSQL = ReflectUtil.GenerateInsertSQL<T>(InlodbType.Inlodb.Value, "dbo", ModelUtil.GetTableName(typeof(T)));
            var tpGameUserInfo = new BaseTPGameUserInfo()
            {
                UserID = userId,
                UserName = userName
            };

            return DbHelper.Execute(insertSQL, new {
                tpGameUserInfo.UserID,
                tpGameUserInfo.UserName,
                TransferIn = 0,
                TransferOut = 0,
                WinOrLoss = 0,
                Rebate = 0,
                AvailableScores = 0,
                FreezeScores = 0,
                LastUpdateTime = DateTime.Now
            }) > 0;
        }
    }
}
