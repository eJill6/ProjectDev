using JxBackendService.Model.Attributes;
using JxBackendService.Model.Entity.Base;

namespace JxBackendService.Model.Entity.Setting
{
    public class RefreshFrequencySetting : BaseEntityModel
    {
        [ExplicitKey]
        public int UserID { get; set; }

        [ExplicitKey]
        public string PermissionKey { get; set; }

        public int IntervalSeconds { get; set; }
    }
}