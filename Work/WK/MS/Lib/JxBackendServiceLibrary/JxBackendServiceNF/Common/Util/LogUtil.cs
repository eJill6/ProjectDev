using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace JxBackendServiceNF.Common.Util
{
    public static class LogUtil
    {
        static LogUtil()
        {
            //找最符合的config
            string directoryPath = AppDomain.CurrentDomain.BaseDirectory;

            FileInfo[] configFileInfos = Directory.GetFiles(directoryPath, "*.config").Select(s => new FileInfo(s)).ToArray();

            if (!configFileInfos.Any())
            {
                return;
            }

            //尋找優先序
            string[] findConfigFileNames = new string[] { "web.config", "*.exe.config", "app.config" };

            foreach (string configFileName in findConfigFileNames)
            {
                FileInfo foundConfigFileInfo = null;

                if (configFileName.StartsWith("*"))
                {
                    foundConfigFileInfo = configFileInfos
                        .Where(w => w.Name.EndsWith(configFileName.Replace("*", string.Empty), StringComparison.OrdinalIgnoreCase))
                        .FirstOrDefault();
                }
                else
                {
                    foundConfigFileInfo = configFileInfos.Where(w => w.Name.Equals(configFileName, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                }

                if (foundConfigFileInfo != null)
                {
                    XmlConfigurator.Configure(foundConfigFileInfo);
                    return;
                }
            }
        }

        private static ILog GetLogger(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }
            else
            {
                return LogManager.GetLogger(name);
            }
        }

        public static void Debug<T>(T value)
        {
            Debug(null, value);
        }

        public static void Debug<T>(string loggerName, T value)
        {
            GetLogger(loggerName).Debug(value);
        }

        public static void ForcedDebug<T>(T value)
        {
            ForcedDebug(null, value);
        }

        public static void ForcedDebug<T>(string loggerName, T value)
        {
            string content = null;
            
            if (value != null)
            {
                content = $"|{nameof(ForcedDebug)}|{value.ToString()}";
            }

            //避免運維把Error等級關掉，將等級Error改為Fatal
            GetLogger(loggerName).Fatal(content);
        }

        public static void Info<T>(T value)
        {
            Info(null, value);
        }

        public static void Info<T>(string loggerName, T value)
        {
            GetLogger(loggerName).Info(value);
        }

        public static void Warn<T>(T value)
        {
            Warn(null, value);
        }

        public static void Warn<T>(string loggerName, T value)
        {
            GetLogger(loggerName).Warn(value);
        }

        public static void Error<T>(T value)
        {
            Error(null, value);
        }

        public static void Error<T>(string loggerName, T value)
        {
            //避免運維把Error等級關掉，將等級Error改為Fatal
            GetLogger(loggerName).Fatal(value);
        }

        //public static void Error(Exception ex)
        //{
        //    GetLogger().Error(ex);
        //}
    }
}