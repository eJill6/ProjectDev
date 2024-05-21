using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace MS.Core.MMModel.Models.My.Enums
{
    public enum OfficialPostOnTheShelves
    {
        /// <summary>
        /// 
        /// </summary>
        [Description("上架")]
        OnShelves = 0,
        
        /// <summary>
        /// 
        /// </summary>
        [Description("下架")]
        RemovedFromShelves = 1
    }
}
