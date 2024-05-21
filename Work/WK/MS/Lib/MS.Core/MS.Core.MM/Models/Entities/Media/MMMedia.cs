using MMService.DBTools;
using MS.Core.Attributes;
using MS.Core.Models;
using System.Data;

namespace MS.Core.MM.Model.Entities.Media
{
    /// <summary>
    /// 媒體檔案
    /// </summary>
    public class MMMedia : BaseDBModel
    {
        /// <summary>
        /// 流水編號
        /// </summary>
        [PrimaryKey]
        [EntityType(DbType.String)]
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// 雲倉儲檔案路徑
        /// </summary>
        [EntityType(DbType.String, StringType.Nvarchar, 250)]
        public string FileUrl { get; set; } = string.Empty;

        /// <summary>
        /// 媒體類型 0:圖片, 1:影片
        /// </summary>
        public int MediaType { get; set; }

        /// <summary>
        /// 媒體的來源 0:Banner, 1:贴子, 2:舉報, 3: 評論
        /// </summary>
        public int SourceType { get; set; }

        /// <summary>
        /// 對應的編號
        /// </summary>
        public string RefId { get; set; } = string.Empty;

        /// <summary>
        /// 新增時間
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 修改時間
        /// </summary>
        public DateTime? ModifyDate { get; set; }
    }
}