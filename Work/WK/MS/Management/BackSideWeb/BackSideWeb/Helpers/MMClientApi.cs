using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.Web.BackSideWeb;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Repository.Extensions;
using MS.Core.MMModel.Models;
using MS.Core.Models.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Text;

namespace BackSideWeb.Helpers
{
    public class MMClientApi
    {
        public static async Task<string> GetAccessToken()
        {
            var url = new StringBuilder(MMApiHelper.ApiUrl);
            url.AppendFormat("/{0}/{1}", "Auth", "BackendSignIn");

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var user = DependencyUtil.ResolveService<IBackSideWebUserService>().Value.GetUser();

                object data = new
                {
                    UserId = user.UserId,
                    Nickname = user.UserName
                };

                var jsonContent = JsonConvert.SerializeObject(data);

                var postContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(url.ToString(), postContent);

                response.EnsureSuccessStatusCode();

                var jsonString = await response.Content.ReadAsStringAsync();
                var json = JObject.Parse(jsonString);

                return (string)json["data"]["accessToken"];
            }
        }

        public static PageResultModel<T2> PostApi<T1, T2>(string controller, string action, T1 data)
        {
            var url = new StringBuilder(MMApiHelper.ApiUrl);
            url.AppendFormat("/{0}/{1}", controller, action);

            var result = new PageResultModel<T2>();

            using (var client = new HttpClient())
            {
                var accessToken = GetAccessToken().GetAwaiter().GetResult();

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var jsonContent = JsonConvert.SerializeObject(data);

                var response = client.PostAsync(url.ToString(), new StringContent(jsonContent, Encoding.UTF8, "application/json")).GetAwaiter().GetResult();

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    var json = JObject.Parse(jsonString);
                    if (Convert.ToBoolean(json["isSuccess"]))
                    {
                        var dataJsonString = json["data"].ToString();
                        result = JsonConvert.DeserializeObject<PageResultModel<T2>>(json["data"].ToString());
                    }
                }
            }
            return result;
        }

        public static ApiResponse<T> PostRequest<T>(string controller, string action, string parameStr) where T : class, new()
        {
            var url = new StringBuilder(MMApiHelper.ApiUrl);
            url.AppendFormat("/{0}/{1}", controller, action);

            var result = new ApiResponse<T>();
            using (var client = new HttpClient())
            {
                var accessToken = GetAccessToken().GetAwaiter().GetResult();

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                //var jsonContent = JsonConvert.SerializeObject(data);

                var response = client.PostAsync(url.ToString(), new StringContent(parameStr, Encoding.UTF8, "application/json")).GetAwaiter().GetResult();

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    var json = JObject.Parse(jsonString);
                    if (Convert.ToBoolean(json["isSuccess"]))
                    {
                        result.IsSuccess = true;
                        var dataJsonString = json["data"].ToString();
                        result.Data = JsonConvert.DeserializeObject<T>(dataJsonString);
                    }
                    else
                    {
                        result.Message = json["message"].ToString();
                        result.IsSuccess = false;
                        result.Code = json["code"].ToString();
                    }
                }
                else
                {
                    result.Message = $"Failed to call API {url}. Error code: {response.StatusCode}. Error message: {response.ReasonPhrase}.";
                    result.IsSuccess = false;
                }
            }
            return result;
        }

        public static ApiResponse PostObjectApi(string controller, string action, string parameStr)
        {
            var url = new StringBuilder(MMApiHelper.ApiUrl);
            url.AppendFormat("/{0}/{1}", controller, action);

            var result = new ApiResponse();
            using (var client = new HttpClient())
            {
                var accessToken = GetAccessToken().GetAwaiter().GetResult();

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                //var jsonContent = JsonConvert.SerializeObject(data);

                var response = client.PostAsync(url.ToString(), new StringContent(parameStr, Encoding.UTF8, "application/json")).GetAwaiter().GetResult();

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    var json = JObject.Parse(jsonString);
                    if (Convert.ToBoolean(json["isSuccess"]))
                        result.IsSuccess = true;
                    else
                    {
                        result.Message = json["message"].ToString();
                        result.IsSuccess = false;
                        result.Code = json["code"].ToString();
                    }
                }
                else
                {
                    result.Message = $"Failed to call API {url}. Error code: {response.StatusCode}. Error message: {response.ReasonPhrase}.";
                    result.IsSuccess = false;
                }
            }
            return result;
        }

        public static BaseReturnDataModel<List<T>> PostListData<T>(string controller, string action, string parameStr) where T : class
        {
            var url = new StringBuilder(MMApiHelper.ApiUrl);
            url.AppendFormat("/{0}/{1}", controller, action);

            var result = new BaseReturnDataModel<List<T>>();
            using (var client = new HttpClient())
            {
                var accessToken = GetAccessToken().GetAwaiter().GetResult();

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                //var jsonContent = JsonConvert.SerializeObject(data);

                var response = client.PostAsync(url.ToString(), new StringContent(parameStr, Encoding.UTF8, "application/json")).GetAwaiter().GetResult();

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    var json = JObject.Parse(jsonString);
                    if (json["data"] != null && json["data"].HasValues)
                    {
                        if (Convert.ToBoolean(json["isSuccess"]))
                        {
                            var dataArray = json["data"].ToArray();

                            //if (dataArray.Length > 0)
                            //{
                            //    var dataJsonString = json["data"].ToString();
                            //    result.Datas = JsonConvert.DeserializeObject<List<T>>(dataJsonString);
                            //}IsFeatureText

                            var dataJsonString = json["data"].ToString();
                            result.DataModel = JsonConvert.DeserializeObject<List<T>>(dataJsonString);

                            result.IsSuccess = true;
                        }
                        else
                        {
                            result.Message = json["message"].ToString();
                            result.IsSuccess = false;
                            result.Code = string.IsNullOrWhiteSpace(json["code"].ToString()) ? JxBackendService.Model.Enums.ReturnCode.SystemError.Value : json["code"].ToString();
                        }
                    }
                    else
                    {
                        if (Convert.ToBoolean(json["isSuccess"]))
                            result.IsSuccess = true;
                        else
                        {
                            result.Message = json["message"].ToString();
                            result.IsSuccess = false;
                            result.Code = json["code"].ToString();
                        }
                    }
                }
                else
                {
                    result.Message = $"Failed to call API {url}. Error code: {response.StatusCode}. Error message: {response.ReasonPhrase}.";
                    result.IsSuccess = false;
                }
            }
            return result;
        }

        public static BaseReturnDataModel<PageResultModel<T>> PostApi<T>(string controller, string action, string parameStr) where T : class
        {
            var url = new StringBuilder(MMApiHelper.ApiUrl);
            url.AppendFormat("/{0}/{1}", controller, action);

            var result = new BaseReturnDataModel<PageResultModel<T>>();
            using (var client = new HttpClient())
            {
                var accessToken = GetAccessToken().GetAwaiter().GetResult();

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var response = client.PostAsync(url.ToString(), new StringContent(parameStr, Encoding.UTF8, "application/json")).GetAwaiter().GetResult();

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    var json = JObject.Parse(jsonString);
                    if (json["data"] != null && json["data"].HasValues)
                    {
                        if (Convert.ToBoolean(json["isSuccess"]))
                        {
                            var dataArray = json["data"]["data"].ToArray();

                            //if (dataArray.Length > 0)
                            //{
                            //    var dataJsonString = json["data"].ToString();
                            //    result.Datas = JsonConvert.DeserializeObject<List<T>>(dataJsonString);
                            //}IsFeatureText

                            var dataJsonString = json["data"].ToString();
                            result.DataModel = JsonConvert.DeserializeObject<PageResultModel<T>>(dataJsonString);

                            result.IsSuccess = true;
                        }
                        else
                        {
                            result.Message = json["message"].ToString();
                            result.IsSuccess = false;
                            result.Code = string.IsNullOrWhiteSpace(json["code"].ToString()) ? JxBackendService.Model.Enums.ReturnCode.SystemError.Value : json["code"].ToString();
                        }
                    }
                    else
                    {
                        if (Convert.ToBoolean(json["isSuccess"]))
                            result.IsSuccess = true;
                        else
                        {
                            result.Message = json["message"].ToString();
                            result.IsSuccess = false;
                            result.Code = json["code"].ToString();
                        }
                    }
                }
                else
                {
                    result.Message = $"Failed to call API {url}. Error code: {response.StatusCode}. Error message: {response.ReasonPhrase}.";
                    result.IsSuccess = false;
                }
            }
            return result;
        }

        public static ApiResult<T> PostApi<T>(string controller, string action, T data) where T : class
        {
            var url = new StringBuilder(MMApiHelper.ApiUrl);
            url.AppendFormat("/{0}/{1}", controller, action);

            var result = new ApiResult<T>();

            using (var client = new HttpClient())
            {
                var accessToken = GetAccessToken().GetAwaiter().GetResult();

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var jsonContent = JsonConvert.SerializeObject(data);

                var response = client.PostAsync(url.ToString(), new StringContent(jsonContent, Encoding.UTF8, "application/json")).GetAwaiter().GetResult();

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    var json = JObject.Parse(jsonString);
                    if (json["data"] != null)
                    {
                        if (Convert.ToBoolean(json["isSuccess"]))
                        {
                            var dataJsonString = json["data"].ToString();
                            result.Datas = JsonConvert.DeserializeObject<List<T>>(dataJsonString);
                            result.IsSuccess = true;
                        }
                        else
                        {
                            result.Message = json["message"].ToString();
                            result.IsSuccess = false;
                            result.Code = json["code"].ToString();
                        }
                    }
                    else
                    {
                        if (Convert.ToBoolean(json["isSuccess"]))
                            result.IsSuccess = true;
                        else
                        {
                            result.Message = json["message"].ToString();
                            result.IsSuccess = false;
                            result.Code = json["code"].ToString();
                        }
                    }
                }
                else
                {
                    result.Message = $"Failed to call API {url}. Error code: {response.StatusCode}. Error message: {response.ReasonPhrase}.";
                    result.IsSuccess = false;
                }
            }
            return result;
        }

        public static ApiResponse PostApi2<T>(string controller, string action, T data)
        {
            var url = new StringBuilder(MMApiHelper.ApiUrl);
            url.AppendFormat("/{0}/{1}", controller, action);

            var result = new ApiResponse { IsSuccess = false };
            try
            {
                using (var client = new HttpClient())
                {
                    var accessToken = GetAccessToken().GetAwaiter().GetResult();

                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                    var parameStr = JsonConvert.SerializeObject(data);
                    var response = client.PostAsync(url.ToString(), new StringContent(parameStr, Encoding.UTF8, "application/json")).GetAwaiter().GetResult();

                    if (response.IsSuccessStatusCode)
                    {
                        var jsonString = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                        var json = JObject.Parse(jsonString);
                        if (json["data"] != null)
                        {
                            if (Convert.ToBoolean(json["isSuccess"]))
                            {
                                var dataJson = JObject.Parse(json["data"].ToString());
                                result.IsSuccess = Convert.ToBoolean(dataJson["isSuccess"]);
                                result.Message = dataJson["message"]?.ToString();
                            }
                        }
                        else
                            result.Message = json["message"].ToString();
                    }
                }
            }
            catch (Exception)
            {
                result.Message = "服务器内部异常";
            }
            return result;
        }

        public static ApiResult PostApi(string controller, string action, string id)
        {
            var url = new StringBuilder(MMApiHelper.ApiUrl);
            url.AppendFormat("/{0}/{1}/{2}", controller, action, id);

            var result = new ApiResult();

            using (var client = new HttpClient())
            {
                var accessToken = GetAccessToken().GetAwaiter().GetResult();

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var response = client.PostAsync(url.ToString(), null).GetAwaiter().GetResult();

                if (response.IsSuccessStatusCode)
                {
                    result.IsSuccess = true;
                }
                else
                {
                    result.Message = $"Failed to call API {url}. Error code: {response.StatusCode}. Error message: {response.ReasonPhrase}.";
                    result.IsSuccess = false;
                }
            }

            return result;
        }

        public static ApiResult PostApiWithParams(string controller, string action, Dictionary<string, string> queryParams)
        {
            var url = new StringBuilder(MMApiHelper.ApiUrl);
            url.AppendFormat("/{0}/{1}", controller, action);

            if (queryParams != null && queryParams.Count > 0)
            {
                url.Append("?");
                foreach (var queryParam in queryParams)
                {
                    url.AppendFormat("{0}={1}&", queryParam.Key, queryParam.Value);
                }
                url.Remove(url.Length - 1, 1);
            }

            var result = new ApiResult();

            using (var client = new HttpClient())
            {
                var accessToken = GetAccessToken().GetAwaiter().GetResult();

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var response = client.PostAsync(url.ToString(), null).GetAwaiter().GetResult();

                if (response.IsSuccessStatusCode)
                {
                    result.IsSuccess = true;
                }
                else
                {
                    result.Message = $"Failed to call API {url}. Error code: {response.StatusCode}. Error message: {response.ReasonPhrase}.";
                }
            }

            return result;
        }

        public static ApiResponse<T> GetApiSingle<T>(string controller, string action, string? id)
        {
            var url = new StringBuilder(MMApiHelper.ApiUrl);
            url.AppendFormat("/{0}/{1}", controller, action);

            if (!string.IsNullOrWhiteSpace(id))
            {
                url.AppendFormat("/{0}", id);
            }

            var result = new ApiResponse<T>();
            using (var client = new HttpClient())
            {
                var accessToken = GetAccessToken().GetAwaiter().GetResult();

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var response = client.GetAsync(url.ToString()).GetAwaiter().GetResult();

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    var json = JObject.Parse(jsonString);
                    var dataJsonString = json["data"].ToString();
                    result.Data = JsonConvert.DeserializeObject<T>(dataJsonString);
                    result.IsSuccess = true;
                }
                else
                {
                    result.Message = $"Failed to call API {url}. Error code: {response.StatusCode}. Error message: {response.ReasonPhrase}.";
                    result.IsSuccess = false;
                }
            }
            return result;
        }

        public static BaseReturnDataModel<T> GetApiSingleBaseReturn<T>(string controller, string action, string? id)
        {
            var url = new StringBuilder(MMApiHelper.ApiUrl);
            url.AppendFormat("/{0}/{1}", controller, action);

            if (!string.IsNullOrWhiteSpace(id))
            {
                url.AppendFormat("/{0}", id);
            }

            var result = new BaseReturnDataModel<T>();
            using (var client = new HttpClient())
            {
                var accessToken = GetAccessToken().GetAwaiter().GetResult();

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var response = client.GetAsync(url.ToString()).GetAwaiter().GetResult();

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    var json = JObject.Parse(jsonString);
                    var dataJsonString = json["data"].ToString();
                    result.DataModel = JsonConvert.DeserializeObject<T>(dataJsonString);
                    result.IsSuccess = true;
                }
                else
                {
                    result.Message = $"Failed to call API {url}. Error code: {response.StatusCode}. Error message: {response.ReasonPhrase}.";
                    result.IsSuccess = false;
                }
            }
            return result;
        }

        public static ApiResult<T> GetApiByDictionary<T>(string controller, string action, Dictionary<string, string> queryParams)
        {
            var url = new StringBuilder(MMApiHelper.ApiUrl);
            url.AppendFormat("/{0}/{1}", controller, action);
            if (queryParams != null && queryParams.Count > 0)
            {
                url.AppendFormat("?");
                bool isFirst = true;
                foreach (var item in queryParams.Keys)
                {
                    if (isFirst)
                        url.AppendFormat("{0}={1}", item, queryParams[item]);
                    else
                        url.AppendFormat("&{0}={1}", item, queryParams[item]);

                    isFirst = false;
                }
            }

            var result = new ApiResult<T>();
            using (var client = new HttpClient())
            {
                var accessToken = GetAccessToken().GetAwaiter().GetResult();

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var response = client.GetAsync(url.ToString()).GetAwaiter().GetResult();

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    var json = JObject.Parse(jsonString);
                    var dataJsonString = json["data"].ToString();
                    result.Datas = JsonConvert.DeserializeObject<List<T>>(dataJsonString);
                    result.IsSuccess = true;
                }
                else
                {
                    result.Message = $"Failed to call API {url}. Error code: {response.StatusCode}. Error message: {response.ReasonPhrase}.";
                    result.IsSuccess = false;
                }
            }
            return result;
        }

        public static ApiResult<T> GetApi<T>(string controller, string action, int? id = null) where T : class
        {
            var url = new StringBuilder(MMApiHelper.ApiUrl);
            url.AppendFormat("/{0}/{1}", controller, action);

            if (id.HasValue)
            {
                url.AppendFormat("/{0}", id.Value);
            }

            var result = new ApiResult<T>();
            using (var client = new HttpClient())
            {
                var accessToken = GetAccessToken().GetAwaiter().GetResult();

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var response = client.GetAsync(url.ToString()).GetAwaiter().GetResult();

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    var json = JObject.Parse(jsonString);
                    var dataJsonString = json["data"].ToString();
                    result.Datas = JsonConvert.DeserializeObject<List<T>>(dataJsonString);
                    result.IsSuccess = true;
                }
                else
                {
                    result.Message = $"Failed to call API {url}. Error code: {response.StatusCode}. Error message: {response.ReasonPhrase}.";
                    result.IsSuccess = false;
                }
            }
            return result;
        }

        public static ApiSingleResult<T> GetSingleApiNoParam<T>(string controller, string action) where T : class
        {
            var url = new StringBuilder(MMApiHelper.ApiUrl);
            url.AppendFormat("/{0}/{1}", controller, action);

            var result = new ApiSingleResult<T>();
            using (var client = new HttpClient())
            {
                var accessToken = GetAccessToken().GetAwaiter().GetResult();

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var response = client.GetAsync(url.ToString()).GetAwaiter().GetResult();

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    var json = JObject.Parse(jsonString);
                    var dataJsonString = json["data"].ToString();
                    result.Datas = JsonConvert.DeserializeObject<T>(dataJsonString);
                    result.IsSuccess = true;
                }
                else
                {
                    result.Message = $"Failed to call API {url}. Error code: {response.StatusCode}. Error message: {response.ReasonPhrase}.";
                    result.IsSuccess = false;
                }
            }
            return result;
        }

        public static ApiSingleResult<T> GetSingleApi<T>(string controller, string action, string id = null) where T : class
        {
            var url = new StringBuilder(MMApiHelper.ApiUrl);
            url.AppendFormat("/{0}/{1}", controller, action);

            if (!string.IsNullOrEmpty(id))
            {
                url.AppendFormat("/{0}", id);
            }

            var result = new ApiSingleResult<T>();

            using (var client = new HttpClient())
            {
                var accessToken = GetAccessToken().GetAwaiter().GetResult();

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var response = client.GetAsync(url.ToString()).GetAwaiter().GetResult();

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    var json = JObject.Parse(jsonString);
                    var dataJsonString = json["data"].ToString();
                    result.Datas = JsonConvert.DeserializeObject<T>(dataJsonString);
                    result.IsSuccess = true;
                }
                else
                {
                    result.Message = $"Failed to call API {url}. Error code: {response.StatusCode}. Error message: {response.ReasonPhrase}.";
                    result.IsSuccess = false;
                }
            }

            return result;
        }

        public static ApiResult<T> GetApiWithParams<T>(string controller, string action, Dictionary<string, string> queryParams) where T : class
        {
            var url = new StringBuilder(MMApiHelper.ApiUrl);
            url.AppendFormat("/{0}/{1}", controller, action);

            if (queryParams != null && queryParams.Count > 0)
            {
                url.Append("?");
                foreach (var queryParam in queryParams)
                {
                    url.AppendFormat("{0}={1}&", queryParam.Key, queryParam.Value);
                }
                url.Remove(url.Length - 1, 1);
            }

            var result = new ApiResult<T>();

            using (var client = new HttpClient())
            {
                var accessToken = GetAccessToken().GetAwaiter().GetResult();

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var response = client.GetAsync(url.ToString()).GetAwaiter().GetResult();

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    var json = JObject.Parse(jsonString);
                    var dataJsonString = json["data"].ToString();
                    result.Datas = JsonConvert.DeserializeObject<List<T>>(dataJsonString);
                    result.IsSuccess = true;
                }
                else
                {
                    result.Message = $"Failed to call API {url}. Error code: {response.StatusCode}. Error message: {response.ReasonPhrase}.";
                }
            }
            return result;
        }

        public static ApiResponse<string> PostRequest(string controller, string action, string parameStr)
        {
            var url = new StringBuilder(MMApiHelper.ApiUrl);
            url.AppendFormat("/{0}/{1}", controller, action);

            var result = new ApiResponse<string>();
            using (var client = new HttpClient())
            {
                var accessToken = GetAccessToken().GetAwaiter().GetResult();

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var response = client.PostAsync(url.ToString(), new StringContent(parameStr, Encoding.UTF8, "application/json")).GetAwaiter().GetResult();

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    var json = JObject.Parse(jsonString);
                    if (Convert.ToBoolean(json["isSuccess"]))
                    {
                        result.IsSuccess = true;
                        result.Data = json["data"].ToString();
                    }
                    else
                    {
                        result.Message = json["message"].ToString();
                        result.IsSuccess = false;
                        result.Code = json["code"].ToString();
                    }
                }
                else
                {
                    result.Message = $"Failed to call API {url}. Error code: {response.StatusCode}. Error message: {response.ReasonPhrase}.";
                    result.IsSuccess = false;
                }
            }
            return result;
        }
    }
}