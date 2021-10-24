using IPToolModel;
using JxBackendService.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Interface.Service
{
    public interface IIpSystemApiService
    {
        string CompositeToString(IpSystemApiResult ipSystemApiResult);

        long GetTimestamp();

        IpSystemApiResult Query(string queryIP);

        string GetArea(JxIpInformation ipInfo);
    }
}
