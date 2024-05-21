using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLPolyGame.Web.MSSeal.Models
{
    public class BalanceDetail
    {
        /// <summary>
        /// 餘額資訊
        /// </summary>
        public string Balance { get; set; }
    }

    public class BalanceResult : ResultModel<BalanceDetail>
    {
    }
}
