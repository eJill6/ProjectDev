using JxBackendService.Model.ViewModel.ThirdParty.Old;
using System;
using System.Configuration;

namespace SportDataBase.Model
{
    public class SportApiParamModel : IOldBetLogApiParam
    {
        /// <summary>
        /// 轉入或轉出
        /// </summary>
        public string Type { get; set; }

        public string TransferID { get; set; }

        public string OrderID { get; set; }

        public string UserID { get; set; }

        public string Money { get; set; }

        public string TPGameUserID { get; set; }

        public string LastVersionKey { get; set; }

        public string LastSearchToken
        {
            get { return LastVersionKey; }
            set { LastVersionKey = value; }
        }
    }

    public static class SportAppSettings
    {
        public static string URL => ConfigurationManager.AppSettings["URL"].Trim();

        public static string VendorID => ConfigurationManager.AppSettings["VendorID"].Trim();

        public static int Currency => Convert.ToInt32(ConfigurationManager.AppSettings["Currency"].Trim());
    }
}