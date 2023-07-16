using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateStoredProcedures
{
    public class SpFileName
    {
        public string OutputFileName { get; set; }
        public string RollbackFileName { get; set; }
    }

    public class SpContent
    {
        public string OutputContent { get; set; }
        public string RollbackContent { get; set; }
    }
}
