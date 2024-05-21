using MS.Core.MM.Models.Entities.Post;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS.Core.MM.Models.Post
{
    public class UpdateMMGoldStoreParam
    {
        /// <summary>
        /// Top
        /// </summary>
        public int Top { get; set; }

        /// <summary>
        /// 会员ID
        /// </summary>
        public int? UserId { get; set; }

        public int Type { get; set; }

        /// <summary>
        /// 操作人員
        /// </summary>
        public string Operator { get; set; } = string.Empty;
    }
}