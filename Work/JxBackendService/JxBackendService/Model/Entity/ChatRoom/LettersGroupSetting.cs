using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.Entity.ChatRoom
{
    public class LettersGroupSetting
    {
        /// <summary>
        /// 新增時站內信群聊時所產生的序列號的 Id
        /// </summary>
        public int GroupId { get; set; }

        /// <summary>
        /// 站內信群聊的名稱
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// 建立該群聊的 UserId
        /// </summary>
        public int CreatedUserId { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        public DateTime CreatedDateTime { get; set; }

        /// <summary>
        /// 更新該群聊的 UserId
        /// </summary>
        public int UpdatedUserId { get; set; }

        /// <summary>
        /// 修改時間
        /// </summary>
        public DateTime UpdatedDateTime { get; set; }

        /// <summary>
        /// 是否已啟用
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// 參加群聊的總人數
        /// </summary>
        public int AmountOfJoinGroup { get; set; }

        public int LastMsgSeqId { get; set; }
    }
}
