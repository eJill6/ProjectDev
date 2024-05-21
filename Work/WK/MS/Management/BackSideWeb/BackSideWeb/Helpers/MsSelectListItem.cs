using JxBackendService.Model.Common;
using JxBackendService.Model.Enums.BackSideWeb.BotBet;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BackSideWeb.Helpers
{
    public class MsSelectListItem
    {
        public static List<JxBackendSelectListItem> GetTimeTypeItems(string botGroup)
        {
            if (Enum.TryParse(botGroup, out BotGroup groupEnum))
            {
                if ((BotGroup)Convert.ToInt16(botGroup) == BotGroup.B)
                {
                    return new List<JxBackendSelectListItem>() {
                         new JxBackendSelectListItem("0","T1"),
                         new JxBackendSelectListItem("1","T2"),
                         new JxBackendSelectListItem("2","T3"),
                    };
                }
                else if ((BotGroup)Convert.ToInt16(botGroup) == BotGroup.C)
                {
                    return new List<JxBackendSelectListItem>() {
                         new JxBackendSelectListItem("0","T1"),
                         new JxBackendSelectListItem("1","T2"),
                      };
                }
                else
                {
                    return new List<JxBackendSelectListItem>() {
                         new JxBackendSelectListItem("0","T1"),
                         new JxBackendSelectListItem("1","T2"),
                         new JxBackendSelectListItem("2","T3"),
                         new JxBackendSelectListItem("3","T4"),
                       };
                }
            }
            return new List<JxBackendSelectListItem>();
        }
    }
}
