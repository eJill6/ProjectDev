namespace FakeMSSeal.Models.Enums
{
    /// <summary>
    /// 推送类型:
    /// 1: 开奖消息
    /// 2: 投注消息
    /// 3: 中奖消息(不同类型content字段类型会有所不同)
    /// </summary>
    public enum MessageType
    {
        /// <summary>
        /// 開獎消息
        /// </summary>
        DrawOpen = 1,

        /// <summary>
        /// 投注消息
        /// </summary>
        Beting = 2,

        /// <summary>
        /// 中獎消息
        /// </summary>
        Prize = 3
    }
}
