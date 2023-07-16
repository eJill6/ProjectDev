using JxBackendService.Model.Enums;
using JxBackendService.Resource.Element;

namespace JxBackendService.Model.ThirdParty.WLBG
{
    public class WLBGResponseCode : BaseIntValueModel<WLBGResponseCode>
    {
        private WLBGResponseCode()
        { }

        /// <summary>成功</summary>
        public static readonly WLBGResponseCode Success = new WLBGResponseCode() { Value = 0 };
    }

    public class WLBGResponseStatus : BaseIntValueModel<WLBGResponseStatus>
    {
        private WLBGResponseStatus()
        { }

        /// <summary>未找到</summary>
        public static readonly WLBGResponseStatus NotFound = new WLBGResponseStatus() { Value = -1 };

        /// <summary>成功</summary>
        public static readonly WLBGResponseStatus Success = new WLBGResponseStatus() { Value = 1 };

        /// <summary>Failed</summary>
        public static readonly WLBGResponseStatus Failed = new WLBGResponseStatus() { Value = 2 };
    }
}