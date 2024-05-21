using JxBackendService.Model.Attributes;
using JxBackendService.Resource.Element;
using Microsoft.AspNetCore.Mvc.Rendering;
using MS.Core.MMModel.Models;
using MS.Core.MMModel.Models.AdminPost;
using MS.Core.MMModel.Models.Post.Enums;
using System.ComponentModel.DataAnnotations;

namespace BackSideWeb.Models.ViewModel.PublishRecord
{
    public class AdminPostDetailInputModel : AdminPostInsertData
    {
        /// <summary>
        /// 帖子分区
        /// </summary>
        public List<SelectListItem>? PostTypeListItem { get; set; }

        /// <summary>
        /// 信息类型
        /// </summary>
        public List<SelectListItem>? MessageTypeItem { get; set; }

        /// <summary>
        /// 会员申请调价
        /// </summary>
        public List<SelectListItem>? ApplyAmountItem { get; set; }

        ///// <summary>
        ///// 所在城市
        ///// </summary>
        //public List<SelectListItem> AreaCodeItem { get; set; }
        /// <summary>
        /// 省份
        /// </summary>
        public List<SelectListItem>? ProvinceItem { get; set; }

        /// <summary>
        /// 年龄
        /// </summary>
        public List<SelectListItem>? AgeItem { get; set; }

        /// <summary>
        /// 身高
        /// </summary>
        public List<SelectListItem>? HeightItem { get; set; }

        /// <summary>
        /// 罩杯
        /// </summary>
        public List<SelectListItem>? CupItem { get; set; }

        /// <summary>
        /// 服务项目
        /// </summary>
        public List<OptionItem>? ServiceItem { get; set; }

        public string? ServiceIdStr { get; set; }

        public string? WeChat { get; set; }
        public string? QQ { get; set; }
        public string? Phone { get; set; }

        /// <summary>
        /// 是否会员申请调价
        /// </summary>
        public bool? IsApply { get; set; }

        public string? PhotoIdsStr { get; set; }
        public string? VideoIdsStr { get; set; }

        #region Combo数据

        /// <summary>
        /// 套餐名稱
        /// </summary>
        public string? ComboName1 { get; set; }

        /// <summary>
        /// 套餐價格
        /// </summary>
        public decimal? ComboPrice1 { get; set; }

        /// <summary>
        /// 服務時間，次數或包含項目
        /// </summary>
        public string? ComboService1 { get; set; }

        /// <summary>
        /// 套餐名稱
        /// </summary>
        public string? ComboName2 { get; set; }

        /// <summary>
        /// 套餐價格
        /// </summary>
        public decimal? ComboPrice2 { get; set; }

        /// <summary>
        /// 服務時間，次數或包含項目
        /// </summary>
        public string? ComboService2 { get; set; }

        /// <summary>
        /// 套餐名稱
        /// </summary>
        public string? ComboName3 { get; set; }

        /// <summary>
        /// 套餐價格
        /// </summary>
        public decimal? ComboPrice3 { get; set; }

        /// <summary>
        /// 服務時間，次數或包含項目
        /// </summary>
        public string? ComboService3 { get; set; }

        #endregion Combo数据

        /// <summary>
        /// 总块数
        /// </summary>
        public int? TotalChunks { get; set; }

        /// <summary>
        /// 当前块
        /// </summary>
        public int? CurrentChunk { get; set; }

        /// <summary>
        /// 视频资源
        /// </summary>
        public byte[]? VideoFile { get; set; }

        /// <summary>
        /// 帖子ID
        /// </summary>
        public string? PostId { get; set; }

        /// <summary>
        /// 贴子狀態
        /// </summary>
        public ReviewStatus Status { get; set; }

        /// <summary>
        /// 精選
        /// </summary>
        public bool IsFeatured { get; set; }

        /// <summary>
        /// 权重
        /// </summary>
        public int? Weight { get; set; }

        /// <summary>
        /// 首页帖
        /// </summary>
        public bool IsHomePost { get; set; }

        /// <summary>
        /// 已认证
        /// </summary>
        public bool IsCertified { get; set; }
    }
}