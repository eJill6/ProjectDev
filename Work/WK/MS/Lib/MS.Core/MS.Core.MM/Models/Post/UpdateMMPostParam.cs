using MS.Core.MM.Models.Entities.Post;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS.Core.MM.Models.Post
{
    public class UpdateMMPostParam
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 權重
        /// </summary>
        public int Weight { get; set; }
        /// <summary>
        /// 操作人員
        /// </summary>
        public string Operator { get; set; } = string.Empty;
    }
}
