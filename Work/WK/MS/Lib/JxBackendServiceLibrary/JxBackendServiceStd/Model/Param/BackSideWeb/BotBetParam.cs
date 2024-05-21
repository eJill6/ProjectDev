using JxBackendService.Model.Enums.BackSideWeb.BotBet;
using System;
using System.Collections.Generic;
using System.Text;

namespace JxBackendService.Model.Param.BackSideWeb
{
    public class BotBetParam : PagedParam
    {
        public int LotteryPatchType { get; set; }
        public BotGroup? BotGroup { get; set; }
        public int? TimeType { get; set; }
        public SettingGroup? SettingGroup { get; set; }
    }
}
