using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JxBackendService.Model.MiseLive.Response
{
    public class BaseMiseLiveResponse
    {
        public bool Success { get; set; }

        public string Error { get; set; }
    }

    public class MiseLiveResponse<T> : BaseMiseLiveResponse where T : class
    {
        public T Data { get; set; }
    }
}