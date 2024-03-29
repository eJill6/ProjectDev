﻿using System.Collections.Generic;
using JxBackendService.Model.Entity;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel.ThirdParty;

namespace JxBackendService.Interface.Service
{
    public interface ITransferCompensationService
    {
        List<int> GetTransferCompensationUserIds(PlatformProduct product);

        BaseReturnModel ProcessedMoneyOutCompensation(ProcessedCompensationParam param);

        BaseReturnModel SaveMoneyOutCompensation(SaveCompensationParam param);
    }
}