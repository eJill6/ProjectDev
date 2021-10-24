using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JxBackendService.Model.Param.User
{
    public class BaseUpdateBankInfoParam
    {
        public int BankID { get; set; }

        public int BankType { get; set; }

        public string OriginalBankName { get; set; }

        public string BankName { get; set; }

        /// <summary>
        /// 銀行卡卡號
        /// </summary>
        public string BankCardNo { get; set; }

        /// <summary>省份</summary>
        public string Province { get; set; }

        /// <summary>城市</summary>
        public string City { get; set; }        
    }

    public class UpdateBankInfoParam : BaseUpdateBankInfoParam
    {        
        public string ClientPin { get; set; }

        public string AccessToken { get; set; }
    }
}
