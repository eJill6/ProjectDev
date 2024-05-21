namespace JxMsgEntities
{
    public enum RabbitMessageType
    {
        /// <summary>
        /// 提現
        /// </summary>
        TX = 0,

        CZ = 1,

        /// <summary>
        /// 開獎
        /// </summary>
        KJ = 2,

        LeaveWhisper = 3,

        NEWS = 4,

        UserEnter = 5,

        UserLeave = 6,

        Receive = 7,

        ReceiveWhisper = 8,

        UpdateOnlineUserList = 9,

        Heartbeat = 10,

        TransferNotice = 11,

        Lottery = 12,

        VIPMessage = 13,

        UpdateLettersGroup = 14,

        /// <summary>後台</summary>
        JXManagement = 15,

        Chat = 16,

        Bet = 30
    }
}