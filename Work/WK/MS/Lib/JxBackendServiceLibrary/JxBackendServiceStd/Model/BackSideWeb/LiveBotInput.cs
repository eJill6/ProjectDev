using JxBackendService.Model.Enums.BackSideWeb.BotBet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;

namespace JxBackendService.Model.BackSideWeb
{
    public class LiveBotInput : AnchorInfoContext
    {
        public long OriginalId { get; set; }

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
    }
}
