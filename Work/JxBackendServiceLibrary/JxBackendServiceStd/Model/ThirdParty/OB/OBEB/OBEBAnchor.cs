using JxBackendService.Interface.Model.MiseLive.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JxBackendService.Model.ThirdParty.OB.OBEB
{
    public class OBEBAnchor
    {
        public int AnchorId { get; set; }

        public string UserName { get; set; }

        public string NickName { get; set; }

        public string Avatar { get; set; }

        public string SmallAvatar { get; set; }

        public int LiveStatus { get; set; }

        public string StartTime { get; set; }

        public string EndTime { get; set; }

        public string FansNum { get; set; }

        public decimal WinRangeLive { get; set; }

        public int GameTypeIdLive { get; set; }

        public int GameStateLive { get; set; }

        public int OnlineLive { get; set; }

        public int FollowLive { get; set; }

        public int ShowStatus { get; set; }

        public int EntranceStatus { get; set; }

        public int AccountStatus { get; set; }

        public string Description { get; set; }
    }
}