using JxBackendService.Common.Util;
using JxBackendService.Interface.Repository.Chat;
using JxBackendService.Model.Entity.Chat;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.Chat;
using JxBackendService.Model.Paging;
using JxBackendService.Model.Param.Chat;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.StoredProcedureParam.Chat;
using JxBackendService.Model.ViewModel;
using JxBackendService.Repository.Base;
using JxBackendService.Repository.Extensions;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace JxBackendService.Repository.Chat
{
    public class MSIMOneOnOneChatMessageRep : BaseDbRepository<MSIMOneOnOneChatMessage>, IMSIMOneOnOneChatMessageRep
    {
        public MSIMOneOnOneChatMessageRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
        }

        public List<MSIMOneOnOneChatMessage> GetRoomMessages(int ownerUserId, QueryRoomMessageParam queryMessageParam, int fetchCount)
        {
            StringBuilder sql = new StringBuilder(GetAllQuerySQL(InlodbType.Inlodb, fetchCount) + @"
                WHERE
                    OwnerUserID = @OwnerUserID AND
                    DialogueUserID = @DialogueUserID ");

            if (!queryMessageParam.LastMessageID.HasValue)
            {
                queryMessageParam.LastMessageID = long.MaxValue;
                queryMessageParam.SearchDirectionTypeValue = SearchDirectionType.Backward.Value;
            }

            SortOrder sortOrder = SortOrder.Ascending;

            if (queryMessageParam.SearchDirectionTypeValue == SearchDirectionType.Forward)
            {
                sql.AppendLine("AND MessageID > @LastMessageID ");
            }
            else
            {
                sql.AppendLine("AND MessageID < @LastMessageID ");
                sortOrder = SortOrder.Descending;
            }

            sql.AppendLine($"ORDER BY MessageID {sortOrder.ToSortOrderText()} ");

            return DbHelper.QueryList<MSIMOneOnOneChatMessage>(
                sql.ToString(), new
                {
                    OwnerUserID = ownerUserId,
                    DialogueUserID = queryMessageParam.RoomID.ToInt32(),
                    LastMessageID = queryMessageParam.LastMessageID.Value
                });
        }

        public BaseReturnModel SaveOneOnOneChatMessage(ProSaveOneOnOneChatMessageParam proSaveOneOnOneChatMessageParam)
        {
            string sql = $"{InlodbType.Inlodb}.dbo.Pro_SaveOneOnOneChatMessage";

            return DbHelper
                .QuerySingle<SPReturnModel>(sql, proSaveOneOnOneChatMessageParam, CommandType.StoredProcedure)
                .ToBaseReturnModel();
        }

        public List<MSIMOneOnOneChatMessageKey> GetOneOnOneChatMessageKeys(QueryOneOnOneMessageParam queryParam, int fetchCount)
        {
            var selectColumns = new List<string>()
            {
                nameof(MSIMOneOnOneChatMessage.MessageID),
                nameof(MSIMOneOnOneChatMessage.DialogueUserID),
                nameof(MSIMOneOnOneChatMessage.OwnerUserID)
            };

            StringBuilder sql = new StringBuilder(GetAllQuerySQL(InlodbType.Inlodb, fetchCount, selectColumns) + @"
                WHERE PublishTimestamp <= @PublishTimestamp ");

            if (queryParam.BothChatUserIDParam != null)
            {
                sql.AppendLine($@"AND ((OwnerUserID = @OwnerUserID AND DialogueUserID = @DialogueUserID)
                    OR (OwnerUserID = @DialogueUserID AND DialogueUserID = @OwnerUserID)) ");
            }

            return DbHelper.QueryList<MSIMOneOnOneChatMessageKey>(
                sql.ToString(),
                new
                {
                    queryParam?.BothChatUserIDParam?.OwnerUserID,
                    queryParam?.BothChatUserIDParam?.DialogueUserID,
                    queryParam.PublishTimestamp,
                });
        }

        public void DeleteChatMessages(string oneOnOneChatKeysJson)
        {
            string sql = $"{InlodbType.Inlodb}.dbo.Pro_DeleteChatMessages";

            DbHelper.Execute(sql, new { OneOnOneChatKeysJson = oneOnOneChatKeysJson.ToVarchar(-1) }, CommandType.StoredProcedure);
        }
    }
}