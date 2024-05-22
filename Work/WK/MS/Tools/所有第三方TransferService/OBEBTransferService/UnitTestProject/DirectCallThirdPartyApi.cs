using System;
using System.Collections.Generic;
using System.Net;
using JxBackendService.Common;
using JxBackendService.Common.Util;
using JxBackendService.Model.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject
{
    [TestClass]
    public class DirectCallThirdPartyApi
    {
        private static readonly string api = "https://uat-obeb.jxjxcdn.com";

        private static readonly string dataapi = "https://uat-obebdataapi.jxjxcdn.com";

        private static readonly string merchantCode = "S3HZP";

        private static readonly string AESKey = "FlTaWh30WuOHbEsg";

        private static readonly string MD5Key = "7U6QtrxO2twVT5ty";

        public DirectCallThirdPartyApi()
        {
        }

        [TestMethod]
        public void CreateOBEBUser()
        {
            string createurl = api + "/api/merchant/create/v2";
            string timestamp = DateTime.Now.ToUnixOfTime().ToString();

            object data = new
            {
                loginName = $"{merchantCode}jxD_googletest",
                loginPassword = $"{merchantCode}jxD_googletest",
                lang = 1,
                timestamp
            };

            // {"code":"200","message":"成功。","request":{"loginName":"S3HZPjxD_tttqqqq123","loginPassword":"","lang":1,"version":2,"timestamp":1661306136171},"data":{"create":"success"}}
            string result = GetApiResult(createurl, data);
        }

        [TestMethod]
        public void CreateforwardGame()
        {
            string createurl = api + "/api/merchant/forwardGame/v2";

            string timestamp = DateTime.Now.ToUnixOfTime().ToString();

            object data = new
            {
                loginName = $"{merchantCode}jxD_googletest00000",
                loginPassword = $"{merchantCode}jxD_googletest00000",
                deviceType = 1, // 1: 網頁版 2: 手機版網頁版 3: android 4: ios
                lang = 1,
                backurl = "http://wwww.google.com",
                domain = "",
                showExit = 0, //0=显示，1=不显示
                timestamp,
                anchorId = 20035, //20058, 水晶晶 // 20035, 紫若
            };

            //{"code":"200","message":"成功。","request":{"loginName":"S3HZPjxD_googletest00000","loginPassword":"","deviceType":2,"lang":1,"backurl":"http://wwww.google.com","domain":"","showExit":0,"version":2,"timestamp":1661330508223},"data":{"create":"success","deposit":"success","url":"https://ep.e99hw.com/?params=iRpLNr8OUxIL2RK7SK91GQF5UCGkpnhowsAJwt/lB6Ge1Lkt38a491DGu+fiykC9QfYoB2smkkTjTLNh4/wAne9pOVS6Hz2A36S3RKspqVES/mnYZ2CrtRKGAmBXA5KXq8LpkunJpeeOXw3lwSsQ/n4aWUSgLMbGlaOVXBvN+E6lgYUU0PdIC9FHSZIW1gR2pk6cpliAsEmZa5GPe2nt15oYHiMlHwYMZRMydyfZ7AnedUD19kRgIBe9M+wAelWdwy8rYiF5wtXzmBPffBhcW+WBhCdD+vZynCofyQBnIIMAqNMR44JISjwus52lgCq12bNnZxbHyGkCs5vX4aNyGeXp36ve9Sk5HuJBHGbQSJnr4+C1++CQB+DAbPJRjfT5uqHlSF296oxBg0w9jCvYkePlv0pRB+xyNBxTT0E6X9WMNGModCoAFIjWvZ2I+V8rWQmmhlhH6QImbrliILY75HBwYCIHkAJgj1qGsoFTEcoIyBjVjaECr1lOTIHBE2Le6o0SyblSUoGgzAgph0oJGLtCRW+Gw8XZhlmPdAZdDs1VUoiQS0xDNp76BRrO+3wueTZrsC7Ibr4q8b3UuwgmdlVxNWSSjehAMP9YcgrHqfpWdCSeXnsD3eMn5RGZIgrYzyN65L4j6ab3Uc37mH+1DsGbGH88PKZYQ4FxbvKTLrK69sslYgURVmMlRB7QE0u5rfg5jaLy0NTb9bnhtA9M7mxIpPpip1MymO5F1hmu5pidXGhmdia40VJmM7stDb/pvtHtTqPIrAbls3En4qqYmTzZlwTEwQOULAGbB/St7GsMQLoPQq3FJZk1OekSFHfvJRB65sOBX24Ou9bFGwY5/tJU91ALL0N4VmvpWKJRYkBIO48zY/dC7xtg/orrMpDSiWzFmSKkkSPXgFs9kGUDCNoEEcR4rApkPuBZSzIfT5pK0ABM0JB2i9HXmRxC5o+5XxQHfrY4Jo9ZZDMv8tA1bd6WTsdc42BpMKp7NkXJwx4032C1DNubbWTmMtY5t7G9MhaZZeI2Wo7EMVAgcgnaI85PH00Y95i48bDcjLsuI1yznIPMEj8PS43UO1SQXIZ56zGM8I2tUfdlDf5hvnijHEAsbrOIPYuBS+PGasDO2ELpwuYkUjAeONg8hRf09SMXEZzZ4VhhRCzdOEEPCotHMzQQ5O5yxNNKSHam6FAKbhcYGrKly3tZss0PmJiNSY3MaDRtsIMdhhf+YPc2GJSPwYT/hEX3cKI2d6Z1Gfoi9ttRlyenPOwfT7zs8GtxEensz9Ts8CbTkbszQ80QTB18YhCXABBUqQxh03LNQtoaVGR6YNZj7xik7bybig/WPVoYphMUWyG6ZJ73nszVUIDjvVqWxbZo94HhyW9PEjTtsue7eur5T+PGIimFAAX4qp/DJ9SYhaHkKEAlrXMtBt3ef8H/T6VTKh6ELjN+iwM/WO7Z+9cAfO6Jr6cLUspPG1BP&signature=10F6714B7BA18BB9AA5B39A72231C73A&ttl=1661330508632"}}
            string result = GetApiResult(createurl, data);
        }

        [TestMethod]
        public void CreatefastGame()
        {
            string createurl = api + "/api/merchant/fastGame/v2";

            string timestamp = DateTime.Now.ToUnixOfTime().ToString();
            string aa = "";

            object data = new
            {
                loginName = $"{merchantCode}jxD_googletest00000",
                loginPassword = $"{merchantCode}jxD_googletest00000",
                deviceType = 1, // 1: 網頁版 2: 手機版網頁版 3: android 4: ios
                lang = 1,
                backurl = "http://wwww.google.com",
                domain = "",
                showExit = 0, //0=显示，1=不显示
                timestamp,
                anchorId = aa,//20059, //20058, 水晶晶 // 20035, 紫若
                amount = 10,
                transferNo = timestamp
            };

            //{"code":"200","message":"成功。","request":{"loginName":"S3HZPjxD_googletest00000","loginPassword":"","deviceType":2,"lang":1,"backurl":"http://wwww.google.com","domain":"","showExit":0,"version":2,"timestamp":1661330508223},"data":{"create":"success","deposit":"success","url":"https://ep.e99hw.com/?params=iRpLNr8OUxIL2RK7SK91GQF5UCGkpnhowsAJwt/lB6Ge1Lkt38a491DGu+fiykC9QfYoB2smkkTjTLNh4/wAne9pOVS6Hz2A36S3RKspqVES/mnYZ2CrtRKGAmBXA5KXq8LpkunJpeeOXw3lwSsQ/n4aWUSgLMbGlaOVXBvN+E6lgYUU0PdIC9FHSZIW1gR2pk6cpliAsEmZa5GPe2nt15oYHiMlHwYMZRMydyfZ7AnedUD19kRgIBe9M+wAelWdwy8rYiF5wtXzmBPffBhcW+WBhCdD+vZynCofyQBnIIMAqNMR44JISjwus52lgCq12bNnZxbHyGkCs5vX4aNyGeXp36ve9Sk5HuJBHGbQSJnr4+C1++CQB+DAbPJRjfT5uqHlSF296oxBg0w9jCvYkePlv0pRB+xyNBxTT0E6X9WMNGModCoAFIjWvZ2I+V8rWQmmhlhH6QImbrliILY75HBwYCIHkAJgj1qGsoFTEcoIyBjVjaECr1lOTIHBE2Le6o0SyblSUoGgzAgph0oJGLtCRW+Gw8XZhlmPdAZdDs1VUoiQS0xDNp76BRrO+3wueTZrsC7Ibr4q8b3UuwgmdlVxNWSSjehAMP9YcgrHqfpWdCSeXnsD3eMn5RGZIgrYzyN65L4j6ab3Uc37mH+1DsGbGH88PKZYQ4FxbvKTLrK69sslYgURVmMlRB7QE0u5rfg5jaLy0NTb9bnhtA9M7mxIpPpip1MymO5F1hmu5pidXGhmdia40VJmM7stDb/pvtHtTqPIrAbls3En4qqYmTzZlwTEwQOULAGbB/St7GsMQLoPQq3FJZk1OekSFHfvJRB65sOBX24Ou9bFGwY5/tJU91ALL0N4VmvpWKJRYkBIO48zY/dC7xtg/orrMpDSiWzFmSKkkSPXgFs9kGUDCNoEEcR4rApkPuBZSzIfT5pK0ABM0JB2i9HXmRxC5o+5XxQHfrY4Jo9ZZDMv8tA1bd6WTsdc42BpMKp7NkXJwx4032C1DNubbWTmMtY5t7G9MhaZZeI2Wo7EMVAgcgnaI85PH00Y95i48bDcjLsuI1yznIPMEj8PS43UO1SQXIZ56zGM8I2tUfdlDf5hvnijHEAsbrOIPYuBS+PGasDO2ELpwuYkUjAeONg8hRf09SMXEZzZ4VhhRCzdOEEPCotHMzQQ5O5yxNNKSHam6FAKbhcYGrKly3tZss0PmJiNSY3MaDRtsIMdhhf+YPc2GJSPwYT/hEX3cKI2d6Z1Gfoi9ttRlyenPOwfT7zs8GtxEensz9Ts8CbTkbszQ80QTB18YhCXABBUqQxh03LNQtoaVGR6YNZj7xik7bybig/WPVoYphMUWyG6ZJ73nszVUIDjvVqWxbZo94HhyW9PEjTtsue7eur5T+PGIimFAAX4qp/DJ9SYhaHkKEAlrXMtBt3ef8H/T6VTKh6ELjN+iwM/WO7Z+9cAfO6Jr6cLUspPG1BP&signature=10F6714B7BA18BB9AA5B39A72231C73A&ttl=1661330508632"}}
            string result = GetApiResult(createurl, data);
        }

        [TestMethod]
        public void Getbalance()
        {
            string createurl = api + "/api/merchant/balance/v1";
            string timestamp = DateTime.Now.ToUnixOfTime().ToString();

            object data = new
            {
                loginName = $"{merchantCode}jxD_googletest",
                timestamp,
            };

            // {"code":"200","message":"成功。","request":{"loginName":"s3hzpjxd_googletest","timestamp":1661330898735},"data":{"balance":"110.000000"}}
            string result = GetApiResult(createurl, data);
        }

        [TestMethod]
        public void Deposit()
        {
            string createurl = api + "/api/merchant/deposit/v1";
            string timestamp = DateTime.Now.ToUnixOfTime().ToString();

            object data = new
            {
                loginName = $"{merchantCode}jxD_googletest",
                timestamp,
                transferNo = "202208241651000",
                amount = 5,
                showBalance = 1
            };

            // {"code":"200","message":"成功。","request":{"loginName":"s3hzpjxd_googletest","transferNo":"202208241651000","amount":5,"showBalance":1,"timestamp":1661331546099},"data":{"deposit":"deposit done","balance":"115.000000"}}
            string result = GetApiResult(createurl, data);
        }

        [TestMethod]
        public void Withdraw()
        {
            string createurl = api + "/api/merchant/withdraw/v1";
            string timestamp = DateTime.Now.ToUnixOfTime().ToString();

            object data = new
            {
                loginName = $"{merchantCode}jxD_googletest",
                timestamp,
                transferNo = "W202208241651000",
                amount = 5,
                showBalance = 1
            };

            // {"code":"200","message":"成功。","request":{"loginName":"s3hzpjxd_googletest","transferNo":"W202208241651000","amount":5,"showBalance":1,"timestamp":1661351405492},"data":{"withdraw":"withdraw done","balance":"110.000000"}}
            string result = GetApiResult(createurl, data);
        }

        [TestMethod]
        public void CheckTransfer()
        {
            string createurl = api + "/api/merchant/transfer/v1";
            string timestamp = DateTime.Now.ToUnixOfTime().ToString();

            object data = new
            {
                loginName = $"{merchantCode}jxD_googletest",
                timestamp,
                transferNo = "W202208241651000",
            };

            // {"code":"200","message":"成功。","request":{"transferNo":"W202208241651000","loginName":"s3hzpjxd_googletest","timestamp":1661351774639},"data":{"tradeNo":"W202208241651000","amount":-5.000000,"transferStatus":0}}
            string result = GetApiResult(createurl, data);
        }

        private string GetApiResult(string url, object obj)
        {
            string datajson = obj.ToJsonString();
            string parameters = AESTool.Encrypt(datajson, AESKey);
            string signature = MD5Tool.MD5EncodingForOBGameProvider(datajson + MD5Key).ToUpper();

            var keyValues = new Dictionary<string, string>
            {
                { "merchantCode",  merchantCode},
                { "params", parameters},
                { "signature", signature},
            };

            string param = keyValues.ToJsonString();

            string apiResult = HttpWebRequestUtil.GetResponse(
                new WebRequestParam()
                {
                    Purpose = $"TPGService.{PlatformProduct.OBEB.Value}請求",
                    Method = HttpMethod.Post,
                    Url = url,
                    Body = param,
                    ContentType = HttpWebRequestContentType.Json,
                    IsResponseValidJson = true,
                },
            out HttpStatusCode httpStatusCode);

            return apiResult;
        }

        [TestMethod]
        public void BetHistoryRecord()
        {
            string dataUrl = dataapi + "/data/merchant/betHistoryRecord/v1";
            string timestamp = DateTime.Now.ToUnixOfTime().ToString();

            object data = new
            {
                startTime = "2022-12-01 00:00:00",
                endTime = "2022-12-01 00:20:00",
                pageIndex = 1,
                timestamp
            };

            // {"code":"200","message":"成功.","request":{"startTime":"2022-08-24 22:50:00","endTime":"2022-08-24 23:20:00","pageIndex":1,"timestamp":1661353280720},"data":{"pageSize":1000,"pageIndex":1,"totalRecord":1,"totalPage":1,"record":[{"id":770436537922158592,"playerId":22899632,"playerName":"s3hzpjxd_googletest00000","agentId":5302,"betAmount":20.0000,"validBetAmount":19.0000,"netAmount":19.0000,"beforeAmount":100.0000,"createdAt":1661353185000,"netAt":1661353193000,"recalcuAt":0,"updatedAt":1661353193000,"gameTypeId":2001,"platformId":3,"platformName":"亚太厅","betStatus":1,"betFlag":0,"betPointId":3001,"odds":"0.95","judgeResult":"34:43;32:28","currency":"CNY","tableCode":"C01","roundNo":"GC0122824755","bootNo":"B0C012282401FP-GP296","loginIp":"61.220.213.91","deviceType":7,"deviceId":"1661330575363571459","recordType":4,"gameMode":1,"signature":"-","nickName":"jxd_googlete","dealerName":"Amirah1","tableName":"经典C01","addstr1":"庄:♥9♠J;闲:♦9♦8;","addstr2":"庄:9;闲:7;","agentCode":"S3HZP","agentName":"210503GOGDEV","betPointName":"庄","gameTypeName":"经典百家乐","payAmount":39.0000,"adddec1":1.0,"adddec2":0.0,"adddec3":0.0,"result":"9;7","startid":770436573455613953,"realDeductAmount":20.0000,"bettingRecordType":1}]}}
            string result = GetDataApiResult(dataUrl, data);
        }

        private string GetDataApiResult(string url, object obj)
        {
            string datajson = obj.ToJsonString();
            string parameters = AESTool.Encrypt(datajson, AESKey);
            string signature = MD5Tool.MD5EncodingForOBGameProvider(datajson + MD5Key).ToUpper();

            var keyValues = new Dictionary<string, string>
            {
                { "merchantCode",  merchantCode},
                { "params", parameters},
                { "signature", signature},
            };

            string param = keyValues.ToJsonString();

            var headers = new Dictionary<string, string>
            {
                { "merchantCode", merchantCode },
                { "pageIndex", "1" }
            };

            string apiResult = HttpWebRequestUtil.GetResponse(
                new WebRequestParam()
                {
                    Purpose = $"TPGService.{PlatformProduct.OBEB.Value}請求",
                    Method = HttpMethod.Post,
                    Url = url,
                    Body = param,
                    ContentType = HttpWebRequestContentType.Json,
                    Headers = headers,
                    IsResponseValidJson = true,
                },
            out HttpStatusCode httpStatusCode);

            return apiResult;
        }

        [TestMethod]
        public void GetLivesList()
        {
            string dataUrl = dataapi + "/data/merchant/lives/v2";
            string timestamp = DateTime.Now.ToUnixOfTime().ToString();

            object data = new
            {
                clientType = 1,
                ip = "127.0.0.1",
                pageIndex = 1,
                pageSize = 30,
                timestamp
            };

            // {"code":"200","message":"成功.","request":{"startTime":"2022-08-24 22:50:00","endTime":"2022-08-24 23:20:00","pageIndex":1,"timestamp":1661353280720},"data":{"pageSize":1000,"pageIndex":1,"totalRecord":1,"totalPage":1,"record":[{"id":770436537922158592,"playerId":22899632,"playerName":"s3hzpjxd_googletest00000","agentId":5302,"betAmount":20.0000,"validBetAmount":19.0000,"netAmount":19.0000,"beforeAmount":100.0000,"createdAt":1661353185000,"netAt":1661353193000,"recalcuAt":0,"updatedAt":1661353193000,"gameTypeId":2001,"platformId":3,"platformName":"亚太厅","betStatus":1,"betFlag":0,"betPointId":3001,"odds":"0.95","judgeResult":"34:43;32:28","currency":"CNY","tableCode":"C01","roundNo":"GC0122824755","bootNo":"B0C012282401FP-GP296","loginIp":"61.220.213.91","deviceType":7,"deviceId":"1661330575363571459","recordType":4,"gameMode":1,"signature":"-","nickName":"jxd_googlete","dealerName":"Amirah1","tableName":"经典C01","addstr1":"庄:♥9♠J;闲:♦9♦8;","addstr2":"庄:9;闲:7;","agentCode":"S3HZP","agentName":"210503GOGDEV","betPointName":"庄","gameTypeName":"经典百家乐","payAmount":39.0000,"adddec1":1.0,"adddec2":0.0,"adddec3":0.0,"result":"9;7","startid":770436573455613953,"realDeductAmount":20.0000,"bettingRecordType":1}]}}
            string result = GetDataApiResult(dataUrl, data);
        }
    }
}