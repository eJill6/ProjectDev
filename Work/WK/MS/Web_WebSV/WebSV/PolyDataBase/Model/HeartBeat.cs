using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SLPolyGame.Web.Model
{
    public class HeartBeat
    {
        public string Key { get; set; }
        public string UserName { get; set; }
        public int UserId { get; set; }
        public DateTime LastHeartBeatTime { get; set; }
        public string LastHeartBeartIP { get; set; }
        public bool IsOnline{get;set;}
        public string Version { get; set; }
    }
}
