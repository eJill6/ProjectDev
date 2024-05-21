using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLPolyGame.Web.MSSeal.Models
{
    /// <summary>
    /// 餘額請求
    /// </summary>
    public class BalanceRequest : BaseRequest
    {
        /// <summary>
        /// 使用者編號
        /// </summary>
        public int UserId { get; set; }

        public override Method GetMethod() => Method.Get;

        public override string GetResource() => "/dapi/user/balance";
    }
}
