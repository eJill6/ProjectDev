using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace MS.Core.MMModel.Models.My.Enums
{
    public enum MyBossPostStatus
    {
        [Description("下架")]
        RemovedFromShelves =1,
        [Description("展示")]
        OnDisplay =2,
        [Description("审核中")]
        UnderReview =3,
        [Description("未通过")]
        DidNotPass =4
    }
}
