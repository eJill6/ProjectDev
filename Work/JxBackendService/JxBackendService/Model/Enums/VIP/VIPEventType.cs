using System;
using JxBackendService.Resource.Element;
using JxBackendService.Service.User.Activity;

namespace JxBackendService.Model.Enums.VIP
{
    public class VIPEventType : BaseIntValueModel<VIPEventType>
    {
        public Type VIPUserEventServiceType { get; private set; }

        private VIPEventType() { }

        /// <summary> VIP月存款福利 </summary>
        public static readonly VIPEventType MonthlyDepositPrize = new VIPEventType()
        {
            Value = 1,
            ResourceType = typeof(VIPContentElement),
            ResourcePropertyName = nameof(VIPContentElement.MonthlyDepositPrize),
            VIPUserEventServiceType = typeof(VIPMonthlyDepositService),
        };
    }
 }
