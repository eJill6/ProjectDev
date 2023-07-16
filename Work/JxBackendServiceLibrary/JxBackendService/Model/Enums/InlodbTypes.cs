using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.Enums
{
    public class InlodbType : BaseStringValueModel<InlodbType>
    {
        private InlodbType() { }

        public static readonly InlodbType Inlodb = new InlodbType() { Value = "Inlodb" };
        public static readonly InlodbType InlodbBak = new InlodbType() { Value = "Inlodb_bak" };
    }
}
