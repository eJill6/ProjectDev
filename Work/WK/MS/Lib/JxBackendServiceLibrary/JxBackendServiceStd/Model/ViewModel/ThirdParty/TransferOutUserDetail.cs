﻿using JxBackendService.Interface.Model.Common;
using JxBackendService.Model.MessageQueue;

namespace JxBackendService.Model.ViewModel.ThirdParty
{
    public class TransferOutUserDetail : IInvocationParam
    {
        public BaseBasicUserInfo AffectedUser { get; set; }

        public string CorrelationId { get; set; }

        public bool IsCompensation { get; set; }

        public RoutingSetting RoutingSetting { get; set; }
    }
}