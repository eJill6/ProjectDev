using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Interface.Service.Web
{
    public interface IHttpContextService
    {
        string GetAbsoluteUri();
    }
}