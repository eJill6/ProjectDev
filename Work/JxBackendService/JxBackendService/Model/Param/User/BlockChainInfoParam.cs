using JxBackendService.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.Param.User
{
    public class BasicBlockChainInfoParam
    {
        public string WalletAddr { get; set; }
        
        public bool IsActive { get; set; }
        
        public string ClientPin { get; set; }

        public string MoneyPasswordHash { get; set; }
    }


    public class BlockChainInfoParam : BasicBlockChainInfoParam
    {
        public BaseBasicUserInfo LoginUser { get; set; }
        
        public string IpAddress { get; set; }
    }

    public class ProUpdateBlockChainInfoReturnModel
    {
        public bool IsSuccess { get; set; }
        
        public string ReturnMsg { get; set; }
    }
}
