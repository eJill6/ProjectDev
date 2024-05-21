using JxBackendService.Model.Entity.BackSideUser;
using Enum = JxBackendService.Model.Enums.BackSideWeb;

namespace JxBackendService.Model.ViewModel.SystemSetting
{
    public class OperationLogViewModel : BWOperationLog
    {
        public Enum.OperationType OperationTypeSetting => Enum.OperationType.GetSingle(OperationType);

        public string OperationTypeText => OperationTypeSetting.Name;

        public object ContentModel { get; set; }
    }
}