using JxBackendService.Common.Extensions;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.Util;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Util;
using JxBackendService.Resource.Element;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;

namespace JxBackendService.Common.Util
{
    public static class LocalizationUtil
    {
        private static readonly ConcurrentDictionary<string, ResourceManager> ResourceManagerMaps = new ConcurrentDictionary<string, ResourceManager>();

        private static readonly string _defaultNamespace = typeof(DBContentElement).FullName;

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
        public static string ToLocalizationContent(this string dbJsonString, string defaultMemo = null, JxApplication application = null)
        {
            if (dbJsonString.IsNullOrEmpty())
            {
                return defaultMemo.ToNonNullString();
            }

            LocalizationParam localizationParam = null;

            try
            {
                localizationParam = dbJsonString.Deserialize<LocalizationParam>();
            }
            catch (Exception ex)
            {
                var logUtilService = DependencyUtil.ResolveService<ILogUtilService>();
                logUtilService.Error(ex);
            }

            if (localizationParam == null)
            {
                return dbJsonString;
            }

            if (application != null)
            {
                localizationParam.LocalizationSentences.RemoveAll(ls => ls.Apps != null && !ls.Apps.Contains(application.AppCodeForLocalizationParam));
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

        public static LocalizationParam ToSingleSentenceLocalizationParam(this string resourcePropertyName, params object[] args)
        {
            return new LocalizationParam()
            {
                LocalizationSentences = new List<LocalizationSentence>()
                {
                    new LocalizationSentence()
                    {
                        ResourcePropertyName = resourcePropertyName,
                        Args = args.Select(a => a.ToString()).ToList()
                    }
                }
            };
        }

        public static LocalizationSentence ToSimpleLocalizationSentence(this string arg)
        {
            return nameof(DBContentElement.Flexible).ToLocalizationSentence(viewableApp: null, arg);
        }

        public static LocalizationSentence ToSimpleLocalizationSentence(this string arg, JxApplication viewableApp)
        {
            return nameof(DBContentElement.Flexible).ToLocalizationSentence(viewableApp, arg);
        }

        public static LocalizationSentence ToLocalizationSentence(this string resourcePropertyName, params string[] args)
        {
            return resourcePropertyName.ToLocalizationSentence(viewableApp: null, args);
        }

        public static LocalizationSentence ToLocalizationSentence(this string resourcePropertyName, JxApplication viewableApp, params string[] args)
        {
            List<int> viewableApps = null;

            if (viewableApp != null)
            {
                viewableApps = new List<int> { viewableApp.AppCodeForLocalizationParam };
            }

            return new LocalizationSentence()
            {
                ResourcePropertyName = resourcePropertyName,
                Args = args.ToList(),
                Apps = viewableApps
            };
        }

        public static LocalizationModel ToLocalizationModel(this LocalizationParam localizationParam)
        {
            return new LocalizationModel
            {
                Content = localizationParam.ToLocalizationContent(),
                JsonString = localizationParam.ToLocalizationJsonString()
            };
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

                string[] subFolderNames =
                {
                    Path.DirectorySeparatorChar.ToString(),
                    $"{Path.DirectorySeparatorChar}bin{Path.DirectorySeparatorChar}"
                };

                string existAssemblyFilePath = null;

                foreach (string subFolderName in subFolderNames)
                {
                    string assemblyPath = AppDomain.CurrentDomain.BaseDirectory.TrimEnd(Path.DirectorySeparatorChar) + subFolderName;
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