using JxBackendService.Model.Enums;

namespace JxBackendService.Model.Common
{
    public class OperationLogSetting
    {
        public JxOperationLogCategory Category { get; set; }

        public string MemoTemplate { get; set; }

        public bool IsMemoRequired { get; set; }
    }
}