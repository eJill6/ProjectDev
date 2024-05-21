using MMService.DBTools;
using MS.Core.Attributes;
using MS.Core.MMModel.Models.Post.Enums;
using MS.Core.Models;
using System.Data;

namespace MS.Core.MM.Models.Entities.Post
{
    public class MMOfficialShopList
    {
        public string BossId { get; set; } = string.Empty;
        public string ApplyId { get; set; } = string.Empty;
        public int UserId { get; set; }
        public string ShopName { get; set; } = string.Empty;
        public string Girls { get; set; }
        public int ViewBaseCount { get; set; }
        public int Views { get; set; }
        public int ShopYears { get; set; }
        public int DealOrder { get; set; }
        public int SelfPopularity { get; set; }
        public string PostId { get; set; }
        public string Title { get; set; } = string.Empty;
        public decimal LowPrice { get; set; }
        public string CoverUrl { get; set; }
    }
}