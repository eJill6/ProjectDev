using System.Collections.Generic;

namespace JxBackendService.Model.ThirdParty.WLBG
{
    public class WLBGBaseResponse
    {
        public int Status { get; set; }
    }

    /// <summary> 注册接口返回数据 </summary>
    public class WLBGRegisterResponseModel : WLBGBaseResponseWithDataModel<WLBGBaseResponse>
    {
    }

    public class LaunchGameModel
    {
        /// <summary> 游戏地址 </summary>
        public string GameUrl { get; set; }

        public string GameReason { get; set; }
    }

    /// <summary> 取得 URL </summary>
    public class WLBGLaunchGameResponseModel : WLBGBaseResponseWithDataModel<LaunchGameModel>
    {
    }

    public class CheckTransferModel : WLBGBaseResponse
    {
        public string OrderId { get; set; }

        public string Reason { get; set; }

        public string Credit { get; set; }

        public string Balance { get; set; }
    }

    public class WLBGCheckTransferResponseModel : WLBGBaseResponseWithDataModel<CheckTransferModel>
    {
    }

    public class TansferModel : WLBGBaseResponse
    {
        public string OrderId { get; set; }

        public string Reason { get; set; }

        public string Balance { get; set; }
    }

    public class WLBGTransferResponseModel : WLBGBaseResponseWithDataModel<TansferModel>
    {
    }

    public class BalanceModel : WLBGBaseResponse
    {
        public string Balance { get; set; }

        public string Transferable { get; set; }
    }

    public class WLBGBalanceResponseModel : WLBGBaseResponseWithDataModel<BalanceModel>
    {
    }

    public class WLBGBetLogResponseModel : WLBGBaseResponseWithDataModel<WLBGBetRecordResult>
    {
    }

    public class WLBGGameListDataResponseModel
    {
        public string IconUrl { get; set; }

        public List<WLBGGameResponseModel> Games { get; set; }
    }

    public class WLBGGameResponseModel
    {
        public int Game { get; set; }

        public string Name { get; set; }

        public int Category { get; set; }
    }
}