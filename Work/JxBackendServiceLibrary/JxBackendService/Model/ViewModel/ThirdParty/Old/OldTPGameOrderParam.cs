using System;

namespace JxBackendService.Model.ViewModel.ThirdParty.Old
{
    public class OldTPGameOrderParam<ApiParamType>
    {
        public ApiParamType ApiParam { get; set; }

        public BaseTPGameMoneyInfo TPGameMoneyInfo { get; set; }
    }

    public interface IOldBetLogApiParam
    {
        string LastSearchToken { get; set; }
    }

    public interface IOldBetLogModel
    {
        string RemoteFileSeq { get; set; }

        Action WriteRemoteContentToOtherMerchant { get; set; }
    }

    public class BetLogResponseInfo
    {
        public string ApiResult { get; set; }

        public string RemoteFileSeq { get; set; }
    }
}