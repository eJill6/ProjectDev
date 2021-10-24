using JxBackendService.Resource.Element;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.Enums
{
    public class TransferToChildStatus : BaseIntValueModel<TransferToChildStatus>
    {
        private TransferToChildStatus() { }

        public static TransferToChildStatus Enabled = new TransferToChildStatus()
        {
            Value = 1,
            ResourceType = typeof(CommonElement),
            ResourcePropertyName = nameof(CommonElement.Enable)
        };

        public static TransferToChildStatus Disabled = new TransferToChildStatus()
        {
            Value = 0,
            ResourceType = typeof(CommonElement),
            ResourcePropertyName = nameof(CommonElement.CloseDown)
        };

        public static TransferToChildStatus ForceDisabled = new TransferToChildStatus()
        {
            Value = 2,
            ResourceType = typeof(CommonElement),
            ResourcePropertyName = nameof(CommonElement.ForceClose)
        };
    }
}
