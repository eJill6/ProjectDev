using BatchService.Job.Base;
using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.MiseLive;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ReturnModel;

namespace BatchService.Job
{
    public class CrawlOBEBAnchorListJob : BaseBatchServiceQuartzJob
    {
        private readonly Lazy<ITPLiveStreamService> _anchorService;

        public CrawlOBEBAnchorListJob()
        {
            _anchorService = DependencyUtil.ResolveJxBackendService<ITPLiveStreamService>(EnvUser, DbConnectionTypes.Master);
        }

        public override void DoJob()
        {
            BaseReturnModel crawlResult = _anchorService.Value.CrawlAnchors();

            if (!crawlResult.IsSuccess)
            {
                LogUtilService.ForcedDebug(crawlResult.ToJsonString());
            }
        }

        //private static string GetLivesList()
        //{
        //    string dataUrl = dataapi + "/data/merchant/lives/v2";
        //    string timestamp = DateTime.Now.ToUnixOfTime().ToString();

        //    object data = new
        //    {
        //        clientType = 1,
        //        ip = "127.0.0.1",
        //        timestamp
        //    };

        //    // {"code":"200","message":"成功.","request":{"startTime":"2022-08-24 22:50:00","endTime":"2022-08-24 23:20:00","pageIndex":1,"timestamp":1661353280720},"data":{"pageSize":1000,"pageIndex":1,"totalRecord":1,"totalPage":1,"record":[{"id":770436537922158592,"playerId":22899632,"playerName":"s3hzpjxd_googletest00000","agentId":5302,"betAmount":20.0000,"validBetAmount":19.0000,"netAmount":19.0000,"beforeAmount":100.0000,"createdAt":1661353185000,"netAt":1661353193000,"recalcuAt":0,"updatedAt":1661353193000,"gameTypeId":2001,"platformId":3,"platformName":"亚太厅","betStatus":1,"betFlag":0,"betPointId":3001,"odds":"0.95","judgeResult":"34:43;32:28","currency":"CNY","tableCode":"C01","roundNo":"GC0122824755","bootNo":"B0C012282401FP-GP296","loginIp":"61.220.213.91","deviceType":7,"deviceId":"1661330575363571459","recordType":4,"gameMode":1,"signature":"-","nickName":"jxd_googlete","dealerName":"Amirah1","tableName":"经典C01","addstr1":"庄:♥9♠J;闲:♦9♦8;","addstr2":"庄:9;闲:7;","agentCode":"S3HZP","agentName":"210503GOGDEV","betPointName":"庄","gameTypeName":"经典百家乐","payAmount":39.0000,"adddec1":1.0,"adddec2":0.0,"adddec3":0.0,"result":"9;7","startid":770436573455613953,"realDeductAmount":20.0000,"bettingRecordType":1}]}}
        //    return GetDataApiResult(dataUrl, data);
        //}

        //private static string GetDataApiResult(string url, object obj)
        //{
        //    try
        //    {
        //        string datajson = obj.ToJsonString();
        //        string parameters = AESTool.Encrypt(datajson, AESKey);
        //        string signature = MD5Tool.MD5EncodingForOBGameProvider(datajson + MD5Key).ToUpper();

        //        var keyValues = new Dictionary<string, string>
        //    {
        //        { "merchantCode",  merchantCode},
        //        { "params", parameters},
        //        { "signature", signature},
        //    };

        //        string param = keyValues.ToJsonString();

        //        var headers = new Dictionary<string, string>
        //    {
        //        { "merchantCode", merchantCode },
        //        { "pageIndex", "1" }
        //    };

        //        string apiResult = HttpWebRequestUtil.GetResponse(
        //            new WebRequestParam()
        //            {
        //                Purpose = $"TPGService.{PlatformProduct.OBEB.Value}請求",
        //                Method = HttpMethod.Post,
        //                Url = url,
        //                Body = param,
        //                ContentType = HttpWebRequestContentType.Json,
        //                Headers = headers,
        //                IsResponseValidJson = true,
        //            },
        //        out HttpStatusCode httpStatusCode);
        //        return apiResult;
        //    }
        //    catch (Exception ex)
        //    {
        //        LogUtil.Error("GetDataApiResult失敗，原因:" + ex.ToString());
        //        return string.Empty;
        //    }
        //}
    }
}