using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JxBackendService.Interface.Model.MiseLive.Request
{
    public interface IMiseLiveUserBalanceRequest : IMiseLiveSaltRequest, IMiseLiveUserColumn
    {
    }

    public interface IMiseLiveUserBalanceRequestParam : IMiseLiveRequestParam, IMiseLiveUserColumn
    {
    }
}