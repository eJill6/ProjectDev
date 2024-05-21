using JxBackendService.Model.Attributes;
using JxBackendService.Model.Entity.Base;

namespace JxBackendService.Model.Entity.Util
{
    public class MachineGenerator : BaseEntityModel
    {
        [ExplicitKey]
        [NVarcharColumnInfo(50)]
        public string MachineName { get; set; }

        public int GeneratorID { get; set; }
    }
}