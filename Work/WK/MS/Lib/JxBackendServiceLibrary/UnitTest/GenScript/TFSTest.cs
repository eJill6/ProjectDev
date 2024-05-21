using JxBackendService.Model.Enums;
using JxBackendService.Service.Setting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace UnitTest.GenScript
{
    [TestClass]
    public class TFSTest
    {
        private static readonly HashSet<JxApplication> s_excludeApps = new HashSet<JxApplication>()
        {
            JxApplication.RGTransferService,
            JxApplication.AWCSPTransferService,
            JxApplication.FYESTransferService,
        };

        private static readonly JxApplication[] s_apps = JxApplication.GetAll()
            .Where(w => (w.AppSettingServiceType == typeof(OldTransferServiceAppSettingService) || w.AppSettingServiceType == typeof(NewTransferServiceAppSettingService))
            && !s_excludeApps.Contains(w)).ToArray();

        [TestMethod]
        public void GenAllTransferServiceCards()
        {
            var fileContent = new StringBuilder();
            const string featureName = "優化MethodInvocationLog利用Queue、批次寫入DB並增加開關";

            foreach (JxApplication jxApplication in s_apps)
            {
                string title = $"[{jxApplication.Value}] {featureName}";
                string content = $@"更新說明：
{title}

更新步驟：

1、 停止 {jxApplication.Value}服務
2、 設定 appsettings.json Default 結構內新增 KEY/VAULE

    ""IsEnabledMethodInvocationLog"": ""0""

3、 GIT PULL  最新檔案
4、 使用本次更新檔進行複製到目標目錄
5、 啟動服務
========================================================
退版動作 :

1、  停止服務
2、  使用備份「{jxApplication.Value}」目錄進行覆蓋
3、  啟動服務
";
                fileContent.AppendLine(CreateScript(title, content));
            }

            fileContent.Insert(0, "var steps = [];" + Environment.NewLine);

            fileContent.AppendLine("$.each(steps,function(i,fn){");
            fileContent.AppendLine("    setTimeout(fn, i * 2000); ");
            fileContent.AppendLine("}); ");

            File.WriteAllText("createCard.js", fileContent.ToString());
        }

        private string CreateScript(string title, string content)
        {
            string tfsFormatContent = string.Join(string.Empty, Regex.Split(content, Environment.NewLine)
                .Select(s => $"<div>{s.Replace("<", "&lt;").Replace(">", "&gt;")}</div>"));

            StringBuilder js = new StringBuilder();

            js.AppendLine("steps.push(()=>{	");
            js.AppendLine("    window.$addCard = $('.board-add-card');");
            js.AppendLine("    $addCard.click();");
            js.AppendLine("});");

            js.AppendLine("steps.push(()=>{");
            js.AppendLine($"    $addCard.parent().next().next().find('textarea').val('{title}'); ");
            js.AppendLine("}); ");

            js.AppendLine("steps.push(()=>{");
            js.AppendLine("    $addCard.click(); ");
            js.AppendLine("}); ");

            //打開卡片
            js.AppendLine("steps.push(()=>{");
            js.AppendLine($"    $(\".clickable-title:contains('{title}'):first()\").click(); ");
            js.AppendLine("});");

            //填入描述
            js.AppendLine("steps.push(()=>{");
            js.AppendLine("    var $descIframe = $('.richeditor-editarea:first iframe');");
            js.AppendLine($"    $('body', $descIframe.contents()).html('{tfsFormatContent}');");
            js.AppendLine("    var $title = $(\"input[placeholder='輸入標題']\");");
            js.AppendLine("    var title = $title.val();");
            js.AppendLine("    $title.val(title + '.').change();");
            js.AppendLine("});");

            //指定運維(jeff)
            js.AppendLine("steps.push(() =>{");
            js.AppendLine("$('.identity-picker-container span').click();");
            js.AppendLine("});");

            js.AppendLine("steps.push(() =>{");
            js.AppendLine("$(\".element-height-large span:contains('jeff.kuo')\").click();");
            js.AppendLine("});");

            //打上tag
            js.AppendLine("steps.push(()=>{");
            js.AppendLine("    $('.tags-add-button .tag-box').click();");
            js.AppendLine("    $('.tags-input').val('平台');");
            js.AppendLine("    $(\"span:contains('儲存'):eq(1)\").click();");
            js.AppendLine("});");

            //js.AppendLine("steps.push(()=>{");
            //js.AppendLine("    $(\"span:contains('儲存'):eq(1)\").click();");
            //js.AppendLine("}); ");

            js.AppendLine("steps.push(()=>{");
            js.AppendLine("    $(\"button[title='關閉']:first\").click();");
            js.AppendLine("}); ");

            return js.ToString();
        }
    }
}