using Dapper;
using JxBackendService.Interface.Repository;
using JxBackendService.Model.Entity.ChatRoom;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.ChatRoom;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.ChatRoom;
using JxBackendService.Repository.Base;
using SLPolyGame.Web.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.Linq;
using static Dapper.SqlMapper;

namespace JxBackendService.Repository
{
    public class GiftBagRep : BaseDbRepository<GiftBagLog>, IGiftBagRep
    {
        public GiftBagRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType) { }

        /// <summary>
        /// 取得開運紅利1元紅包已領取的記錄
        /// </summary>
        /// <param name="userIds"></param>
        /// <param name="actType"></param>
        /// <returns></returns>
        public List<GiftBagLog> GetGiftBagByUserOpened(List<int?> userIds, int actType)
        {
            var sql = $@"
                SELECT ID, PrizeMoney, UserID, UserName, Msg
                , RiseTime, Processtime, ActType, Status, Memo
                ,SubUserID
                FROM {InlodbType.Inlodb.Value}.dbo.GiftBag_log WITH(NOLOCK)
                WHERE UserID IN @userIds
                AND ActType = @actType
                AND Processtime IS NOT NULL
                AND Processtime >= @startDateTime
				AND Processtime <= @endDateTime
                AND [Status] = 1
            ";

            var now = DateTime.Now;
            var startDateTime = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0, 0);
            var endDateTime = new DateTime(now.Year, now.Month, now.Day, 23, 59, 59, 999);

            return DbHelper.QueryList<GiftBagLog>(sql, new { userIds, actType , startDateTime, endDateTime });
        }
    }
}