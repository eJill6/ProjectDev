using JxBackendService.Common;
using JxBackendService.Common.Util;
using JxBackendService.Model.Enums;
using JxBackendService.Service.Setting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace UnitTest.GenScript
{
    [TestClass]
    public class TFSTest
    {
        private static readonly JxApplication[] s_apps = new JxApplication[]
        {
            JxApplication.AGTransferService,
            JxApplication.OBEBTransferService,
            JxApplication.IMKYTransferService,
            JxApplication.IMPPTransferService,
            JxApplication.IMSGTransferService,
            JxApplication.IMSportTransferService,
            JxApplication.IMVRTransferService,
            JxApplication.LCTransferService,
            JxApplication.OBSPTransferService,
            JxApplication.PGSLTransferService,
            JxApplication.IMeBetTransferService,
            JxApplication.FYESTransferService,
            JxApplication.WLBGTransferService,
            JxApplication.JDBFITransferService
        };

        [TestMethod]
        public void GenAllTransferServiceCards()
        {
            var fileContent = new StringBuilder();

            foreach (JxApplication jxApplication in JxApplication.GetAll().Where(w => s_apps.Contains(w)))
            {
                string title = $"[{jxApplication.Value}] 投注紀錄上傳oss";
                string content = $@"更新說明：
[{jxApplication.Value}] 投注紀錄上傳oss

更新步驟：

1、 停止 {jxApplication.Value}服務
2、 GIT PULL  最新檔案
3、 使用本次更新檔進行複製到目標目錄
4、 TODO 設定CONFIG
------UAT---------------------------------------
<!--阿里雲OSS設定開始-->
<add key=""AliYun.AccessKeyId"" value=""LTAI5tRfDdpUFhev347bJBJj"" />
<add key=""AliYun.AccessKeySecret"" value=""uKTIRWFiqsIQiZUY62tmPNoun1UoVi"" />
<add key=""AliYun.Endpoint"" value=""oss-cn-hongkong.aliyuncs.com"" />
<add key=""AliYun.CoreBucketName"" value=""core-uat"" />
<!--阿里雲OSS設定結束-->

------LIVE---------------------------------------
<!--阿里雲OSS設定開始-->
<add key=""AliYun.AccessKeyId"" value=""LTAI5tRfDdpUFhev347bJBJj"" />
<add key=""AliYun.AccessKeySecret"" value=""uKTIRWFiqsIQiZUY62tmPNoun1UoVi"" />
<add key=""AliYun.Endpoint"" value=""oss-cn-hongkong.aliyuncs.com"" />
<add key=""AliYun.CoreBucketName"" value=""core-live"" />
<!--阿里雲OSS設定結束-->

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

            js.AppendLine("steps.push(()=>{");
            js.AppendLine($"    $(\".clickable-title:contains('{title}'):first()\").click(); ");
            js.AppendLine("});");

            js.AppendLine("steps.push(()=>{");
            js.AppendLine("    var $descIframe = $('.richeditor-editarea:first iframe');");
            js.AppendLine($"    $('body', $descIframe.contents()).html('{tfsFormatContent}');");
            js.AppendLine("    var $title = $(\"input[placeholder='輸入標題']\");");
            js.AppendLine("    var title = $title.val();");
            js.AppendLine("    $title.val(title + '.').change();");
            js.AppendLine("});");

            js.AppendLine("steps.push(()=>{");
            js.AppendLine("    $(\"span:contains('儲存'):eq(1)\").click();");
            js.AppendLine("}); ");

            js.AppendLine("steps.push(()=>{");
            js.AppendLine("    $(\"span:contains('關閉'):first\").click(); ");
            js.AppendLine("}); ");

            return js.ToString();
        }
    }
}