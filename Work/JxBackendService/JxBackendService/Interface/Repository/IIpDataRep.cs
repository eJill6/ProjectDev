using IPToolModel;
using JxBackendService.Model.Entity;
using JxBackendService.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Interface.Repository
{
    public interface IIpDataRep : IBaseDbRepository<IpData>
    {
        string GetIpArea(JxIpInformation ipInfo);
    }
}
