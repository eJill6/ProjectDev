using JxBackendService.Common.Util;
using JxBackendService.Interface.Repository;
using JxBackendService.Interface.Repository.Security;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.Security;
using JxBackendService.Model.Entity.Security;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using JxBackendService.Service.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Service.Security
{
    public class AreaService : BaseService, IAreaReadService
    {
        private readonly IIpDataRep _ipDataRep;
        private readonly IIpSystemApiService _ipSystemApiService;

        public AreaService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _ipDataRep = ResolveJxBackendService<IIpDataRep>();
            _ipSystemApiService = ResolveJxBackendService<IIpSystemApiService>();
        }

        public string GetArea(JxIpInformation jxIpInformation)
        {
            JxIpInformation ipInformation = IpUtil.GetDoWorkIPInformation().ToJxIpInformation();
            string area = _ipSystemApiService.GetArea(ipInformation);

            if (area.IsNullOrEmpty())
            {
                area = _ipDataRep.GetIpArea(ipInformation);
            }

            return area;
        }
    }
}
