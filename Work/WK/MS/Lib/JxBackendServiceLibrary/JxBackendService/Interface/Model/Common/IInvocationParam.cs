using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JxBackendService.Interface.Model.Common
{
    public interface IInvocationParam
    {
        string CorrelationId { get; set; }
    }

    public interface IInvocationUserParam : IInvocationParam
    {
        int UserID { get; set; }
    }
}