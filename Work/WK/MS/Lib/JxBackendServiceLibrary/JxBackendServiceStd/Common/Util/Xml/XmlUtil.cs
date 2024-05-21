using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.Util;
using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace JxBackendService.Common.Util.Xml
{
    public static class XmlUtil
    {
        public static T Deserialize<T>(string xml)
        {
            if (string.IsNullOrEmpty(xml))
            {
                return default(T);
            }

            try
            {
                var serializer = new XmlSerializer(typeof(T));
                T model = (T)serializer.Deserialize(new StringReader(xml));
                return model;
            }
            catch (Exception ex)
            {
                var logUtilService = DependencyUtil.ResolveService<ILogUtilService>().Value;
                logUtilService.Error($"Xml Deserialize Error xml:{xml}");

                throw ex;
            }
        }

        public static bool IsValidXml(string xml)
        {
            var xmlDocument = new XmlDocument();
            try
            {
                xmlDocument.LoadXml(xml);

                if (xml.StartsWith("<?xml"))
                {
                    return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}