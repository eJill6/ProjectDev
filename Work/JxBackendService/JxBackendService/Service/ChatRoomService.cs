using JxBackendService.Common.Extensions;
using JxBackendService.Common.Util;
using JxBackendService.Common.Util.Cache;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Repository;
using JxBackendService.Interface.Service;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.ChatRoom;
using JxBackendService.Resource.Element;
using JxBackendService.Service.Base;
using JxBackendService.Service.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Service
{
    public class ChatRoomService : BaseService, IChatRoomService
    {
        private readonly IChatRoomRep _chatRoomRep;
        private readonly IUserInfoRelatedService _userInfoRelatedService;
        private readonly IMessageQueueService _chatRoomMqService;
        private readonly IJxCacheService _jxCacheService;

        /// <summary>
        /// [系統訊息]的Admin帳號固定UserId
        /// </summary>
        private static readonly int adminUserId = 1;

        public ChatRoomService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _chatRoomRep = ResolveJxBackendService<IChatRoomRep>();
            _userInfoRelatedService = ResolveJxBackendService<IUserInfoRelatedService>();
            _chatRoomMqService = ResolveServiceForModel<IMessageQueueService>(envLoginUser.Application);
            _jxCacheService = ResolveServiceForModel<IJxCacheService>(envLoginUser.Application);
        }

        public BaseReturnDataModel<MemberChatRoomListViewModel> GetMemberChatRoomListForApi(int userId, List<int> existedUsers)
        {
            //SP中控制是否需要顯示上級、系統，目前Api需要拿下級跟上級，呈現在畫面上所以給 True
            bool isNeedSpecialMembers = true;

            //這裡 App 會傳入目前畫面上存在的 User (existedUsers)，Sp會排除這些User進行排序然後給結果，避免App畫面上出現同樣用戶
            int[] excludeUserIds = new int[] { userId };

            if (existedUsers.AnyAndNotNull())
            {
                excludeUserIds = existedUsers.ToArray();
            }

            var repReturnModel = _chatRoomRep.GetMemberChatRoomList(userId, isNeedSpecialMembers, excludeUserIds);

            MemberChatRoomListViewModel resultModel = new MemberChatRoomListViewModel();

            //Rep執行失敗，直接回傳Message出去
            if (!repReturnModel.IsSuccess)
            {
                return new BaseReturnDataModel<MemberChatRoomListViewModel>(ReturnCode.GetSingle(repReturnModel.Code));
            }

            //轉型態，特別處理 AvatarId(頭像Id)以及上級、系統的名稱
            //[上級列表] AvatarId是之前站內信API保留的邏輯，App那裡存放45張的頭象圖，這裡沿用當時的邏輯，取45的餘數
            //[上級列表] 特別處理上級、系統的名稱
            List<MemberChatRoomViewModel> parentChatRoomList = repReturnModel.DataModel.ParentChatRoomList.Select(pcrl => new MemberChatRoomViewModel()
            {
                UserId = pcrl.PubUserId,
                UserName = (pcrl.PubUserId == adminUserId) ? ChatRoomElement.AdminUserName : ChatRoomElement.ParentUserName,
                UnReadMessageTotalCount = pcrl.UnReadTotalCount,
                AvatarId = GetAvatarId(pcrl.PubUserId ,(pcrl.PubUserId == adminUserId) )
            }).ToList();

            //[下級列表] AvatarId是之前站內信API保留的邏輯，App那裡存放45張的頭象圖，這裡沿用當時的邏輯，取45的餘數
            List<MemberChatRoomViewModel> childChatRoomList = repReturnModel.DataModel.ChildChatRoomList.Select(ccrl => new MemberChatRoomViewModel()
            {
                UserId = ccrl.PubUserId,
                UserName = ccrl.PubUserName,
                UnReadMessageTotalCount = ccrl.UnReadTotalCount,
                AvatarId = GetAvatarId(ccrl.PubUserId)
            }).ToList();

            resultModel.ParentChatRoomList = parentChatRoomList;
            resultModel.ChildChatRoomList = childChatRoomList;

            return new BaseReturnDataModel<MemberChatRoomListViewModel>(ReturnCode.Success, resultModel);
        }

        public BaseReturnDataModel<GroupChatRoomListViewModel> GetGroupChatRoomListForApi(int userId)
        {
            var repReturnModel = _chatRoomRep.GetGroupChatRoomList(userId);

            GroupChatRoomListViewModel resultModel = new GroupChatRoomListViewModel();

            if (!repReturnModel.IsSuccess)
            {
                return new BaseReturnDataModel<GroupChatRoomListViewModel>(ReturnCode.GetSingle(repReturnModel.Code));
            }

            //被加入的
            List<GroupChatRoomViewModel> joinedChatRoomList = 
            repReturnModel.DataModel
                .Where(gcrl => gcrl.CreatedUserId != userId)
                .Select(mcrl => new GroupChatRoomViewModel()
                {
                    ChatRoomId = mcrl.GroupId,
                    ChatRoomName = mcrl.GroupName,
                    ChatRoomMemberCount = mcrl.AmountOfJoinGroup,
                    EnableSendMessage = mcrl.EnablePublishAuthority,
                    UnReadMessageTotalCount = mcrl.UnReadTotalCount
                }).ToList();

            //管理的
            List<GroupChatRoomViewModel> manageChatRoomList = 
            repReturnModel.DataModel
                .Where(gcrl => gcrl.CreatedUserId == userId)
                .Select(mcrl => new GroupChatRoomViewModel()
                {
                    ChatRoomId = mcrl.GroupId,
                    ChatRoomName = mcrl.GroupName,
                    ChatRoomMemberCount = mcrl.AmountOfJoinGroup,
                    EnableSendMessage = mcrl.EnablePublishAuthority,
                    UnReadMessageTotalCount = mcrl.UnReadTotalCount
                }).ToList();

            resultModel.JoinedChatRoomList = joinedChatRoomList;
            resultModel.ManageChatRoomList = manageChatRoomList;
            resultModel.ManageChatRoomMaxCount = GlobalVariables.ManageChatRoomMaxCount;

            return new BaseReturnDataModel<GroupChatRoomListViewModel>(ReturnCode.Success, resultModel);
        }

        public BaseReturnDataModel<List<GroupChatRoomChildCreateModeViewModel>> GetCreateModeGroupChatRoomChildListForApi(int userId ,
                                                                                                                          int queryType, 
                                                                                                                          int? chatRoomId)
        {
            //檢查傳入的 queryType 是否正確
            GroupChatRoomChildListQueryTypes childListQueryTypes = GroupChatRoomChildListQueryTypes.GetSingle(queryType);

            List<GroupChatRoomChildCreateModeViewModel> resultModel = new List<GroupChatRoomChildCreateModeViewModel>();

            //取得所有下級
            var allChildUserInfos = _userInfoRelatedService.GetAllFirstChildUserInfo(userId);

            if (childListQueryTypes == GroupChatRoomChildListQueryTypes.AllChildList)
            {
                resultModel = allChildUserInfos.Select(cuis => new GroupChatRoomChildCreateModeViewModel()
                {
                    UserId = cuis.UserID,
                    UserName = cuis.UserName,
                    LastLoginTime = (cuis.LastLoginTime.HasValue) ? cuis.LastLoginTime.Value.ToUnixOfTime().ToString() : DateTime.MinValue.ToUnixOfTime().ToString(),
                    AvatarId = GetAvatarId(cuis.UserID)
                }).ToList();
            }
            else if (childListQueryTypes == GroupChatRoomChildListQueryTypes.NotInGroupChatRoomAnotherChild)
            {
                if (!chatRoomId.HasValue)
                {
                    throw new Exception($"{nameof(GetCreateModeGroupChatRoomChildListForApi)}," +
                                        $"{nameof(queryType)},{queryType},{nameof(chatRoomId)}為空");
                }

                bool isExistChatRoom = _chatRoomRep.CheckChatRoomIsExist(chatRoomId.Value);

                if (!isExistChatRoom)
                {
                    return new BaseReturnDataModel<List<GroupChatRoomChildCreateModeViewModel>>(ReturnCode.CroupChatRoomBeDeleted);
                }

                //取得當前群組的下級名單
                var repReturnModel = _chatRoomRep.GetGroupChatRoomChildList(userId, chatRoomId.Value);

                if (!repReturnModel.IsSuccess)
                {
                    return new BaseReturnDataModel<List<GroupChatRoomChildCreateModeViewModel>>(ReturnCode.GetSingle(repReturnModel.Code));
                }

                var childListId = repReturnModel.DataModel.GroupChatRoomChildList.Select(gcrcl => gcrcl.UserIdInGroup).ToList();

                //排除
                resultModel = allChildUserInfos
                    .Where(cuis => !childListId.Contains(cuis.UserID))
                    .Select(cuis => new GroupChatRoomChildCreateModeViewModel()
                    {
                        UserId = cuis.UserID,
                        UserName = cuis.UserName,
                        LastLoginTime = (cuis.LastLoginTime.HasValue) ? cuis.LastLoginTime.Value.ToUnixOfTime().ToString() : DateTime.MinValue.ToUnixOfTime().ToString(),
                        AvatarId = GetAvatarId(cuis.UserID)
                    }).ToList();
            }
            else
            {
                throw new NotSupportedException();
            }

            return new BaseReturnDataModel<List<GroupChatRoomChildCreateModeViewModel>>(ReturnCode.Success, resultModel);
        }

        public BaseReturnDataModel<List<GroupChatRoomChildEditModeViewModel>> GetEditModeGroupChatRoomChildListForApi(int userId ,int chatRoomId)
        {
            bool isExistChatRoom = _chatRoomRep.CheckChatRoomIsExist(chatRoomId);

            if (!isExistChatRoom)
            {
                return new BaseReturnDataModel<List<GroupChatRoomChildEditModeViewModel>>(ReturnCode.CroupChatRoomBeDeleted);
            }

            //取得所有下級
            var allChildUserInfos = _userInfoRelatedService.GetAllFirstChildUserInfo(userId);

            //取得當前群組的下級名單
            var repReturnModel = _chatRoomRep.GetGroupChatRoomChildList(userId, chatRoomId);

            List<GroupChatRoomChildEditModeViewModel> resultModel = new List<GroupChatRoomChildEditModeViewModel>();

            if (!repReturnModel.IsSuccess)
            {
                return new BaseReturnDataModel<List<GroupChatRoomChildEditModeViewModel>>(ReturnCode.GetSingle(repReturnModel.Code));
            }

            //清單必須排除自己
            resultModel = repReturnModel.DataModel.GroupChatRoomChildList.Where(cuis => cuis.UserIdInGroup != userId)
                                                                         .Select(cuis => new GroupChatRoomChildEditModeViewModel()
                                                                         {
                                                                             UserId = cuis.UserIdInGroup,
                                                                             UserName = cuis.UserName,
                                                                             AvatarId = GetAvatarId(cuis.UserIdInGroup),
                                                                             EnableSendMessage = cuis.EnablePublishAuthority
                                                                         }).ToList();

            //處理LastLoginTime
            foreach (var item in resultModel)
            {
                var userInfo = allChildUserInfos.FirstOrDefault(cuis => cuis.UserID == item.UserId);
                if (userInfo != null)
                {
                    item.LastLoginTime = (userInfo.LastLoginTime.HasValue) ? userInfo.LastLoginTime.Value.ToUnixOfTime().ToString() : DateTime.MinValue.ToUnixOfTime().ToString();
                }
                else
                {
                    item.LastLoginTime = DateTime.MinValue.ToUnixOfTime().ToString();
                }
            }

            return new BaseReturnDataModel<List<GroupChatRoomChildEditModeViewModel>>(ReturnCode.Success, resultModel);
        }

        public BaseReturnDataModel<List<GroupChatRoomMessageViewModel>> GetGroupChatRoomMessageListForApi(int userId, int chatRoomId, int queryType, long? messageId)
        {
            bool isExistChatRoom = _chatRoomRep.CheckChatRoomIsExist(chatRoomId);

            List<GroupChatRoomMessageViewModel> resultModel = new List<GroupChatRoomMessageViewModel>();

            if (!isExistChatRoom)
            {
                return new BaseReturnDataModel<List<GroupChatRoomMessageViewModel>>(ReturnCode.CroupChatRoomBeDeleted);
            }

            //檢查傳入的 queryType 是否正確
            GroupChatRoomMessageQueryTypes messageListQueryTypes = GroupChatRoomMessageQueryTypes.GetSingle(queryType);

            var repReturnModel = _chatRoomRep.GetGroupChatRoomMessageList(userId, chatRoomId, queryType, messageId);

            if (!repReturnModel.IsSuccess)
            {
                return new BaseReturnDataModel<List<GroupChatRoomMessageViewModel>>(ReturnCode.GetSingle(repReturnModel.Code));
            }

            resultModel = repReturnModel.DataModel.GroupChatRoomMessageList
                .Select(msgs => new GroupChatRoomMessageViewModel()
                {
                    UserId = msgs.PublishUserId,
                    UserName = msgs.PublishUserName,
                    AvatarId = GetAvatarId(msgs.PublishUserId),
                    MessageId = msgs.MessageId,
                    MessageContent = msgs.MessageContent,
                    SendTime = msgs.PublishDateTime.ToUnixOfTime().ToString()
                }).ToList();

            return new BaseReturnDataModel<List<GroupChatRoomMessageViewModel>>(ReturnCode.Success, resultModel);
        }

        public BaseReturnModel SendGroupChatRoomMessageForApi(int userId, string userName, string messageContent, int chatRoomId)
        {
            var isInSendMaxRange = CheckSendMessageCountIsInMaxRange(userId);

            if (!isInSendMaxRange.IsSuccess)
            {
                return isInSendMaxRange;
            }

            bool isExistChatRoom = _chatRoomRep.CheckChatRoomIsExist(chatRoomId);

            if (!isExistChatRoom)
            {
                return new BaseReturnModel(ReturnCode.CroupChatRoomBeDeleted);
            }

            //判斷 messageContent 是否超過500字元
            if (messageContent.Length > GlobalVariables.ChatRoomMessageContentMaxLength)
            {
                return new BaseReturnModel(ReturnCode.ChatRoomMessageOutOfRange);
            }

            //不能直接Return _chatRoomRep.SendGroupChatRoomMessage的結果出去，
            //雖然 BaseReturnModel跟BaseReturnDataModel<GroupChatRoomMessageListResultModel>有繼承關係，編譯上會過
            //但是WCF會認為那是違反合約
            var repReturnModel = _chatRoomRep.SendGroupChatRoomMessage(userId, userName, messageContent, chatRoomId);

            //發送MQ
            if (repReturnModel.IsSuccess)
            {
                //發送聊天室內訊息詳細資訊MQ
                _chatRoomMqService.SendGroupChatRoomDetailMessage(chatRoomId, messageContent, userId, userName, GetAvatarId(userId));

                //用戶個人動作控制MQ，避免收到自己發送的訊息的MQ
                List<int> userIdListForMqSend = GetGroupChatRoomUserIdList(chatRoomId, userId);
                foreach (var userIdForMqSend in userIdListForMqSend)
                {
                    _chatRoomMqService.SendGroupChatRoomActionControlMessage(GroupChatRoomActionControlTypes.UpdateMessage.Value,
                                                                             chatRoomId,
                                                                             string.Empty,
                                                                             userIdForMqSend);             
                }
            }

            return new BaseReturnModel(ReturnCode.GetSingle(repReturnModel.Code));

        }

        public BaseReturnDataModel<GroupChatRoomViewModel> CreateGroupChatRoomForApi(int userId, string chatRoomName, List<int> childUserIdList)
        {
            List<int> userIdsSqlParam = new List<int>();

            //檢查清單是否全部為下級
            if (!CheckChatRoomChildListAndParentRelationship(userId , childUserIdList))
            {
                return new BaseReturnDataModel<GroupChatRoomViewModel>(ReturnCode.ParameterIsInvalid);
            }

            //檢查通過後，呼叫SP要加上自己
            userIdsSqlParam.Add(userId);
            userIdsSqlParam.AddRange(childUserIdList);

            //判斷 chatRoomName 是否超過16字元
            if (!CheckChatRoomNameLength(chatRoomName))
            {
                return new BaseReturnDataModel<GroupChatRoomViewModel>(ReturnCode.ChatRoomNameOutOfRange , 
                                                                       new string[] { GlobalVariables.ChatRoomNameMaxLength.ToString() });
            }

            //不能直接Return _chatRoomRep.CreateGroupChatRoom的結果出去，
            //雖然 BaseReturnModel跟BaseReturnDataModel<int>有繼承關係，編譯上會過
            //但是WCF會認為那是違反合約
            var repReturnModel = _chatRoomRep.CreateGroupChatRoom(userId, chatRoomName, userIdsSqlParam.ToArray());

            int totalMemberCount = userIdsSqlParam.Count();

            //發送MQ
            if (repReturnModel.IsSuccess)
            {
                foreach (int childUserId in childUserIdList)
                {
                    //[容舊搬移]前台站內信在做任何動作的時候都會發這則MQ，先搬到這裡來
                    _chatRoomMqService.SendUpdateLettersGroupMessage(childUserId);

                    //控制App畫面上的更新動作MQ
                    _chatRoomMqService.SendGroupChatRoomBeJoinedMessage(repReturnModel.DataModel, //新的聊天室的Id
                                                                        chatRoomName,             //新的聊天室的名稱
                                                                        childUserId,              //被加入的下級的名單
                                                                        totalMemberCount);        //群組人數
                }
            }

            var resultDataModel = new GroupChatRoomViewModel()
            {
                ChatRoomId = repReturnModel.DataModel,
                ChatRoomName = chatRoomName,
                EnableSendMessage = true,
                ChatRoomMemberCount = totalMemberCount,
                UnReadMessageTotalCount = 0
            };

            return new BaseReturnDataModel<GroupChatRoomViewModel>(ReturnCode.GetSingle(repReturnModel.Code) , resultDataModel);
        }

        public BaseReturnModel AddGroupChatRoomChildForApi(int userId, int chatRoomId, List<int> childUserIdList)
        {
            bool isExistChatRoom = _chatRoomRep.CheckChatRoomIsExist(chatRoomId);

            if (!isExistChatRoom)
            {
                return new BaseReturnModel(ReturnCode.CroupChatRoomBeDeleted);
            }

            //檢查清單是否全部為下級
            if (!CheckChatRoomChildListAndParentRelationship(userId, childUserIdList))
            {
                return new BaseReturnModel(ReturnCode.ParameterIsInvalid);
            }

            //取得當前群組的下級名單
            var repReturnModel = _chatRoomRep.GetGroupChatRoomChildList(userId, chatRoomId);

            if (!repReturnModel.IsSuccess)
            {
                return new BaseReturnModel(ReturnCode.GetSingle(repReturnModel.Code));
            }

            //避免傳入目前已存在群組的下級
            childUserIdList = childUserIdList.Where(
                cui => !repReturnModel.DataModel.GroupChatRoomChildList.Select(gccl => gccl.UserIdInGroup).Contains(cui)).ToList();

            var result = _chatRoomRep.AddGroupChatRoomChild(userId, chatRoomId, repReturnModel.DataModel.GroupChatRoomSetting.GroupName, childUserIdList.ToArray());

            //發送MQ
            if (result.IsSuccess)
            {
                int newTotalMemberCount = repReturnModel.DataModel.GroupChatRoomChildList.Count() + childUserIdList.Count();

                foreach (int childUserId in childUserIdList)
                {
                    //[容舊搬移]前台站內信在做任何動作的時候都會發這則MQ，先搬到這裡來
                    _chatRoomMqService.SendUpdateLettersGroupMessage(childUserId);

                    //控制App畫面上的更新動作MQ
                    _chatRoomMqService.SendGroupChatRoomBeJoinedMessage(repReturnModel.DataModel.GroupChatRoomSetting.GroupId,
                                                                        repReturnModel.DataModel.GroupChatRoomSetting.GroupName,
                                                                        childUserId,
                                                                        newTotalMemberCount); 
                }
            }

            return result;
        }

        public BaseReturnModel UpdateGroupChatRoomChildEnableSendMessageForApi(int userId, int chatRoomId, int childUserId, bool enableSendMessage)
        {
            bool isExistChatRoom = _chatRoomRep.CheckChatRoomIsExist(chatRoomId);

            if (!isExistChatRoom)
            {
                return new BaseReturnModel(ReturnCode.CroupChatRoomBeDeleted);
            }

            //取得當前群組的下級名單
            var repReturnModel = _chatRoomRep.GetGroupChatRoomChildList(userId, chatRoomId);

            if (!repReturnModel.IsSuccess)
            {
                return new BaseReturnModel(ReturnCode.GetSingle(repReturnModel.Code));
            }

            var result = _chatRoomRep.UpdateGroupChatRoomChildEnableSendMessage(userId, chatRoomId, 
                                                                          repReturnModel.DataModel.GroupChatRoomSetting.GroupName, 
                                                                          childUserId, enableSendMessage);

            //發送MQ
            if (result.IsSuccess)
            {
                //[容舊搬移]前台站內信在做任何動作的時候都會發這則MQ，先搬到這裡來
                _chatRoomMqService.SendUpdateLettersGroupMessage(childUserId);

                //控制App畫面上的更新動作MQ
                _chatRoomMqService.SendGroupChatRoomUpdatedSendPermissionMessage(repReturnModel.DataModel.GroupChatRoomSetting.GroupId,
                                                                                 repReturnModel.DataModel.GroupChatRoomSetting.GroupName,
                                                                                 childUserId,
                                                                                 enableSendMessage);
            }

            return result;
        }

        public BaseReturnModel DeleteGroupChatRoomChildForApi(int userId, int chatRoomId, int childUserId)
        {
            bool isExistChatRoom = _chatRoomRep.CheckChatRoomIsExist(chatRoomId);

            if (!isExistChatRoom)
            {
                return new BaseReturnModel(ReturnCode.CroupChatRoomBeDeleted);
            }

            //取得當前群組的下級名單
            var repReturnModel = _chatRoomRep.GetGroupChatRoomChildList(userId, chatRoomId);

            if (!repReturnModel.IsSuccess)
            {
                return new BaseReturnModel(ReturnCode.GetSingle(repReturnModel.Code));
            }

            var result =  _chatRoomRep.DeleteGroupChatRoomChild(userId, chatRoomId, 
                                                         repReturnModel.DataModel.GroupChatRoomSetting.GroupName, childUserId);

            //發送MQ
            if (result.IsSuccess)
            {
                //[容舊搬移]前台站內信在做任何動作的時候都會發這則MQ，先搬到這裡來
                _chatRoomMqService.SendUpdateLettersGroupMessage(childUserId);

                //控制App畫面上的更新動作MQ
                _chatRoomMqService.SendGroupChatRoomActionControlMessage(GroupChatRoomActionControlTypes.BeDeleted.Value,
                                                                         repReturnModel.DataModel.GroupChatRoomSetting.GroupId,
                                                                         repReturnModel.DataModel.GroupChatRoomSetting.GroupName,
                                                                         childUserId);
            }

            return result;
        }

        public BaseReturnModel UpdateGroupChatRoomNameForApi(int userId, int chatRoomId, string newChatRoomName)
        {
            bool isExistChatRoom = _chatRoomRep.CheckChatRoomIsExist(chatRoomId);

            if (!isExistChatRoom)
            {
                return new BaseReturnModel(ReturnCode.CroupChatRoomBeDeleted);
            }

            //判斷 newChatRoomName 是否超過16字元
            if (!CheckChatRoomNameLength(newChatRoomName))
            {
                return new BaseReturnModel(ReturnCode.ChatRoomNameOutOfRange, 
                                           new string[] { GlobalVariables.ChatRoomNameMaxLength.ToString() });
            }

            var result =  _chatRoomRep.UpdateGroupChatRoomName(userId, chatRoomId, newChatRoomName);

            //發送MQ
            if (result.IsSuccess)
            {
                //用戶個人動作控制MQ，避免收到自己發送的訊息的MQ
                List<int> userIdListForMqSend = GetGroupChatRoomUserIdList(chatRoomId, userId);
                foreach (var userIdForMqSend in userIdListForMqSend)
                {
                    //控制App畫面上的更新動作MQ
                    _chatRoomMqService.SendGroupChatRoomActionControlMessage(GroupChatRoomActionControlTypes.ChatRoomNameBeUpdated.Value,
                                                                             chatRoomId,
                                                                             newChatRoomName,
                                                                             userIdForMqSend);
                }
            }

            return result;
        }

        public BaseReturnModel DeleteGroupChatRoomForApi(int userId, int chatRoomId)
        {
            bool isExistChatRoom = _chatRoomRep.CheckChatRoomIsExist(chatRoomId);

            if (!isExistChatRoom)
            {
                return new BaseReturnModel(ReturnCode.CroupChatRoomBeDeleted);
            }

            //拿出要發送MQ的人員清單
            List<int> userIdListForMqSend = GetGroupChatRoomUserIdList(chatRoomId, userId);

            var result = _chatRoomRep.DeleteGroupChatRoom(userId, chatRoomId);

            //發送MQ
            if (result.IsSuccess)
            {
                foreach (var userIdForMqSend in userIdListForMqSend)
                {
                    //控制App畫面上的更新動作MQ
                    _chatRoomMqService.SendGroupChatRoomActionControlMessage(GroupChatRoomActionControlTypes.ChatRoomBeDeleted.Value,
                                                                             chatRoomId,
                                                                             string.Empty,
                                                                             userIdForMqSend);
                }
            }

            return result;
        }

        public BaseReturnDataModel<int> GetAllUnReadMessageCountForApi(int userId)
        {
            return _chatRoomRep.GetAllUnReadMessageCount(userId);
        }

        public BaseReturnModel UpdateAllUnReadMessageToRead(int userId)
        {
            //把Controller的回傳訊息邏輯搬到這裡處理
            //更新筆數 > 0 成功
            //更新筆數 == 0 失敗，回傳 没有记录被更新

            int updateCount = _chatRoomRep.UpdateAllUnReadMessageToRead(userId);

            if (updateCount > 0)
            {
                return new BaseReturnModel(ReturnCode.Success);
            }
            else
            {
                return new BaseReturnModel("没有记录被更新");
            }
        }

        /// <summary>
        /// 檢查聊天室名稱長度
        /// </summary>
        /// <param name="chatRoomName"></param>
        /// <returns></returns>
        private bool CheckChatRoomNameLength(string chatRoomName)
        {
            return (chatRoomName.Length <= GlobalVariables.ChatRoomNameMaxLength);
        }

        //轉換App會用到的頭像Id
        private static int GetAvatarId(int userId, bool isAdmin = false)
        {
            //這兩個參數有其他地方需要用到再搬出去

            // 存在App內的隨機頭像圖片數量
            int avatarPhotoCount = 45;

            // [系統訊息]的Admin帳號固定頭像Id
            int adminAvatarId = -1;

            return (isAdmin) ? adminAvatarId : userId % avatarPhotoCount;
        }

        /// <summary>
        /// 取得聊天室下級完整人員Id清單，排除自己(使用Cache)
        /// </summary>
        /// <param name="chatRoomId"></param>
        /// <param name="currentUserId"></param>
        /// <returns></returns>
        private List<int> GetGroupChatRoomUserIdList(int chatRoomId , int currentUserId)
        {
            var searchCacheParam = new SearchCacheParam()
            {
                Key = CacheKey.GroupChatRoomUserIdList(chatRoomId),
                CacheSeconds = 60,
                IsForceRefresh = false
            };

            return _jxCacheService.GetCache(searchCacheParam, () =>
            {
                List<int> userIdList = new List<int>();

                //聊天室設定
                var groupChatRoomSetting = _chatRoomRep.GetGroupChatRoomSetting(chatRoomId);

                var repReturnModel = _chatRoomRep.GetGroupChatRoomChildList(groupChatRoomSetting.CreatedUserId, chatRoomId);

                if (!repReturnModel.IsSuccess)
                {
                    return userIdList;
                }

                userIdList.AddRange(repReturnModel.DataModel.GroupChatRoomChildList.Where(gcrc => gcrc.UserIdInGroup != currentUserId)
                                                                                   .Select(gcrc => gcrc.UserIdInGroup)
                                                                                   .ToList());

                return userIdList;
            });
        }

        /// <summary>
        /// 取得站內信全域設定
        /// </summary>
        /// <returns></returns>
        private GroupChatRoomConfigSettingSqlRawModel GetChatRoomConfigSetting()
        {
            var searchCacheParam = new SearchCacheParam()
            {
                Key = CacheKey.ChatRoomConfigSetting,
                CacheSeconds = 24 * 60 * 60, //1天
                IsForceRefresh = false
            };

            return _jxCacheService.GetCache(searchCacheParam, () =>
            {
                var repReturnModel = _chatRoomRep.GetChatRoomConfigSetting();

                if (!repReturnModel.IsSuccess)
                {
                    return new GroupChatRoomConfigSettingSqlRawModel();
                }

                return repReturnModel.DataModel;
            });
        }

        /// <summary>
        /// 取得用戶1分鐘內發送訊息數量
        /// </summary>
        /// <returns></returns>
        private ChatRoomUserSendMessageCountViewModel GetUserSendMessageCountInOneMinute(int userId)
        {
            int cacheSeconds = 60; //1分鐘

            var searchCacheParam = new SearchCacheParam()
            {
                Key = CacheKey.ChatRoomUserSendMessageCountInOneMinute(userId),
                CacheSeconds = cacheSeconds,
                IsForceRefresh = false
            };

            return _jxCacheService.GetCache(searchCacheParam, () =>
            {
                int firstSendCount = 1;

                return new ChatRoomUserSendMessageCountViewModel() {
                    SendTotalCount = firstSendCount,
                    StartSendTime = DateTime.Now,
                    IsFirstSend = true
                };
            });
        }

        /// <summary>
        /// 更新用戶1分鐘內發送訊息數量
        /// </summary>
        /// <returns></returns>
        private void SetUserSendMessageCountInOneMinute(int userId, ChatRoomUserSendMessageCountViewModel cacheData)
        {
            int cacheSeconds = 60; //1分鐘

            var searchCacheParam = new SearchCacheParam()
            {
                Key = CacheKey.ChatRoomUserSendMessageCountInOneMinute(userId),
                CacheSeconds = cacheSeconds
            };

            _jxCacheService.SetCache(searchCacheParam, cacheData);
        }

        /// <summary>
        /// 檢查ChildList是否全部為Parent的下級
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="childUserIdList"></param>
        /// <returns></returns>
        private bool CheckChatRoomChildListAndParentRelationship(int parentId , List<int> childUserIdList)
        {
            if (!childUserIdList.AnyAndNotNull())
            {
                return false;
            }

            //取得所有下級
            var allChildUserInfos = _userInfoRelatedService.GetAllFirstChildUserInfo(parentId);

            foreach (int childUserId in childUserIdList)
            {
                //只要有一個childId不屬於這個上級，回傳false
                var userInfo = allChildUserInfos.Where(cui => cui.UserID == childUserId).SingleOrDefault();

                if (userInfo == null)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 檢查站內信每分鐘發信次數是否有超過系統設定次數 (一對一人員及群聊都算在內)
        /// 於前台 HomeController 搬過來
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public BaseReturnModel CheckSendMessageCountIsInMaxRange(int userId)
        {
            //取得當前User的1分鐘內發送數量
            var userSendMessageCountInfo = GetUserSendMessageCountInOneMinute(userId);

            //如果之前的紀錄超過 1分鐘，重新設置起始計算時間，回傳檢查成功
            if (userSendMessageCountInfo.StartSendTime.AddSeconds(60) < DateTime.Now)
            {
                return new BaseReturnModel(ReturnCode.Success);
            }

            //繼續計算、判斷

            //取得站內信全域設定
            GroupChatRoomConfigSettingSqlRawModel chatRoomConfigSetting = GetChatRoomConfigSetting();

            if (!userSendMessageCountInfo.IsFirstSend)
            {
                userSendMessageCountInfo.SendTotalCount += 1;
            }
            
            //超過，回傳失敗
            if (userSendMessageCountInfo.SendTotalCount > chatRoomConfigSetting.MaxPublishLettersPerMinute)
            {
                return new BaseReturnModel(ReturnCode.ChatRoomPerMinuteMessageCountOutOfRange);
            }

            //回寫之前指定IsFirstSend為false
            userSendMessageCountInfo.IsFirstSend = false;

            //回寫Cache資料
            SetUserSendMessageCountInOneMinute(userId, userSendMessageCountInfo);

            return new BaseReturnModel(ReturnCode.Success);
        }
    }
}
