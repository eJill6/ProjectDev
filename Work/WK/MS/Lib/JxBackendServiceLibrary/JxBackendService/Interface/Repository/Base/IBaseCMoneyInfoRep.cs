using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JxBackendService.Interface.Repository.Base
{
    public interface IBaseCMoneyInfoRep<T> : IBaseDbRepository<T>
    {
        string CreateMoneyID();

        List<T> GetProcessingOrders3DaysAgo();
    }
}