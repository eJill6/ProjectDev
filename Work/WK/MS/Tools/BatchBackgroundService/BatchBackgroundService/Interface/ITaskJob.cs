using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatchService.Interface
{
    public interface ITaskJob
    {
        void DoJob(CancellationToken cancellationToken);
    }
}