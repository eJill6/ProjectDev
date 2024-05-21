using MS.Core.MMModel.Models.Auth.Enums;
using MS.Core.MMModel.Models.User.Enums;

namespace MS.Core.MMModel.Models.Auth
{
    /// <summary>
    /// 發佈認證返回資訊
    /// </summary>
    public class CertificationResponse
    {
        /// <summary>
        /// 剩餘發佈次數
        /// </summary>
        public int RemainPublish { get; set; }

        /// <summary>
        /// 申請身份
        /// </summary>
        public IdentityType ApplyIdentity { get; set; }

        /// <summary>
        /// 申請狀態
        /// </summary>
        public IdentityApplyStatus ApplyStatus { get; set; }
    }
}