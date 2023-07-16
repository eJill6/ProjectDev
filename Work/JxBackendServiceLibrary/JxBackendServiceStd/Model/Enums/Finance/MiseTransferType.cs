using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JxBackendService.Model.Enums.Finance
{
    public class MiseTransferType : BaseIntValueModel<MiseTransferType>
    {
        private MiseTransferType()
        {
        }

        public static MiseTransferType In = new MiseTransferType() { Value = 1 };

        public static MiseTransferType Out = new MiseTransferType() { Value = 2 };
    }
}