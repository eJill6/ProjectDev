using System;
using System.Collections.Generic;
using System.Text;

namespace MS.Core.MMModel.Models.Post
{
    public class NextPagePostCoverViewForClient
    {
        public string PostId { get; set; } = string.Empty;
        /// <summary>
        /// 封面照片
        /// </summary>
        public string CoverUrl { get; set; } = string.Empty;
    }
}
