using System;
using System.Reflection;
using System.ServiceProcess;

namespace JxBackendServiceNF.Common.Util
{
    public class ReflectUtilNF
    {
        /// <summary>
        /// 屬性切換到主控台Debug
        /// </summary>
        public static void RunInteractive(params ServiceBase[] services)
        {
            Console.WriteLine("Install the services in interactive mode.");
            //Start
            var onStartMethod = typeof(ServiceBase).GetMethod("OnStart", BindingFlags.Instance | BindingFlags.NonPublic);

            foreach (ServiceBase service in services)
            {
                onStartMethod.Invoke(service, new object[] { new string[] { } });
            }

            Console.WriteLine("Press a key to uninstall all services...");
            Console.ReadKey();

            //Stop
            var onStopMethod = typeof(ServiceBase).GetMethod("OnStop", BindingFlags.Instance | BindingFlags.NonPublic);

            foreach (var service in services)
            {
                onStopMethod.Invoke(service, null);
            }
        }

        public static void CopyValue(object source, object target)
        {
            PropertyInfo[] propertyInfos = target.GetType().GetProperties();
            Type sourceType = source.GetType();

            foreach (PropertyInfo propertyInfo in propertyInfos)
            {
                if (!propertyInfo.CanWrite)
                {
                    continue;
                }

                string propertyName = propertyInfo.Name;
                PropertyInfo sourcePropertyInfo = sourceType.GetProperty(propertyName);

                if (sourcePropertyInfo == null)
                {
                    continue;
                }

                propertyInfo.SetValue(target, sourcePropertyInfo.GetValue(source), null);
            }
        }
    }
}