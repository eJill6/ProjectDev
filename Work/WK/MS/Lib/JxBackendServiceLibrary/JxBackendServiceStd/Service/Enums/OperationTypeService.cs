using JxBackendService.Interface.Service.Enums;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.BackSideWeb;
using System.Collections.Generic;

namespace JxBackendService.Service.Enums
{
    public class OperationTypeService : BaseValueModelService<string, OperationType>, IOperationTypeService
    {
        protected override List<OperationType> CreateAllList()
        {
            List<OperationType> operationTypes = base.CreateAllList();

            if (SharedAppSettings.GetEnvironmentCode() != EnvironmentCode.Development)
            {
                operationTypes.RemoveAll(r => r == OperationType.DemoCRUD);
            }

            return operationTypes;
        }
    }
}