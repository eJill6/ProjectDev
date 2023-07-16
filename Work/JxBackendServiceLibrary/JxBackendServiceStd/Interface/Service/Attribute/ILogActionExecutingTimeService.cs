using System;
using System.Collections.Generic;
using System.Text;

namespace JxBackendService.Interface.Service.Attribute
{
    public interface ILogActionExecutingTimeService : IActionFilterService
    {
        void Init(double? warningMilliseconds);
    }
}