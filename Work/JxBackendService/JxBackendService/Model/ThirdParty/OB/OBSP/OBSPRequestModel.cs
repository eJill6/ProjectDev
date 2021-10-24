using JxBackendService.Common;
using JxBackendService.Common.Extensions;
using JxBackendService.Common.Util;
using JxBackendService.Model.Enums;
using System;
using System.Collections.Specialized;

namespace JxBackendService.Model.ThirdParty.OB.OBSP
{
    public abstract class OBSPBaseRequest
    {
        public string MerchantCode { get; set; }

        public string Key { get; set; }

        public readonly string Timestamp = DateTime.Now.ToUnixOfTime().ToString();

        public abstract NameValueCollection GetRequestNameValueCollection();

        public abstract string[] SignaturePropertyValues { get; }

        public string Signature
        {
            get
            {
                //20210628 跟OB體育RD確認過，兩段MD5之後都需要轉小寫，否則會出現驗證簽名失敗
                string firstPart = string.Join("&", SignaturePropertyValues);
                string firstPartHash = MD5Tool.MD5EncodingForOBGameProvider(firstPart);
                string secondPart = firstPartHash + "&" + Key;
                string secondPartHash = MD5Tool.MD5EncodingForOBGameProvider(secondPart);
                return secondPartHash;
            }
        }

        public string ToRequestBody() => GetRequestNameValueCollection().ToString();

        protected NameValueCollection CreateMerchantNameValueCollection()
        {
            NameValueCollection postParams = System.Web.HttpUtility.ParseQueryString(string.Empty);
            postParams.Add("merchantCode", MerchantCode);
            postParams.Add("timestamp", Timestamp);
            postParams.Add("signature", Signature);

            return postParams;
        }

    }

    public class OBSPUserBasicRequest : OBSPBaseRequest
    {
        public string UserName { get; set; }

        public override string[] SignaturePropertyValues => new string[] { MerchantCode, UserName, Timestamp };

        public override NameValueCollection GetRequestNameValueCollection()
        {
            NameValueCollection postParams = CreateMerchantNameValueCollection();
            postParams.Add("userName", UserName);

            return postParams;
        }
    }

    public class OBSPCreateUserRequest : OBSPUserBasicRequest
    {
        public string Currency { get; set; }

        public override string[] SignaturePropertyValues => new string[] { UserName, MerchantCode, Timestamp };

        public override NameValueCollection GetRequestNameValueCollection()
        {
            NameValueCollection nameValueCollection = base.GetRequestNameValueCollection();
            nameValueCollection.Add("currency", Currency);

            return nameValueCollection;
        }
    }

    public class OBSPLoginUserRequest : OBSPCreateUserRequest
    {
        /// <summary>
        /// 终端类型, 为空或"pc"为电脑端,"mobile"为手机端
        /// </summary>
        public string Terminal { get; set; }

        public override string[] SignaturePropertyValues => new string[] { MerchantCode, UserName, Terminal, Timestamp };

        public override NameValueCollection GetRequestNameValueCollection()
        {
            NameValueCollection nameValueCollection = base.GetRequestNameValueCollection();
            nameValueCollection.Add("terminal", Terminal);

            return nameValueCollection;
        }
    }

    public class OBSPGetTransferRecordRequest : OBSPUserBasicRequest
    {
        public string TransferId { get; set; }

        public override string[] SignaturePropertyValues => new string[] { MerchantCode, TransferId, Timestamp };


        public override NameValueCollection GetRequestNameValueCollection()
        {
            NameValueCollection nameValueCollection = base.GetRequestNameValueCollection();
            nameValueCollection.Add("transferId", TransferId);

            return nameValueCollection;
        }
    }

    public class OBSPCreateTransferRequest : OBSPGetTransferRecordRequest
    {
        public OBSPTransferType TransferType { get; set; }

        /// <summary>
        /// 金額,至小數2位
        /// </summary>
        public string Amount { get; set; }

        public override string[] SignaturePropertyValues => new string[] {
            MerchantCode,
            UserName,
            TransferType.Value,
            Amount,
            TransferId,
            Timestamp };

        public override NameValueCollection GetRequestNameValueCollection()
        {
            NameValueCollection nameValueCollection = base.GetRequestNameValueCollection();
            nameValueCollection.Add("transferType", TransferType.Value);
            nameValueCollection.Add("amount", Amount);

            return nameValueCollection;
        }
    }

    public class OBSPGQueryBetListRequest : OBSPBaseRequest
    {
        public string StartTime { get; set; }

        public string EndTime { get; set; }

        public int PageNum { get; set; }

        public int PageSize { get; set; }

        public OBSPBetListOrderBy OrderBy => OBSPBetListOrderBy.UpdateTime;

        public override string[] SignaturePropertyValues => new string[] { MerchantCode, StartTime, EndTime, Timestamp };

        public override NameValueCollection GetRequestNameValueCollection()
        {
            NameValueCollection nameValueCollection = CreateMerchantNameValueCollection();
            nameValueCollection.Add("startTime", StartTime);
            nameValueCollection.Add("endTime", EndTime);
            nameValueCollection.Add("pageNum", PageNum.ToString());
            nameValueCollection.Add("pageSize", PageSize.ToString());
            nameValueCollection.Add("orderBy", ((int)OrderBy).ToString());
            
            return nameValueCollection;
        }
    }

    public enum OBSPBetListOrderBy
    {
        BetTime = 1,
        UpdateTime = 2
    }

    public class OBSPTransferType : BaseStringValueModel<OBSPTransferType>
    {
        private OBSPTransferType() { }

        public static OBSPTransferType Deposit = new OBSPTransferType { Value = "1" };

        public static OBSPTransferType Withdraw = new OBSPTransferType { Value = "2" };
    }

}
