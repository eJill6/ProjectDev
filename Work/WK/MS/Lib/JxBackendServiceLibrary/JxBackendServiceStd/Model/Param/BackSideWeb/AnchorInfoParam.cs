using System;
using System.Collections.Generic;
using System.Text;

namespace JxBackendService.Model.Param.BackSideWeb
{
    public class AnchorInfoParam : PagedParam
    {
        /// <summary>
        /// 直播間Id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 組別編號(ex: A、B、C)
        /// </summary>

        public int? GroupId { get; set; }
    }
}
