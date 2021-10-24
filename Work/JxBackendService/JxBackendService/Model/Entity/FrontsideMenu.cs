using JxBackendService.Model.Entity.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.Entity
{
    public class FrontsideMenu : BaseEntityModel
    {
        public int No { get; set; }
        public string MenuName { get; set; }
        public string EngName { get; set; }
        public string PicName { get; set; }
        public string ProductCode { get; set; }
        public string GameCode { get; set; }
        public int Type { get; set; }
        public int Sort { get; set; }
        public int AppSort { get; set; }
        public string Url { get; set; }
        public bool Active { get; set; }
    }

}
