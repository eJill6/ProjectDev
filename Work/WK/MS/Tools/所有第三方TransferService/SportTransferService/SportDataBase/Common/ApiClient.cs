using System;
using System.Collections.Generic;
using System.Runtime.Serialization.Json;
using System.IO;
using SportDataBase.Model;
using JxBackendService.Common.Util;

namespace SportDataBase.Common
{
    public interface IApiClient
    {
        ApiResult<FundTransferResult> CheckFundTransfer(string vendor_trans_id);
        ApiResult<List<UserBalanceItem>> CheckUserBalance(string vendor_member_ids);
        ApiResult<FundTransferResult> FundTransfer(string vendor_member_id, string vendor_trans_id, string amount, string direction, ref string apiResult);
        string GetBetDetailsResponse(string version_key);
        ApiResult<LeagueName> GetLeagueName(string league_id);
        ApiResult<List<BetSettingItem>> GetMemberBetSetting(string vendor_member_id);
        ApiResult<TeamName> GetTeamName(string team_id, string bet_type);
        ApiResult SetMemberBetSetting(string vendor_member_id, List<BetSettingItem> lstBetSetting);
    }

    public class ApiClient : IApiClient
    {
        private readonly string vendorId, url;

        public ApiClient()
        {
            vendorId = SportAppSettings.VendorID;
            url = SportAppSettings.URL;

            if (!url.EndsWith("/"))
            {
                url += "/";
            }
        }

        public ApiResult<FundTransferResult> FundTransfer(string vendor_member_id, string vendor_trans_id, string amount,
            string direction, ref string apiResult)
        {
            try
            {
                string currency = SportAppSettings.Currency.ToString();

                ParameterBuilder p = new ParameterBuilder();
                p.Add("vendor_id", vendorId)
                    .Add("vendor_member_id", vendor_member_id)
                    .Add("vendor_trans_id", vendor_trans_id)
                    .Add("amount", amount.ToString())
                    .Add("currency", currency.ToString())
                    .Add("direction", direction.ToString());

                string json = HttpHelper.HttpPost(url + "FundTransfer", p.ToString());
                apiResult = json;

                if (json.IsNullOrEmpty())
                {
                    return null;
                }

                return (ApiResult<FundTransferResult>)json2obj(json, typeof(ApiResult<FundTransferResult>));
            }
            catch (Exception ex)
            {
                LogsManager.Error("FundTransfer失败：" + ex.Message + ",堆栈：" + ex.StackTrace);
            }
            return new ApiResult<FundTransferResult>() { error_code = -1 };
        }

        /// <summary> 为确认资金转帐状态 </summary>
        public virtual ApiResult<FundTransferResult> CheckFundTransfer(string vendor_trans_id)
        {
            ParameterBuilder p = new ParameterBuilder();
            p.Add("vendor_id", vendorId)
             .Add("vendor_trans_id", vendor_trans_id);

            string json = HttpHelper.HttpPost(url + "CheckFundTransfer", p.ToString());

            if (json.IsNullOrEmpty())
            {
                return null;
            }

            return (ApiResult<FundTransferResult>)json2obj(json, typeof(ApiResult<FundTransferResult>));
        }

        public string GetBetDetailsResponse(string version_key)
        {
            ParameterBuilder param = new ParameterBuilder();
            param.Add("vendor_id", vendorId)
                 .Add("version_key", version_key)
                 .Add("options", "");

            string json = HttpHelper.HttpPost(url + "GetBetDetail", param.ToString());

            return json;
        }

        public ApiResult<LeagueName> GetLeagueName(string league_id)
        {
            ParameterBuilder p = new ParameterBuilder();
            p.Add("vendor_id", vendorId)
                .Add("league_id", league_id);

            string json = HttpHelper.HttpPost(url + "GetLeagueName", p.ToString());

            return (ApiResult<LeagueName>)json2obj(json, typeof(ApiResult<LeagueName>));
        }

        public ApiResult<TeamName> GetTeamName(string team_id, string bet_type)
        {
            ParameterBuilder p = new ParameterBuilder();
            p.Add("vendor_id", vendorId)
                .Add("team_id", team_id)
                .Add("bet_type", bet_type);

            string json = HttpHelper.HttpPost(url + "GetTeamName", p.ToString());

            return (ApiResult<TeamName>)json2obj(json, typeof(ApiResult<TeamName>));
        }

        public ApiResult<List<UserBalanceItem>> CheckUserBalance(string vendor_member_ids)
        {
            ParameterBuilder p = new ParameterBuilder();
            p.Add("vendor_id", vendorId)
                .Add("vendor_member_ids", vendor_member_ids);

            string json = HttpHelper.HttpPost(url + "CheckUserBalance", p.ToString());

            if (json.IsNullOrEmpty())
            {
                return null;
            }

            return (ApiResult<List<UserBalanceItem>>)json2obj(json, typeof(ApiResult<List<UserBalanceItem>>)); ;
        }

        public ApiResult<List<BetSettingItem>> GetMemberBetSetting(string vendor_member_id)
        {
            ParameterBuilder p = new ParameterBuilder();
            p.Add("vendor_id", vendorId)
                .Add("vendor_member_id", vendor_member_id);

            string json = HttpHelper.HttpPost(url + "GetMemberBetSetting", p.ToString());

            if (json.IsNullOrEmpty())
            {
                return null;
            }

            return (ApiResult<List<BetSettingItem>>)json2obj(json, typeof(ApiResult<List<BetSettingItem>>));
        }

        public ApiResult SetMemberBetSetting(string vendor_member_id, List<BetSettingItem> lstBetSetting)
        {
            ParameterBuilder p = new ParameterBuilder();
            p.Add("vendor_id", vendorId)
                .Add("vendor_member_id", vendor_member_id)
                .Add("bet_setting", obj2json(lstBetSetting));

            string json = HttpHelper.HttpPost(url + "SetMemberBetSetting", p.ToString());

            if (json.IsNullOrEmpty())
            {
                return null;
            }

            return (ApiResult)json2obj(json, typeof(ApiResult));
        }

        #region json Serialzer

        public static string obj2json(object obj)
        {
            if (obj == null) return "null";
            var dcs = new DataContractJsonSerializer(obj.GetType());
            using (var stream = new MemoryStream())
            {
                dcs.WriteObject(stream, obj);
                stream.Position = 0;
                return System.Text.Encoding.UTF8.GetString(stream.ToArray());
            }
        }

        public static object json2obj(string json, Type type)
        {
            try
            {
                var dcs = new DataContractJsonSerializer(type);
                using (var stream = new MemoryStream())
                {
                    byte[] bytes = System.Text.Encoding.UTF8.GetBytes(json);
                    stream.Write(bytes, 0, bytes.Length);
                    stream.Position = 0;
                    return dcs.ReadObject(stream);
                }
            }
            catch (Exception ex)
            {
                LogsManager.Info("反序列化失败：" + ex.Message + ",堆栈：" + ex.StackTrace);
                return null;
            }
        }

        #endregion
    }
}
