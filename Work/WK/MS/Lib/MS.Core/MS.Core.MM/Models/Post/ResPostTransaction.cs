using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS.Core.MM.Models.Post
{
    public class ResPostTransaction
    {
        /// <summary>
        /// 用戶解鎖可以得到的訊息
        /// </summary>
        public UserUnlockGetInfo? UnlockInfo { get; set; }
        public bool IsFree { get; set; }
    }
}
