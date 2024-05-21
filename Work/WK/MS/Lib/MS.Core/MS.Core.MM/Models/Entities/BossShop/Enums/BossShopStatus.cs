using MS.Core.MMModel.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS.Core.MM.Models.Entities.BossShop.Enums
{
    public enum BossShopStatus
    {
        [ManageDescription("待审核")]
        [Description("待审核")]
        Reviewed =0,
        [ManageDescription("审核通过")]
        [Description("审核通过")]
        Pass =1,
        [ManageDescription("审核不通过")]
        [Description("审核不通过")]
        NotPass =2
    }
}
