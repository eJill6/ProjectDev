using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JxBackendService.Interface.Model.AWCSP
{
    public interface IAWCLunchGameCode
    {
        string GameCode { get; set; }

        string Platform { get; set; }
    }
}