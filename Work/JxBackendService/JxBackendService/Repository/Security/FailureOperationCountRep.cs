using JxBackendService.Interface.Repository;
using JxBackendService.Interface.Repository.Security;
using JxBackendService.Model.Entity.Security;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Repository.Base;
using JxBackendService.Repository.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Repository.Security
{
    public class FailureOperationCountRep : BaseDbRepository<FailureOperationCount>, IFailureOperationCountRep
    {
        public FailureOperationCountRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {

        }
    }
}
