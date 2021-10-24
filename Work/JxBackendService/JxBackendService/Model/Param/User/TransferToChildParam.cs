using JxBackendService.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JxBackendService.Model.Param.User
{
    public class BasicTransferToChildParam
    {
        public string MoneyPasswordHash { get; set; }

        public string ChildUserName { get; set; }

        public decimal TransferAmount { get; set; }

        public string ClientPin { get; set; }
    }

    public class TransferToChildParam : BasicTransferToChildParam
    {
        public BaseBasicUserInfo LoginUser { get; set; }

        public string IpAddress { get; set; }
    }
}
