using MMService.DBTools;
using MS.Core.Attributes;
using MS.Core.Models;
using System.Data;

namespace MS.Core.MM.Models.Entities.User
{
    /// <summary>
    /// 身份申請表
    /// </summary>
    public class MMIdentityApply : BaseDBModel
    {
        /// <summary>
        /// 申請 Id
        /// </summary>
        [PrimaryKey]
        [EntityType(DbType.String)]
        public string ApplyId { get; set; }

        /// <summary>
        /// 用戶 Id
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 原始身份。0：一般、1：觅经纪、2：觅女郎、3：觅老板、4：星觅官
        /// </summary>
        public byte OriginalIdentity { get; set; }

        /// <summary>
        /// 申請身份類型。0：一般、1：觅经纪、2：觅女郎、3：觅老板、4：星觅官
        /// </summary>
        public byte ApplyIdentity { get; set; }

        /// <summary>
        /// 保證金
        /// </summary>
        public decimal EarnestMoney { get; set; }

        /// <summary>
        /// 額外發贴次數
        /// </summary>
        public int ExtraPostCount { get; set; }

        /// <summary>
        /// 狀態。0：審核中、1：審核通過、2：未通過
        /// </summary>
        public byte Status { get; set; }

        /// <summary>
        /// 编辑身份备注
        /// </summary>
        public string Memo { get; set; } = string.Empty;

        /// <summary>
        /// 申請時間
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 審核人
        /// </summary>
        public string? ExamineMan { get; set; }

        /// <summary>
        /// 審核時間
        /// </summary>
        public DateTime? ExamineTime { get; set; }

        /// <summary>
        /// 联系软件
        /// </summary>
        public string? ContactApp { get; set; }

        /// <summary>
        /// 联系号码
        /// </summary>
        public string? ContactInfo { get; set; }
    }
}