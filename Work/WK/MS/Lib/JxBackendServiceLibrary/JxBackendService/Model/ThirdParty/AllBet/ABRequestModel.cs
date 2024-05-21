using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.ThirdParty.AllBet
{
    public class ABUserBaseRequestModel
    {
        public string client { get; set; }
    }

    public class ABUserBaseWithPasswordRequestModel : ABUserBaseRequestModel
    {
        public string password { get; set; }
    }

    public class ABRegisterRequestModel : ABUserBaseWithPasswordRequestModel
    {
        public int orHallRebate { get; set; } = 0;
    }

    public class ABMobileLunchGameRequestModel : ABUserBaseWithPasswordRequestModel
    {
        public int appType { get; set; }
    }

    public class ABCheckTransferRequestModel
    {
        public string sn { get; set; }
    }

    public class ABTransferRequestModel : ABUserBaseRequestModel
    {
        public string sn { get; set; }
        public int operFlag { get; set; }
        public string credit { get; set; }
    }

    public class ABBetLogRequestModel
    {
        public string startTime { get; set; }
        public string endTime { get; set; }

    }

}
