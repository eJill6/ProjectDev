using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.ViewModel
{
    public class TPGameProfitLoss
    {
        public string ProfitLossID { get; set; }

        public DateTime ProfitLossTime { get; set; }

        public string Memo { get; set; }

        public string ProfitLossType { get; set; }

        public decimal ProfitLossMoney { get; set; }

        public int PalyID { get; set; }

        public int UserID { get; set; }

        public decimal CZProfitLossMoney { get; set; }

        public decimal TXProfitLossMoney { get; set; }

        public decimal FDProfitLossMoney { get; set; }

        public decimal KYProfitLossMoney { get; set; }

        public decimal ZKYProfitLossMoney { get; set; }

        public decimal XJFDProfitLossMoney { get; set; }

        public decimal TZProfitLossMoney { get; set; }

        public decimal HBProfitLossMoney { get; set; }

        public decimal WinMoney { get; set; }

        public int GameType { get; set; }

        public string UserName { get; set; }

        /// <summary>
        /// 用于使用用户名查询时 记录返回的UserID，方便使用
        /// </summary>
        public int SelectUserID { get; set; }
    }
}
