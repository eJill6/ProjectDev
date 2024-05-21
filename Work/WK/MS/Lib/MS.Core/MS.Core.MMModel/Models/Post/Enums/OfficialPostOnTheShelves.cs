using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace MS.Core.MMModel.Models.Post.Enums
{
    public enum OfficialPostOnTheShelves
    {
        [Description("上架")]
        OnShelves =0,
        [Description("下架")]
        RemovedFromShelves =1
    }
}
