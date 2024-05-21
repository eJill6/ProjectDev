using Castle.Core.Internal;
using JxBackendService.Interface.Repository.Chat;
using JxBackendService.Model.Entity.Chat;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Param.Chat;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.StoredProcedureParam.Chat;
using JxBackendService.Model.ViewModel;
using JxBackendService.Repository.Base;
using JxBackendService.Repository.Extensions;
using System.Collections.Generic;
using System.Data;

namespace JxBackendService.Repository.Chat
{
    public class MSIMLastMessageInfoRep : BaseDbRepository<MSIMLastMessageInfo>, IMSIMLastMessageInfoRep
    {
        public MSIMLastMessageInfoRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
        }

        public BaseReturnModel ClearUnreadCount(int ownerUserId, string roomId)
        {
            string sql = $"{InlodbType.Inlodb}.dbo.Pro_ClearMSIMLastMessageInfoUnreadCount";

            return DbHelper
                .QuerySingle<SPReturnModel>(sql, new ProClearUnreadCountParam
                {
                    OwnerUserID = ownerUserId,
                    RoomID = roomId,
                }, CommandType.StoredProcedure).ToBaseReturnModel();
        }

        public List<MSIMLastMessageInfo> GetMSIMLastMessageInfos(QueryLastMessagesParam queryLastMessagesParam, int fetchCount)
        {
            string sql = GetAllQuerySQL(InlodbType.Inlodb, fetchCount) + @"
                WHERE OwnerUserID = @OwnerUserID ";

            if (!queryLastMessagesParam.RoomID.IsNullOrEmpty())
            {
                sql += "AND RoomID = @RoomID ";
            }

            if (queryLastMessagesParam.LastMessageID.HasValue)
            {
                sql += "AND MessageID < @LastMessageID ";
            }

            if (queryLastMessagesParam.PublishTimestamp.HasValue)
            {
                sql += "AND PublishTimestamp <= @PublishTimestamp ";
            }

            sql += "ORDER BY MessageID DESC ";

            return DbHelper.QueryList<MSIMLastMessageInfo>(sql,
                new
                {
                    queryLastMessagesParam.OwnerUserID,
                    RoomID = queryLastMessagesParam.RoomID.ToVarchar(50),
                    queryLastMessagesParam.LastMessageID,
                    queryLastMessagesParam.PublishTimestamp,
                });
        }

        public bool HasUnreadMessage(int ownerUserId)
        {
            string sql = "SELECT TOP 1 1 " +
                GetFromTableSQL(InlodbType.Inlodb) +
                "WHERE OwnerUserID = @OwnerUserID AND UnreadCount > 0 ";

            return DbHelper.ExecuteScalar<int?>(sql, new { ownerUserId }).HasValue;
        }

        public List<MSIMLastMessageKey> GetLastMessageKeys(string messageIDsJson)
        {
            string sql = $@"
                DROP TABLE IF EXISTS #tmpMSIMLastMessageInfoMessageID

                CREATE TABLE #tmpMSIMLastMessageInfoMessageID (
					MessageID BIGINT);

                INSERT INTO #tmpMSIMLastMessageInfoMessageID (MessageID)
                SELECT MessageID
                FROM OPENJSON(@messageIDsJson)
                WITH( MessageID BIGINT '$')

                SELECT
					LM.{nameof(MSIMLastMessageInfo.OwnerUserID)},
					LM.{nameof(MSIMLastMessageInfo.RoomID)},
					LM.{nameof(MSIMLastMessageInfo.MessageID)}
				FROM {nameof(InlodbType.Inlodb)}.dbo.{nameof(MSIMLastMessageInfo)} LM WITH(NOLOCK)
                JOIN #tmpMSIMLastMessageInfoMessageID AS TLM ON
                    LM.{nameof(MSIMLastMessageInfo.MessageID)} = TLM.{nameof(MSIMLastMessageInfo.MessageID)}";

            return DbHelper.QueryList<MSIMLastMessageKey>(sql, new { messageIDsJson });
        }

        public void DeleteLastMessages(string lastMessageKeysJson)
        {
            string sql = $"{InlodbType.Inlodb}.dbo.Pro_DeleteLastMessages";

            DbHelper.Execute(sql, new { LastMessageKeysJson = lastMessageKeysJson.ToVarchar(-1) }, CommandType.StoredProcedure);
        }
    }
}