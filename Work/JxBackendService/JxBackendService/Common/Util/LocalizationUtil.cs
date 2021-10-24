using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using JxBackendService.Common.Extensions;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.Resource;
using JxBackendService.Model.Util;
using JxBackendService.Resource.Element;

namespace JxBackendService.Common.Util
{
    public static class LocalizationUtil
    {
        private static readonly ConcurrentDictionary<string, ResourceManager> ResourceManagerMaps = new ConcurrentDictionary<string, ResourceManager>();
        private static readonly ConcurrentDictionary<string, Dictionary<string, string>> ResourceDictionaryMapper = new ConcurrentDictionary<string, Dictionary<string, string>>();
        private static readonly string _defaultNamespace = typeof(DBContentElement).FullName;
        private const string CommonReferenceAssemblyName = "JxBackendService";

        /// <summary>
        /// 多國語系序列化寫入DB
        /// </summary>
        /// <param name="localizationParam"></param>
        /// <returns></returns>
        public static string ToLocalizationJsonString(this LocalizationParam localizationParam)
        {
            if (localizationParam == null)
            {
                return string.Empty;
            }

            return localizationParam.ToJsonString(ignoreNull: true);
        }

        /// <summary>
        /// DB字串反解成對應多國語系的內容
        /// </summary>
        /// <param name="dbJsonString"></param>
        /// <returns></returns>
        public static string ToLocalizationContent(this string dbJsonString)
        {
            if (dbJsonString.IsNullOrEmpty())
            {
                return string.Empty;
            }

            LocalizationParam localizationParam = null;

            try
            {
                localizationParam = dbJsonString.Deserialize<LocalizationParam>();
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex);
            }

            if (localizationParam == null)
            {
                return dbJsonString;
            }

            return ToLocalizationContent(localizationParam);
        }

        public static string ToLocalizationContent(this LocalizationParam localizationParam)
        {
            var contents = new List<string>();

            foreach (LocalizationSentence sentence in localizationParam.LocalizationSentences)
            {
                contents.Add(GetElementContent(sentence));
            }

            if (contents.Count == 1)
            {
                return contents.First();
            }

            string splitOperator = GetSplitOperator(localizationParam.SplitOperator);
            string returnContent = string.Join(splitOperator, contents);

            return returnContent;
        }

        /// <summary>
        /// 取得 ResourceElement 對應的內容
        /// </summary>
        /// <param name="sentence"></param>
        /// <returns></returns>
        private static string GetElementContent(LocalizationSentence sentence)
        {
            if (sentence == null)
            {
                return string.Empty;
            }

            string fullAssemblyName = sentence.ResourceName;

            // 若空的預設 DBContentElement
            if (fullAssemblyName.IsNullOrEmpty())
            {
                fullAssemblyName = _defaultNamespace;
            }

            // 取得建立 ResourceManager 取對應的字串內容
            ResourceManager resourceManager = GetCachedResourceManager(fullAssemblyName);

            if (resourceManager == null)
            {
                return sentence.ResourcePropertyName;
            }

            string elementContent = resourceManager.GetString(sentence.ResourcePropertyName);

            if (elementContent.IsNullOrEmpty())
            {
                return sentence.ResourcePropertyName;
            }

            // 將參數塞回對應位置
            if (sentence.Args.AnyAndNotNull())
            {
                return string.Format(elementContent, sentence.Args.ToArray<object>());
            }

            return elementContent;
        }

        public static Dictionary<string, string> GetResource(ResourceInfo resourceInfo, PlatformCulture platformCulture)
        {
            return GetResource(resourceInfo.Directory, resourceInfo.ResourceName, platformCulture);
        }

        /// <summary>
        /// 針對語系取得對應的resource檔, 並轉為dictionary格式(方便前端使用)
        /// </summary>
        /// <param name="directory">Resource底下對應的資料夾名稱</param>
        /// <param name="resourceName">Resource檔名</param>
        /// <param name="platformCulture">語系</param>
        /// <returns></returns>
        public static Dictionary<string, string> GetResource(string directory, string resourceName, PlatformCulture platformCulture)
        {
            var key = $"{directory}.{resourceName}";
            if (platformCulture.Value != PlatformCulture.China.Value)
            {
                key += $".{platformCulture.Name}";
            }

            if (ResourceDictionaryMapper.ContainsKey(key))
            {
                return ResourceDictionaryMapper[key];
            }

            Dictionary<string, string> result = GetDictionaryByResource(platformCulture, key);
            ResourceDictionaryMapper.TryAdd(key, result);

            return result;
        }

        private static Dictionary<string, string> GetDictionaryByResource(PlatformCulture platformCulture, string key)
        {
            Assembly assembly = AppDomain.CurrentDomain.Load(CommonReferenceAssemblyName);
            if (platformCulture.Value != PlatformCulture.China.Value)
            {
                assembly = assembly.GetSatelliteAssembly(platformCulture.Culture);
            }

            var resourceDictionary = new Dictionary<string, string>();
            string resourceFullName = assembly.GetManifestResourceNames()
                .FirstOrDefault(n => n.IndexOf(key, StringComparison.OrdinalIgnoreCase) > -1);
            if (!string.IsNullOrEmpty(resourceFullName))
            {
                using (Stream stream = assembly.GetManifestResourceStream(resourceFullName))
                using (var resourceReader = new ResourceReader(stream))
                {
                    foreach (DictionaryEntry item in resourceReader)
                    {
                        resourceDictionary.Add(item.Key.ToString(), item.Value.ToString());
                    }
                }
            }

            return resourceDictionary;
        }

        private static string GetSplitOperator(string splitOperator)
        {
            if (splitOperator.IsNullOrEmpty())
            {
                return string.Empty;
            }

            return splitOperator;
        }

        private static ResourceManager GetCachedResourceManager(string fullAssemblyName)
        {
            // 預設先塞入 DBContentElement
            if (ResourceManagerMaps.ContainsKey(fullAssemblyName))
            {
                return ResourceManagerMaps[fullAssemblyName];
            }
            
            Type elementType = Type.GetType(fullAssemblyName);

            if (elementType == null)
            {
                // 利用namespace找出對應DLL中type
                string dllFileName = fullAssemblyName.Split('.')[0] + ".dll";

                string[] subFolderNames = { "\\", "\\bin\\" };
                string existAssemblyFilePath = null;

                foreach (string subFolderName in subFolderNames)
                {
                    string assemblyPath = AppDomain.CurrentDomain.BaseDirectory + subFolderName;
                    string assemblyFilePath = string.Concat(assemblyPath, dllFileName);

                    if (File.Exists(assemblyFilePath))
                    {
                        existAssemblyFilePath = assemblyFilePath;
                        break;                        
                    }
                }

                if (existAssemblyFilePath.IsNullOrEmpty())
                {
                    return null;
                }

                Assembly serviceAssmbly = Assembly.LoadFrom(existAssemblyFilePath);
                elementType = serviceAssmbly.GetTypes().Where(w => w.FullName == fullAssemblyName).FirstOrDefault();
            }

            if (elementType == null)
            {
                return null;
            }

            var resourceManager = new ResourceManager(elementType);
            ResourceManagerMaps.TryAdd(fullAssemblyName, resourceManager);

            return resourceManager;
        }
    }
}
