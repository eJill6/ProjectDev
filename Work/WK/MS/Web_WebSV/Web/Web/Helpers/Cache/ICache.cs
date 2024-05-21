using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Helpers.Cache
{
    public interface ICache
    {
        T Get<T>(string key);

        bool Set<T>(string key, T obj);

        bool Set<T>(string key, T obj, DateTime expired);

        bool Del(string key);
    }

    public interface ICacheRemote : ICache
    {
    }
}
