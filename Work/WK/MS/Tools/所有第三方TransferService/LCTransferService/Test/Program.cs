using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            LCTransferService.LCTransferService sport = new LCTransferService.LCTransferService();
            sport.InitAppSettings();
            LCDataBase.BLL.UserBll.RefreshAvailableScores(true);
        }
    }
}
