using System.Reflection;

namespace JxBackendService.Model.Util.Export
{
    public class ExportColumnSetting
    {
        public string HeaderName { get; set; }

        public int Sort { get; set; }

        public PropertyInfo ColumnProperty { get; set; }
    }
}