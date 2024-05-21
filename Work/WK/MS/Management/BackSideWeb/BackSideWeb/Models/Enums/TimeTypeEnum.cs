using System.ComponentModel;

namespace BackSideWeb.Models.Enums
{
    public enum TimeTypeEnum
    {
        [Description("首次送审时间")]
        CreateTime,
        [Description("再次送审时间")]
        UpdateTime,
        [Description("审核时间")]
        ExamineTime
    }
}
