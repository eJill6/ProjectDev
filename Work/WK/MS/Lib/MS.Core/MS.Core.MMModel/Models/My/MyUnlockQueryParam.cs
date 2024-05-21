using MS.Core.MMModel.Models.Post.Enums;
using MS.Core.Models.Models;

namespace MS.Core.MMModel.Models.My
{
    public class MyUnlockQueryParam : MyUnlockQueryParamForClient
    {
        /// <summary>
        /// 使用者編號
        /// </summary>
        public int UserId { get; set; }
    }
}
