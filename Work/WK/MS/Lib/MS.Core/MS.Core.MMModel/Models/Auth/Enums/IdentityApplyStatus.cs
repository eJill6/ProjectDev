namespace MS.Core.MMModel.Models.Auth.Enums
{
    /// <summary>
    /// 身份申請狀態
    /// </summary>
    public enum IdentityApplyStatus
    {
        /// <summary>
        /// 尚未申請
        /// </summary>
        NotYet = 0,

        /// <summary>
        /// 申請中
        /// </summary>
        Applying = 1,

        /// <summary>
        /// 通過
        /// </summary>
        Pass = 2
    }
}