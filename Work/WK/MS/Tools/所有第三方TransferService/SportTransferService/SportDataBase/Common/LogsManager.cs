using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.Util;
using JxBackendServiceNF.Service.Util;

namespace SportDataBase.Common
{
    public class LogsManager
    {
        public static void Info(object message)
        {
            var logUtilService = DependencyUtil.ResolveService<ILogUtilService>();
            logUtilService.Info(message);
        }

        public static void InfoToEmail(object message)
        {
            var logUtilService = DependencyUtil.ResolveService<ILogUtilService>();
            logUtilService.Info(message);
        }

        public static void Error(object message)
        {
            var logUtilService = DependencyUtil.ResolveService<ILogUtilService>();
            logUtilService.Error(message);
        }

        public static void ForcedDebug(object message)
        {
            var logUtilService = DependencyUtil.ResolveService<ILogUtilService>();
            logUtilService.ForcedDebug(message);
        }
    }
}