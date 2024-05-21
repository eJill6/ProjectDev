using JxBackendService.Interface.Service.Web.BackSideWeb;
using JxBackendService.Model.BackSideWeb;
using JxBackendService.Model.Enums.BackSideWeb.BotBet;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BackSideWeb.Models.ViewModel
{
    public class AnchorInfoContextViewModel: AnchorInfoContext, IDataKey
    {
        public string? KeyContent => Id.ToString();
        public string GroupIdText
        {
            get
            {
                if (Enum.IsDefined(typeof(BotGroup), GroupId))
                {
                    return ((BotGroup)GroupId).ToString();
                }
                else
                {
                    return GroupId.ToString();
                }
            }
        }
        public List<SelectListItem> BotGroupItems { get; set; }

        /// <summary>
        /// 原直播間Id
        /// </summary>
        public long OriginalId { get; set; }
    }
}
