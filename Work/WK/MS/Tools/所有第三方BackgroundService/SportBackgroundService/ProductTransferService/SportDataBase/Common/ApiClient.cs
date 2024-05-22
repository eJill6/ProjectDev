using System.Runtime.Serialization.Json;
using ProductTransferService.SportDataBase.Model;
using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.Util;

namespace ProductTransferService.SportDataBase.Common
{
    public interface IApiClient
    {
        ApiResult<LeagueName> GetLeagueName(string league_id);

        ApiResult<TeamName> GetTeamName(string team_id, string bet_type);
    }

    public class ApiClient : IApiClient
    {
        private readonly SportAppSetting _sportAppSetting;

        private readonly string vendorId, url;

        public ApiClient()
        {
            _sportAppSetting = new SportAppSetting();
            vendorId = _sportAppSetting.VendorID;
            url = _sportAppSetting.URL;

            if (!url.EndsWith("/"))
            {
                url += "/";
            }
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

        #region json Serialzer

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
                DependencyUtil.ResolveService<ILogUtilService>().Value.Info("反序列化失败：" + ex.Message + ",堆栈：" + ex.StackTrace);

                return null;
            }
        }

        #endregion json Serialzer
    }
}