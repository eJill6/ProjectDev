using MS.Core.MM.Models.Entities.Post;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MS.Core.MM.Models.Post
{
    public class UpdateAdvertisingContentParam : MMAdvertisingContent
    {
        /// <summary>
        /// 廣告類型。1：什么是XX、2 : 如何发XX贴。
        /// </summary>
        [JsonIgnore]
        public int AdvertisingType { get; set; }

        /// <summary>
        /// 贴子類型。1：廣場、2：担保(原為中介)、3：官方、4：體驗
        /// </summary>
        [JsonIgnore]
        public int PostType { get; set; }

        /// <summary>
        /// 創建時間
        /// </summary>
        [JsonIgnore]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 建立者
        /// </summary>
        [JsonIgnore]
        public string CreateUser { get; set; } = string.Empty;

        /// <summary>
        /// 更新時間
        /// </summary>
        [JsonIgnore]
        public DateTime? ModifyDate { get; set; }
    }
}