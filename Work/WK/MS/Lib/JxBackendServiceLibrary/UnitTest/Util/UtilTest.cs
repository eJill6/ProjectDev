using JxBackendService.Common;
using JxBackendService.Common.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace UnitTest.Util
{
    [TestClass]
    public class UtilTest
    {
        [TestMethod]
        public void FTPTest()
        {
            FtpUtil.DownloadFile(new JxBackendService.Model.Ftp.FtpLoginParam()
            {
                FtpAddress = "127.0.0.1",
                FtpUser = "tpUser",
                FtpPassword = "123456",
                FtpPort = 21
            }, "c:/temp/201907010002.xml", "/AG/AGINLostAndFoundXMLFileDir/201907010002.xml");
        }

        [TestMethod]
        public void LogonTest()
        {
            int userId = 2023061702;

            string url = $"http://192.168.104.70/?userId={userId}" +
                $"&userName=%E5%A4%A7%E8%A1%A3%E5%93%A5&roomNo=10093&gameID=65&depositUrl=https%3A%2F%2Fwww.baidu.com%3Ftoken%3Dxxxxxx&clientWebPageValue=GameCenter&logonMode=1";

            int concurrentRequests = 100;

            // 建立 HttpClient 實例
            HttpClient httpClient = new HttpClient();

            // 建立一個 List 來儲存每個併發任務的工作
            List<Task<HttpResponseMessage>> tasks = new List<Task<HttpResponseMessage>>();

            // 同時發出多個請求
            for (int i = 0; i < concurrentRequests; i++)
            {
                tasks.Add(httpClient.GetAsync(url));
            }

            // 等待所有請求完成
            Task.WhenAll(tasks);

            // 處理每個併發任務的結果
            foreach (var task in tasks)
            {
                HttpResponseMessage response = task.Result;
                // 這裡可以處理每個請求的回應
                Console.WriteLine($"Status code: {response.StatusCode}");
            }

            // 結束 HttpClient 的使用
            httpClient.Dispose();
        }
    }
}