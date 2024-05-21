using JxBackendService.Model.ViewModel;

namespace JxBackendService.Model.ErrorHandle
{
    public class BaseSendTelegramParam
    {
        /// <summary>
        /// 實際API Url或返代網址
        /// </summary>
        public string ApiUrl { get; set; }

        public string Message { get; set; }
    }

    public class SendTelegramParam : BaseSendTelegramParam
    {
        public EnvironmentUser EnvironmentUser { get; set; }
    }
}