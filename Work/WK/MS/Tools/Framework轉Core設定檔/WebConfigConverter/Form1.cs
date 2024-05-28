using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace WebConfigConverter
{
    public partial class Form1 : Form
    {
        private static readonly HashSet<string> s_ignoreKeys = new HashSet<string>()
        {
            "IsWorkAgTransferProfitLoss", "IsWorkAgTransferLostAndFoundProfitLoss","IsWorkClearExpiredAgProfitLoss",
            "IsWorkRepaireAgAvailableScores","IsWorkRefreshAgAvailableScores", "ClientSettingsProvider.ServiceUri"
        };

        public Form1()
        {
            InitializeComponent();
        }

        private void btnLoadConfig_Click(object sender, EventArgs e)
        {
            XDocument doc = GetXDocumentFromConfigFile();

            if (doc == null)
            {
                return;
            }

            XElement appSettings = doc.Descendants("appSettings").Single();

            IGrouping<string, XElement> removeItem = appSettings.Elements("add").GroupBy(g => g.Attribute("key").Value).Where(w => w.Count() > 1).FirstOrDefault();

            if (removeItem != null)
            {
                throw new Exception($"key {removeItem.Key} 重複");
            }

            // 将 appSettings 转换为字典
            var appSettingsDict = appSettings.Elements("add").Where(w => !s_ignoreKeys.Contains(w.Attribute("key").Value))
                .ToDictionary(k => k.Attribute("key").Value, v => v.Attribute("value").Value);

            var newAppSetting = new NewAppSetting()
            {
                Logging = new Logging(),
                Default = appSettingsDict
            };

            // 将字典转换为 JSON 字符串
            string json = JsonConvert.SerializeObject(newAppSetting, Newtonsoft.Json.Formatting.Indented);
            txtParseResult.Text = json;

            SaveJsonFile(json);
        }

        private XDocument GetXDocumentFromConfigFile()
        {
            var openFileDialog = new OpenFileDialog();

            openFileDialog.Title = "选择文件";
            openFileDialog.Filter = "*.config|*.config";

            // 打开文件对话框并获取用户选择的文件
            DialogResult result = openFileDialog.ShowDialog();

            if (result != DialogResult.OK)
            {
                return null;
            }

            string configFilePath = openFileDialog.FileName;
            XDocument doc = XDocument.Load(configFilePath);

            return doc;
        }

        private void SaveJsonFile(string json)
        {
            var saveFileDialog = new SaveFileDialog()
            {
                Filter = "JSON 文件 (*.json)|*.json",
                FileName = "appsettings.json"
            };

            if (saveFileDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            string fileName = saveFileDialog.FileName;
            File.WriteAllText(fileName, json, Encoding.UTF8);
            string directoryPath = Path.GetDirectoryName(fileName);
            Process.Start("explorer.exe", directoryPath);
        }
    }

    public class NewAppSetting
    {
        public Logging Logging { get; set; }

        public Dictionary<string, string> Default { get; set; }
    }

    public class Logging
    {
        public Dictionary<string, string> LogLevel { get; set; } = new Dictionary<string, string>()

        {
                { "Default", "Information" },
                { "Microsoft.Hosting.Lifetime", "Information"}
        };
    }
}