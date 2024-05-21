using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JxBackendService.Common.Exceptions
{
    public class ViewNotFoundException : Exception
    {
        public ViewNotFoundException(string view) : base($"{view} not found.")
        {
        }
    }
}
