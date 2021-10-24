using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.Enums.ChatRoom
{
    /// <summary>
    /// 把群聊SP的舊有ReturnCode轉換成共用層統一的ReturnCode
    /// </summary>
    public class ChatRoomSpReturnCode : BaseStringValueModel<ChatRoomSpReturnCode>
    {
        public ReturnCode ReturnCode { get; private set; } = ReturnCode.SystemError;

        private ChatRoomSpReturnCode(string value , ReturnCode returnCode)
        {
            Value = value;
            ReturnCode = returnCode;
        }

        /// <summary>成功</summary>
        public static readonly ChatRoomSpReturnCode Success = new ChatRoomSpReturnCode("0", ReturnCode.Success);

        public static readonly ChatRoomSpReturnCode E51001 = new ChatRoomSpReturnCode("51001", ReturnCode.BeRemovedFromGroupChatRoom);

        public static readonly ChatRoomSpReturnCode E51002 = new ChatRoomSpReturnCode("51002", ReturnCode.NotEnabledToSendGroupChatRoomMessage);

        public static readonly ChatRoomSpReturnCode E51011 = new ChatRoomSpReturnCode("51011", ReturnCode.CroupChatRoomBeDeleted);

        public static readonly ChatRoomSpReturnCode E51012 = new ChatRoomSpReturnCode("51012", ReturnCode.CroupChatRoomBeDeleted);

        public static readonly ChatRoomSpReturnCode E51013 = new ChatRoomSpReturnCode("51013", ReturnCode.NotCreaterForCroupChatRoom);

        public static readonly ChatRoomSpReturnCode E51014 = new ChatRoomSpReturnCode("51014", ReturnCode.SystemError);

        public static readonly ChatRoomSpReturnCode E51021 = new ChatRoomSpReturnCode("51021", ReturnCode.CroupChatRoomBeDeleted);

        public static readonly ChatRoomSpReturnCode E51022 = new ChatRoomSpReturnCode("51022", ReturnCode.CroupChatRoomBeDeleted);

        public static readonly ChatRoomSpReturnCode E51023 = new ChatRoomSpReturnCode("51023", ReturnCode.NotCreaterForCroupChatRoom);

        public static readonly ChatRoomSpReturnCode E51031 = new ChatRoomSpReturnCode("51031", ReturnCode.CroupChatRoomNameDuplicate);

        public static readonly ChatRoomSpReturnCode E51032 = new ChatRoomSpReturnCode("51032", ReturnCode.CroupChatRoomCountReachedTheMaxLimit);

        public static readonly ChatRoomSpReturnCode E51033 = new ChatRoomSpReturnCode("51033", ReturnCode.CroupChatRoomMemberCountReachedTheMaxLimit);

        public static readonly ChatRoomSpReturnCode E51034 = new ChatRoomSpReturnCode("51034", ReturnCode.SystemError);

        public static readonly ChatRoomSpReturnCode E51051 = new ChatRoomSpReturnCode("51051", ReturnCode.NotCreaterForCroupChatRoom);

        public static readonly ChatRoomSpReturnCode E51052 = new ChatRoomSpReturnCode("51052", ReturnCode.CroupChatRoomNameDuplicate);

        public static readonly ChatRoomSpReturnCode E51054 = new ChatRoomSpReturnCode("51054", ReturnCode.CroupChatRoomMemberCountReachedTheMaxLimit);

        public static readonly ChatRoomSpReturnCode E51055 = new ChatRoomSpReturnCode("51055", ReturnCode.SystemError);

        public static readonly ChatRoomSpReturnCode E51071 = new ChatRoomSpReturnCode("51071", ReturnCode.MemberNotInThisCroupChatRoom);

        public static readonly ChatRoomSpReturnCode E51081 = new ChatRoomSpReturnCode("51081", ReturnCode.YourAccountIsDisabled);

        public static readonly ChatRoomSpReturnCode E51091 = new ChatRoomSpReturnCode("51091", ReturnCode.YourAccountIsDisabled);

        public static readonly ChatRoomSpReturnCode E51101 = new ChatRoomSpReturnCode("51101", ReturnCode.SystemError);

        public static readonly ChatRoomSpReturnCode E51102 = new ChatRoomSpReturnCode("51102", ReturnCode.SystemError);

        public static readonly ChatRoomSpReturnCode E51103 = new ChatRoomSpReturnCode("51103", ReturnCode.SystemError);

        public static readonly ChatRoomSpReturnCode E51111 = new ChatRoomSpReturnCode("51111", ReturnCode.YourAccountIsDisabled);
    }
}