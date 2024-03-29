﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JxBackendService.Interface.Model.MiseLive.Request
{
    public interface IMiseLiveTransferOrderColumn
    {
        string OrderNo { get; set; }
    }

    public interface IMiseLiveTransferColumn : IMiseLiveTransferOrderColumn
    {
        decimal Amount { get; set; }
    }

    public interface IMiseLiveTransferRequest : IMiseLiveSaltRequest, IMiseLiveUserColumn, IMiseLiveTransferColumn, IMiseLiveAppRequest
    {
    }

    public interface IMiseLiveTransferRequestParam : IMiseLiveRequestParam, IMiseLiveUserColumn, IMiseLiveTransferColumn
    {
    }

    public interface IMiseLiveTransferOrderRequest : IMiseLiveSaltRequest, IMiseLiveTransferOrderColumn, IMiseLiveAppRequest
    {
    }

    public interface IMiseLiveTransferOrderRequestParam : IMiseLiveRequestParam, IMiseLiveTransferOrderColumn
    {
    }
}