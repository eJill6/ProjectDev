using Dapper;
using MMService.DBTools;
using MS.Core.Attributes;
using MS.Core.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace MS.Core.MM.Model.Entities.Banner
{
    /// <summary>
    /// Banner資料庫資料
    /// </summary>
    public class MMBanner : BaseDBModel
    {
        /// <summary>
        /// 流水號
        /// </summary>
        [PrimaryKey]
        [EntityType(DbType.String)]
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// 標題
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 創建時間
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 開始時間
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// 結束時間
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// 修改時間
        /// </summary>
        public DateTime? ModifyDate { get; set; }

        /// <summary>
        /// 創建使用者
        /// </summary>
        public string CreateUser { get; set; } = string.Empty;

        /// <summary>
        /// 修改使用者
        /// </summary>
        public string ModifyUser { get; set; } = string.Empty;

        /// <summary>
        /// 是否啟用
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// 廣告類型
        /// </summary>
        public byte Type { get; set; }

        /// <summary>
        /// 連結方式
        /// </summary>
        public byte LinkType { get; set; }

        /// <summary>
        /// 轉導網址
        /// </summary>
        public string RedirectUrl { get; set; } = string.Empty;

        /// <summary>
        /// 前台位置 1：首页主选单 2：首页轮播 3：店铺轮播
        /// </summary>

        public int LocationType { get; set; }
    }
}