using Dapper;
using JxBackendService.Interface.Repository;
using JxBackendService.Model.Entity.ChatRoom;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.ChatRoom;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.ChatRoom;
using JxBackendService.Repository.Base;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.Linq;
using static Dapper.SqlMapper;

namespace JxBackendService.Repository
{
    public class ChatRoomRep : BaseDbRepository<LettersGroupSetting>, IChatRoomRep
    {
        public ChatRoomRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType) { }

        /// <summary>
        /// 取得[人員]聊天室列表
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="isNeedSpecialMembers"></param>
        /// <param name="excludeUserIds"></param>
        /// <returns></returns>
        public BaseReturnDataModel<MemberChatRoomSqlResultModel> GetMemberChatRoomList(int userId, bool isNeedSpecialMembers , int[] excludeUserIds)
        {
            SPReturnModel spReturnModel = new SPReturnModel();
            MemberChatRoomSqlResultModel memberChatRoomResult = new MemberChatRoomSqlResultModel();

            var excludeUserIdTable = CreateTVPExcludeUserId(excludeUserIds);

            DbHelper.QueryMultiple(
                "Pro_GetDownlineMembersBySortUnRead",
                new
                {
                    UserId = userId,
                    IsNeedSpecialMembers = isNeedSpecialMembers,
                    ExcludeUserIds = excludeUserIdTable,
                },
                System.Data.CommandType.StoredProcedure ,
                (reader) =>
                {
                    spReturnModel = reader.ReadSingleOrDefault<SPReturnModel>();

                    //若SP有邏輯上的錯誤，只會回傳 ReturnCode、ErrorMessage，所以這裡判斷是否還有下一個查詢資料，避免Dapper錯誤
                    if (!reader.IsConsumed)
                    {
                        memberChatRoomResult.ChildChatRoomList = reader.Read<MemberChatRoomSqlRawModel>().ToList();
                        memberChatRoomResult.ParentChatRoomList = reader.Read<MemberChatRoomSqlRawModel>().ToList();
                    }
                });

            //Sp的ReturnCode轉換成共用層的ReturnCode統一訊息
            ChatRoomSpReturnCode chatRoomSpReturnCode = ChatRoomSpReturnCode.GetSingle(spReturnModel.ReturnCode);

            return new BaseReturnDataModel<MemberChatRoomSqlResultModel>(chatRoomSpReturnCode.ReturnCode, memberChatRoomResult);
        }

        /// <summary>
        /// 取得[群聊]聊天室資訊
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public BaseReturnDataModel<List<GroupChatRoomSqlRawModel>> GetGroupChatRoomList(int userId)
        {
            SPReturnModel spReturnModel = new SPReturnModel();

            List<GroupChatRoomSqlRawModel> groupChatRoomResultList = new List<GroupChatRoomSqlRawModel>();

            DbHelper.QueryMultiple(
                "Pro_GetAllLettersGroupInformation",
                new
                {
                    UserId = userId
                },
                System.Data.CommandType.StoredProcedure,
                (reader) =>
                {
                    spReturnModel = reader.ReadSingleOrDefault<SPReturnModel>();

                    //若SP有邏輯上的錯誤，只會回傳 ReturnCode、ErrorMessage，所以這裡判斷是否還有下一個查詢資料，避免Dapper錯誤
                    if (!reader.IsConsumed)
                    {
                        groupChatRoomResultList = reader.Read<GroupChatRoomSqlRawModel>().ToList();
                    }
                });

            //Sp的ReturnCode轉換成共用層的ReturnCode統一訊息
            ChatRoomSpReturnCode chatRoomSpReturnCode = ChatRoomSpReturnCode.GetSingle(spReturnModel.ReturnCode);

            return new BaseReturnDataModel<List<GroupChatRoomSqlRawModel>>(chatRoomSpReturnCode.ReturnCode, groupChatRoomResultList);
        }

        public BaseReturnDataModel<GroupChatRoomChildListResultModel> GetGroupChatRoomChildList(int userId, int chatRoomId)
        {
            SPReturnModel spReturnModel = new SPReturnModel();

            GroupChatRoomChildListResultModel resultModel = new GroupChatRoomChildListResultModel();

            DbHelper.QueryMultiple(
                "Pro_GetLettersGroupMembersInfo",
                new
                {
                    UserId = userId,
                    GroupId = chatRoomId
                },
                System.Data.CommandType.StoredProcedure,
                (reader) =>
                {
                    spReturnModel = reader.ReadSingleOrDefault<SPReturnModel>();

                    //若SP有邏輯上的錯誤，只會回傳 ReturnCode、ErrorMessage，所以這裡判斷是否還有下一個查詢資料，避免Dapper錯誤
                    if (!reader.IsConsumed)
                    {
                        resultModel.GroupChatRoomSetting = reader.ReadSingleOrDefault<GroupChatRoomSettingSqlRawModel>();
                        resultModel.GroupChatRoomChildList = reader.Read<GroupChatRoomChildSqlRawModel>().ToList();
                    }
                });

            //Sp的ReturnCode轉換成共用層的ReturnCode統一訊息
            ChatRoomSpReturnCode chatRoomSpReturnCode = ChatRoomSpReturnCode.GetSingle(spReturnModel.ReturnCode);

            return new BaseReturnDataModel<GroupChatRoomChildListResultModel>(chatRoomSpReturnCode.ReturnCode, resultModel);
        }

        public bool CheckChatRoomIsExist(int chatRoomId)
        {
            //檢查audittype 未處理的，是否有同單號的
            string sql = $" SELECT COUNT(1) " +
                         $" {GetFromTableSQL(InlodbType.Inlodb, "LettersGroupSetting" )} " +
                         $" WHERE GroupId = @GroupId " +
                         $" AND IsActive = 1 ";

            return DbHelper.QuerySingle<int>(sql, new
            {
                GroupId = chatRoomId
            }) > 0;
        }

        public LettersGroupSetting GetGroupChatRoomSetting(int chatRoomId)
        {
            string sql = GetAllQuerySQL(InlodbType.Inlodb, "LettersGroupSetting") + " WHERE GroupId = @GroupId ";
            return DbHelper.QuerySingleOrDefault<LettersGroupSetting>(sql, new { GroupId = chatRoomId });
        }

        public BaseReturnDataModel<GroupChatRoomConfigSettingSqlRawModel> GetChatRoomConfigSetting()
        {
            SPReturnModel spReturnModel = new SPReturnModel();

            GroupChatRoomConfigSettingSqlRawModel resultModel = new GroupChatRoomConfigSettingSqlRawModel();

            DbHelper.QueryMultiple(
                @"
                	DECLARE @MaxCreateLettersGroupCount INT;  
                    DECLARE @MaxPublishLettersPerMinute INT;  
                    DECLARE @MaxPersonCountInPerGroup INT;  

	                EXEC [Inlodb].[dbo].[Pro_GetLettersConfigSettings] 
                        @MaxCreateLettersGroupCount = @MaxCreateLettersGroupCount OUTPUT, 
                        @MaxPublishLettersPerMinute = @MaxPublishLettersPerMinute OUTPUT,
                        @MaxPersonCountInPerGroup = @MaxPersonCountInPerGroup OUTPUT

                    SELECT @MaxCreateLettersGroupCount AS MaxCreateLettersGroupCount , 
	                       @MaxPublishLettersPerMinute AS MaxPublishLettersPerMinute, 
	                       @MaxPersonCountInPerGroup AS MaxPersonCountInPerGroup
                ",
                new { },
                (reader) =>
                {
                    spReturnModel = reader.ReadSingleOrDefault<SPReturnModel>();

                    //若SP有邏輯上的錯誤，只會回傳 ReturnCode、ErrorMessage，所以這裡判斷是否還有下一個查詢資料，避免Dapper錯誤
                    if (!reader.IsConsumed)
                    {
                        resultModel = reader.ReadSingleOrDefault<GroupChatRoomConfigSettingSqlRawModel>();
                    }
                });

            //Sp的ReturnCode轉換成共用層的ReturnCode統一訊息
            ChatRoomSpReturnCode chatRoomSpReturnCode = ChatRoomSpReturnCode.GetSingle(spReturnModel.ReturnCode);

            return new BaseReturnDataModel<GroupChatRoomConfigSettingSqlRawModel>(chatRoomSpReturnCode.ReturnCode, resultModel);
        }

        public BaseReturnDataModel<GroupChatRoomMessageListResultModel> GetGroupChatRoomMessageList(int userId, int chatRoomId, int queryType, long? messageId)
        {
            SPReturnModel spReturnModel = new SPReturnModel();

            GroupChatRoomMessageListResultModel resultModel = new GroupChatRoomMessageListResultModel();

            DbHelper.QueryMultiple(
                "Pro_GetLettersGroupMessage",
                new
                {
                    UserIdInGroup = userId,
                    GroupId = chatRoomId,
                    MessageId = (messageId.HasValue) ? messageId.Value : -1 ,
                    ScrollBarDirection = queryType
                },
                System.Data.CommandType.StoredProcedure,
                (reader) =>
                {
                    spReturnModel = reader.ReadSingleOrDefault<SPReturnModel>();

                    //若SP有邏輯上的錯誤，只會回傳 ReturnCode、ErrorMessage，所以這裡判斷是否還有下一個查詢資料，避免Dapper錯誤
                    if (!reader.IsConsumed)
                    {
                        resultModel.GroupChatRoomChildInfo = reader.ReadSingleOrDefault<GroupChatRoomChildSqlRawModel>();
                        resultModel.GroupChatRoomMessageList = reader.Read<GroupChatRoomMessageSqlRawModel>().ToList();
                    }                   
                });

            //Sp的ReturnCode轉換成共用層的ReturnCode統一訊息
            ChatRoomSpReturnCode chatRoomSpReturnCode = ChatRoomSpReturnCode.GetSingle(spReturnModel.ReturnCode);

            return new BaseReturnDataModel<GroupChatRoomMessageListResultModel>(chatRoomSpReturnCode.ReturnCode, resultModel);
        }

        public BaseReturnDataModel<GroupChatRoomMessageListResultModel> SendGroupChatRoomMessage(int userId, string userName, string messageContent, int chatRoomId)
        {
            SPReturnModel spReturnModel = new SPReturnModel();

            GroupChatRoomMessageListResultModel resultModel = new GroupChatRoomMessageListResultModel();

            DbHelper.QueryMultiple(
                "Pro_PublishGroupMessage",
                new
                {
                    PublishUserId = userId,
                    PublishUserName = userName,
                    PublishMessageContent = messageContent,
                    BelongGroupId = chatRoomId,
                    CurrentNewestMessageId = long.MaxValue
                },
                System.Data.CommandType.StoredProcedure,
                (reader) =>
                {
                    spReturnModel = reader.ReadSingleOrDefault<SPReturnModel>();

                    //若SP有邏輯上的錯誤，只會回傳 ReturnCode、ErrorMessage，所以這裡判斷是否還有下一個查詢資料，避免Dapper錯誤
                    if (!reader.IsConsumed)
                    {
                        resultModel.GroupChatRoomChildInfo = reader.ReadSingleOrDefault<GroupChatRoomChildSqlRawModel>();
                        resultModel.GroupChatRoomMessageList = reader.Read<GroupChatRoomMessageSqlRawModel>().ToList();
                    }                   
                });

            //Sp的ReturnCode轉換成共用層的ReturnCode統一訊息
            ChatRoomSpReturnCode chatRoomSpReturnCode = ChatRoomSpReturnCode.GetSingle(spReturnModel.ReturnCode);

            return new BaseReturnDataModel<GroupChatRoomMessageListResultModel>(chatRoomSpReturnCode.ReturnCode, resultModel);
        }

        public BaseReturnDataModel<int> CreateGroupChatRoom(int userId, string chatRoomName, int[] childUserIds)
        {
            SPReturnModel spReturnModel = new SPReturnModel();

            int newGroupId = -1;

            var memberTable = ToCreateGroupChatRoomMemberTable(childUserIds);

            DbHelper.QueryMultiple(
                "Pro_CreateLettersGroup",
                new
                {
                    UserId = userId,
                    GroupName = chatRoomName,
                    LettersGroupMembers = memberTable
                },
                System.Data.CommandType.StoredProcedure,
                (reader) =>
                {
                    spReturnModel = reader.ReadSingleOrDefault<SPReturnModel>();

                    //若有邏輯上的錯誤，只會回傳 ReturnCode、ErrorMessage，所以這裡判斷是否還有下一個查詢資料，避免Dapper錯誤
                    if (!reader.IsConsumed)
                    {
                        newGroupId = reader.ReadSingleOrDefault<int>();
                    }
                });

            //Sp的ReturnCode轉換成共用層的ReturnCode統一訊息
            ChatRoomSpReturnCode chatRoomSpReturnCode = ChatRoomSpReturnCode.GetSingle(spReturnModel.ReturnCode);

            return new BaseReturnDataModel<int>(chatRoomSpReturnCode.ReturnCode, newGroupId);
        }

        public BaseReturnModel AddGroupChatRoomChild(int userId, int chatRoomId, string chatRoomName, int[] childUserIds)
        {
            var memberTable = ToAddGroupChatRoomChildMemberTable(chatRoomId, childUserIds);

            return UpdateLettersGroupSetting(userId, chatRoomId, chatRoomName, memberTable);
        }

        public BaseReturnModel UpdateGroupChatRoomChildEnableSendMessage(int userId, int chatRoomId, string chatRoomName, int childUserId, bool enableSendMessage)
        {
            var memberTable = ToUpdateGroupChatRoomChildEnableSendMessageMemberTable(chatRoomId, childUserId, enableSendMessage);

            return UpdateLettersGroupSetting(userId, chatRoomId, chatRoomName, memberTable);
        }

        public BaseReturnModel DeleteGroupChatRoomChild(int userId, int chatRoomId, string chatRoomName, int childUserId)
        {
            var memberTable = ToDeleteGroupChatRoomChildMemberTable(chatRoomId, childUserId);

            return UpdateLettersGroupSetting(userId, chatRoomId, chatRoomName, memberTable);
        }

        public BaseReturnModel UpdateGroupChatRoomName(int userId, int chatRoomId, string newChatRoomName)
        {
            var memberTable = CreateTVPLettersGroupMember(null);

            return UpdateLettersGroupSetting(userId, chatRoomId, newChatRoomName, memberTable);
        }

        public BaseReturnModel DeleteGroupChatRoom(int userId, int chatRoomId)
        {
            SPReturnModel spReturnModel = new SPReturnModel();

            DbHelper.QueryMultiple(
                "Pro_DisbandLettersGroup",
                new
                {
                    GroupId = chatRoomId,
                    UpdatedUserId = userId
                },
                System.Data.CommandType.StoredProcedure,
                (reader) =>
                {
                    spReturnModel = reader.ReadSingleOrDefault<SPReturnModel>();
                });

            //Sp的ReturnCode轉換成共用層的ReturnCode統一訊息
            ChatRoomSpReturnCode chatRoomSpReturnCode = ChatRoomSpReturnCode.GetSingle(spReturnModel.ReturnCode);

            return new BaseReturnModel(chatRoomSpReturnCode.ReturnCode);
        }

        public BaseReturnDataModel<int> GetAllUnReadMessageCount(int userId)
        {
            SPReturnModel spReturnModel = new SPReturnModel();

            int unReadMessageCount = 0;

            DbHelper.QueryMultiple(
                "Pro_GetAllUnReadLettersMessageCount",
                new
                {
                    UserId = userId
                },
                System.Data.CommandType.StoredProcedure,
                (reader) =>
                {
                    spReturnModel = reader.ReadSingleOrDefault<SPReturnModel>();

                    //若SP有邏輯上的錯誤，只會回傳 ReturnCode、ErrorMessage，所以這裡判斷是否還有下一個查詢資料，避免Dapper錯誤
                    if (!reader.IsConsumed)
                    {
                        unReadMessageCount = reader.ReadSingleOrDefault<int>();
                    }
                });

            //Sp的ReturnCode轉換成共用層的ReturnCode統一訊息
            ChatRoomSpReturnCode chatRoomSpReturnCode = ChatRoomSpReturnCode.GetSingle(spReturnModel.ReturnCode);

            return new BaseReturnDataModel<int>(chatRoomSpReturnCode.ReturnCode, unReadMessageCount);
        }

        public int UpdateAllUnReadMessageToRead(int userId)
        {
            string sql = $@"
                            --[人員]
                            UPDATE [Message]
                                SET ReadTag = 1
                                WHERE RCUserId = @userId and ReadTag = 0

                            --[群聊]
                            UPDATE [LettersGroupMembers]
                            SET	LastReadingDateTime = GETDATE(), LastReadMsgSeqId = GRP.LastMsgSeqId
                            FROM (
		                            SELECT  LGS.GroupId AS GroupId , ISNULL(MAX(LGS.LastMsgSeqId), 0) AS LastMsgSeqId
		                            FROM [LettersGroupMembers] LGM WITH(NOLOCK) 
		                            INNER JOIN [LettersGroupSetting] LGS WITH(NOLOCK) ON LGM.GroupId = LGS.GroupId
		                            WHERE LGM.UserIdInGroup = @userId
		                            GROUP BY LGS.GroupId
	                             ) AS GRP
                            WHERE LettersGroupMembers.GroupId = GRP.GroupId ";

            return DbHelper.Execute(sql, new
            {
                userId
            });
        }

        private BaseReturnModel UpdateLettersGroupSetting(int userId, int chatRoomId, string chatRoomName, ICustomQueryParameter lettersGroupMembers)
        {
            SPReturnModel spReturnModel = new SPReturnModel();

            DbHelper.QueryMultiple(
                "Pro_UpdateLettersGroupSetting",
                new
                {
                    UpdatedGroupId = chatRoomId,
                    UpdatedGroupName = chatRoomName,
                    UpdatedUserId = userId,
                    LettersGroupMembers = lettersGroupMembers
                },
                System.Data.CommandType.StoredProcedure,
                (reader) =>
                {
                    spReturnModel = reader.ReadSingleOrDefault<SPReturnModel>();
                });

            //Sp的ReturnCode轉換成共用層的ReturnCode統一訊息
            ChatRoomSpReturnCode chatRoomSpReturnCode = ChatRoomSpReturnCode.GetSingle(spReturnModel.ReturnCode);

            return new BaseReturnModel(chatRoomSpReturnCode.ReturnCode);
        }

        #region 轉換 TVP格式

        private ICustomQueryParameter CreateTVPExcludeUserId(int[] excludeUserIds)
        {
            var excludeUserIdTable = new DataTable();
            excludeUserIdTable.Columns.Add("UserId", typeof(Int32));

            foreach (int userId in excludeUserIds)
            {
                excludeUserIdTable.Rows.Add(userId);
            }

            return excludeUserIdTable.AsTableValuedParameter("dbo.TVP_ExcludeUserId");
        }

        private ICustomQueryParameter ToCreateGroupChatRoomMemberTable(int[] childUserIds)
        {
            List<TVPLettersGroupMemberSqlParam> tvpParams = new List<TVPLettersGroupMemberSqlParam>();

            foreach (int childUserId in childUserIds)
            {
                TVPLettersGroupMemberSqlParam param = new TVPLettersGroupMemberSqlParam()
                {
                    OperatorStatus = GroupChatRoomChildUpdateTypes.Add.Value,
                    UserIdInGroup = childUserId
                };

                tvpParams.Add(param);
            }

            return CreateTVPLettersGroupMember(tvpParams);
        }

        private ICustomQueryParameter ToAddGroupChatRoomChildMemberTable(int chatRoomId, int[] childUserIds)
        {
            List<TVPLettersGroupMemberSqlParam> tvpParams = new List<TVPLettersGroupMemberSqlParam>();

            foreach (int childUserId in childUserIds)
            {
                TVPLettersGroupMemberSqlParam param = new TVPLettersGroupMemberSqlParam()
                {
                    OperatorStatus = GroupChatRoomChildUpdateTypes.Add.Value,
                    UserIdInGroup = childUserId,
                    GroupId = chatRoomId
                };

                tvpParams.Add(param);
            }

            return CreateTVPLettersGroupMember(tvpParams);
        }

        private ICustomQueryParameter ToUpdateGroupChatRoomChildEnableSendMessageMemberTable(int chatRoomId, int childUserId, bool enableSendMessage)
        {
            List<TVPLettersGroupMemberSqlParam> tvpParams = new List<TVPLettersGroupMemberSqlParam>()
            {
                new TVPLettersGroupMemberSqlParam()
                {
                    OperatorStatus = GroupChatRoomChildUpdateTypes.Others.Value,
                    UserIdInGroup = childUserId,
                    GroupId = chatRoomId,
                    EnablePublishAuthority = enableSendMessage
                }
            };

            return CreateTVPLettersGroupMember(tvpParams);
        }

        private ICustomQueryParameter ToDeleteGroupChatRoomChildMemberTable(int chatRoomId, int childUserId)
        {
            List<TVPLettersGroupMemberSqlParam> tvpParams = new List<TVPLettersGroupMemberSqlParam>()
            {
                new TVPLettersGroupMemberSqlParam()
                {
                    OperatorStatus = GroupChatRoomChildUpdateTypes.Delete.Value,
                    UserIdInGroup = childUserId,
                    GroupId = chatRoomId
                }
            };

            return CreateTVPLettersGroupMember(tvpParams);
        }

        private ICustomQueryParameter CreateTVPLettersGroupMember(List<TVPLettersGroupMemberSqlParam> param = null)
        {
            var tvpLettersGroupMemberTable = new DataTable();
            tvpLettersGroupMemberTable.Columns.Add("SerialId", typeof(Int32));
            tvpLettersGroupMemberTable.Columns.Add("GroupId", typeof(Int32));
            tvpLettersGroupMemberTable.Columns.Add("UserIdInGroup", typeof(Int32));
            tvpLettersGroupMemberTable.Columns.Add("OperatorStatus", typeof(Int32));
            tvpLettersGroupMemberTable.Columns.Add("LastReadingDateTime");
            tvpLettersGroupMemberTable.Columns.Add("EnablePublishAuthority", typeof(Boolean));

            //這兩個欄位目前所有用到的地方都不會傳值，但Sql中TVP_LettersGroupMember已開這個欄位，先不開到Model裡面避免誤傳
            int serialIdDefaultValue = 0;
            string lastReadingDateTimeDefaultValue = SqlDateTime.MinValue.Value.ToString("yyyy-MM-dd HH:mm:ss");

            if (param != null && param.Any())
            {
                foreach (var item in param)
                {
                    tvpLettersGroupMemberTable.Rows.Add(serialIdDefaultValue,
                                                    item.GroupId,
                                                    item.UserIdInGroup,
                                                    item.OperatorStatus,
                                                    lastReadingDateTimeDefaultValue,
                                                    item.EnablePublishAuthority);
                }
            }
            //沒有需要更改Child的資料也需要給一筆預設值的Row，不然SP會執行失敗
            else
            {
                //預設值
                TVPLettersGroupMemberSqlParam defaultValue = new TVPLettersGroupMemberSqlParam();

                tvpLettersGroupMemberTable.Rows.Add(serialIdDefaultValue,
                                                    defaultValue.GroupId,
                                                    defaultValue.UserIdInGroup,
                                                    defaultValue.OperatorStatus,
                                                    lastReadingDateTimeDefaultValue,
                                                    defaultValue.EnablePublishAuthority);
            }

            return tvpLettersGroupMemberTable.AsTableValuedParameter("dbo.TVP_LettersGroupMember");
        }

        protected class TVPLettersGroupMemberSqlParam
        {
            public int GroupId { get; set; } = 0;

            public int UserIdInGroup { get; set; } = 0;

            public bool EnablePublishAuthority { get; set; } = false;

            public int OperatorStatus { get; set; } = GroupChatRoomChildUpdateTypes.Others.Value;
        }

        #endregion
    }
}