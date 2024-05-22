using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text;

namespace IMBGDataBase.Common
{
    public class SaveFile
    {
        readonly string settingLocationWriteTo = "SettingLocationWriteTo";
        readonly string locationWriteTo = string.Empty;
        private List<DirectoryInfo> directoryArray = new List<DirectoryInfo>();
        readonly string content;
        readonly string fileName;

        public SaveFile(string content, string fileName)
        {
            this.content = content;
            this.fileName = fileName;
            locationWriteTo = ConfigurationManager.AppSettings[settingLocationWriteTo].Trim();
        }

        public string LocationWriteTo
        {
            get
            {
                return locationWriteTo;
            }
        }

        public void LoadLocationWriteTo()
        {
            string[] locationArray = null;
            try
            {
                locationArray = locationWriteTo.Split(';');
            }
            catch (Exception ex)
            {
                LogsManager.InfoToTelegram(ex);
            }

            if (locationArray == null || locationArray.Length == 0)
            {
                LogsManager.InfoToTelegram($"讀取設定 {nameof(settingLocationWriteTo)} 失敗或無設定");
                return;
            }

            if (locationArray.Length == 1 && string.IsNullOrEmpty(locationArray[0]))
            {
                LogsManager.InfoToTelegram($"{nameof(settingLocationWriteTo)} 為empty 清除copyto目標");
                directoryArray.Clear();
                return;
            }

            DirectoryInfo[] temp = new DirectoryInfo[directoryArray.Count];
            directoryArray.CopyTo(temp);
            directoryArray.Clear();
            for (int i = 0; i < locationArray.Length; i++)
            {
                LogsManager.Info($"開始轉換{nameof(settingLocationWriteTo)}設定 {locationArray[i]} by {nameof(LoadLocationWriteTo)}，轉換為DirectoryInfo");

                try
                {
                    DirectoryInfo directoryInfo = new DirectoryInfo(locationArray[i]);
                    directoryArray.Add(directoryInfo);
                }
                catch (Exception ex)
                {
                    LogsManager.InfoToTelegram($"轉換{nameof(settingLocationWriteTo)}設定 {locationArray[i]}，轉換為DirectoryInfo Error :{ex}");
                }
            }
        }

        /// <summary>
        /// 建立寫文件的目標位置目錄
        /// </summary>
        public void DirectoryForLocationWriteToNotExistThenCreate()
        {
            foreach (DirectoryInfo info in directoryArray.ToArray())
            {
                try
                {
                    if (false == info.Exists)
                    {
                        info.Create();
                    }
                }
                catch (Exception ex)
                {
                    LogsManager.Info($"無法建立目錄 {info.FullName}");
                    LogsManager.InfoToTelegram(ex);
                }
            }
        }

        public bool IsFileExist(string targetFolder)
        {
            string filePath = Path.Combine(targetFolder, GetFileName());
            return (File.Exists(filePath));
        }

        public string GetNewFileName()
        {
            return $"{fileName}-{Guid.NewGuid()}.json";
        }

        public string GetFileName()
        {
            return $"{fileName}.json";
        }

        /// <summary>
        /// 寫檔
        /// </summary>
        public void WriteRemoteContentToOtherPlace()
        {
            string destinitionFilePath = string.Empty;
            string fileName = string.Empty;
            bool isOverWrite = false;

            foreach (DirectoryInfo info in directoryArray.ToArray())
            {
                try
                {
                    if (IsFileExist(info.FullName))
                    {
                        fileName = GetNewFileName();
                    }
                    else
                    {
                        fileName = GetFileName();
                    }

                    destinitionFilePath = Path.Combine(info.FullName, fileName);

                    LogsManager.Info($"{nameof(WriteRemoteContentToOtherPlace)} to {destinitionFilePath}，存在則複寫:{isOverWrite}");

                    using (StreamWriter sw = new StreamWriter(destinitionFilePath, isOverWrite, Encoding.UTF8))
                    {
                        sw.Write(content);
                    }
                }
                catch (Exception ex)
                {
                    LogsManager.InfoToTelegram($"{nameof(WriteRemoteContentToOtherPlace)} Fail; Exception:{ex}");
                }
            }
        }
    }
}
