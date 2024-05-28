using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatchService.Interface
{
    public interface IQueueUserWorkItemJob
    {
        void DoJob(object state);
    }
}
