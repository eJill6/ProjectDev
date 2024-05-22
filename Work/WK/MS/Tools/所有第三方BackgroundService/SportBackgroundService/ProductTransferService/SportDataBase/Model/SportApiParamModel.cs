using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.Config;
using JxBackendService.Model.ViewModel.ThirdParty.Old;
using JxBackendService.Common.Util;

namespace ProductTransferService.SportDataBase.Model
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

    public class SportAppSetting
    {
        private readonly Lazy<IConfigUtilService> _configUtilService;

        public SportAppSetting()
        {
            _configUtilService = DependencyUtil.ResolveService<IConfigUtilService>();
        }

        public string URL => _configUtilService.Value.Get("URL", string.Empty).Trim();

        public string VendorID => _configUtilService.Value.Get("VendorID", string.Empty).Trim();

        public int Currency => _configUtilService.Value.Get("Currency", string.Empty).Trim().ToInt32();
    }
}