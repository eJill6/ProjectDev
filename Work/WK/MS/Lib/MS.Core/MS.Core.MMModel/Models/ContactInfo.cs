using MS.Core.MMModel.Models.Post.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MS.Core.MMModel.Models
{
    public class ContactInfo
    {
        /// <summary>
        /// 聯絡方式。1：微信、2：QQ、3：手機號
        /// </summary>
        public ContactType ContactType { get; set; }

        /// <summary>
        /// 聯繫方式
        /// </summary>
        public string Contact { get; set; } = string.Empty;
    }
}
