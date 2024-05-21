namespace MS.Core.MMModel.Models.Post.Enums
{
    /// <summary>
    /// 選項類型
    /// </summary>
    public enum OptionType
    {
        /// <summary>
        /// 信息类型
        /// </summary>
        Message = 1,

        /// <summary>
        /// 申请调价
        /// </summary>
        ApplyAdjustPrice = 2,

        /// <summary>
        /// 标签 (已經不使用了，但由於是枚舉，故定義不動)
        /// </summary>
        Label = 3,

        /// <summary>
        /// 服务项目
        /// </summary>
        Service = 4
    }
}