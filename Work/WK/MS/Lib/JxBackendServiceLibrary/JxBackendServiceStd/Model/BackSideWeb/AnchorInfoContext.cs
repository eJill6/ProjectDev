using JxBackendService.Model.Entity.Base;
using System.ComponentModel.DataAnnotations;

namespace JxBackendService.Model.BackSideWeb
{
    public class AnchorInfoContext
    {
        /// <summary>
        /// 直播間Id
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 組別編號(ex: A、B、C)
        /// </summary>

        public int GroupId { get; set; }
    }
}
