using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateStoredProcedures
{
    public class ColumnInfo
    {
        public string ColumnName { get; set; }
        public string Description { get; set; }
        public string IsNullable { get; set; }
        public string DataType { get; set; }
        public int? MaxLength { get; set; }
        public int? Precision { get; set; }
        public int? Scale { get; set; }
    }
}
