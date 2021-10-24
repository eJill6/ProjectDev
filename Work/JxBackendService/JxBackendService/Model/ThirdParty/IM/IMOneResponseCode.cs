using JxBackendService.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.ThirdParty.IM
{
    public class IMOneResponseCode : BaseStringValueModel<IMOneResponseCode>
    {
        private IMOneResponseCode() { }

        /// <summary>成功</summary>
        public static readonly IMOneResponseCode Success = new IMOneResponseCode() { Value = "0" };

        /// <summary>無數據</summary>
        public static readonly IMOneResponseCode NoDataFound = new IMOneResponseCode() { Value = "558" };
    }

    public class IMOneResponseStatus : BaseStringValueModel<IMOneResponseStatus>
    {
        private IMOneResponseStatus() { }

        public static readonly IMOneResponseStatus Approved = new IMOneResponseStatus() { Value = "Approved" };

        public static readonly IMOneResponseStatus Declined = new IMOneResponseStatus() { Value = "Declined" };

        public static readonly IMOneResponseStatus Processed = new IMOneResponseStatus() { Value = "Processed" };
    }

    public class IMLotteryOrderStatus : BaseStringValueModel<IMLotteryOrderStatus>
    {
        private IMLotteryOrderStatus() { }

        public static IMLotteryOrderStatus Open = new IMLotteryOrderStatus() { Value = "Open" };
        public static IMLotteryOrderStatus Settled = new IMLotteryOrderStatus() { Value = "Settled" };
        public static IMLotteryOrderStatus Cancelled = new IMLotteryOrderStatus() { Value = "Cancelled" };
        public static IMLotteryOrderStatus Adjusted = new IMLotteryOrderStatus() { Value = "Adjusted" };
    }
}
