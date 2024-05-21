using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MS.Core.Extensions;
using MS.Core.Helpers.RestRequestHelpers;
using MS.Core.Infrastructures.Exceptions;
using MS.Core.Infrastructures.Providers;
using MS.Core.Infrastructures.ZeroOne.Models;
using MS.Core.Infrastructures.ZeroOne.Models.Requests;
using MS.Core.Infrastructures.ZeroOne.Models.Responses;
using MS.Core.Models;
using MS.Core.Models.Models;
using MS.Core.Services;
using MS.Core.Utils;
using Newtonsoft.Json;
using RestSharp.Extensions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;

namespace MS.Core.Infrastructures.ZoneOne
{
    public class ZeroOneApiServiceBase : BaseHttpRequestService
    {
        public ZeroOneApiServiceBase(
            IDateTimeProvider dateTimeProvider,
            IOptions<ZeroOneSettings> zeroOneSettingsOptions, IRequestIdentifierProvider provider, ILogger logger) : base(provider, logger)
        {
            DateTimeProvider = dateTimeProvider;
            ZeroOneSettings = zeroOneSettingsOptions.Value;
        }

        public IDateTimeProvider DateTimeProvider { get; }

        public ZeroOneSettings ZeroOneSettings { get; }

        private readonly static int ChunkSize = 15 * 1024 * 1024;

        public async Task<BaseReturnDataModel<OutPut>> GetAsync<OutPut>(ZeroOneApi zeroOneApi, object input)
        {
            return await TryCatchProcedure(async (param) =>
            {
                var (Sign, Ts) = GetSign(param);

                var res = await RestRequestHelper.Request(ZeroOneSettings.Domain, zeroOneApi.GetApiUrl())
                      .Get(e => e
                              .AddParameter(param)
                              .AddHeader("sign", Sign)
                              .AddHeader("ts", Ts.ToString())
                          )
                      .ResponseAsync<ZenoOneResDataBase<OutPut>>();

                CheckSuccess(res);

                return new BaseReturnDataModel<OutPut>(ReturnCode.Success)
                {
                    DataModel = GetResponseData(res)
                };

            }, input);
        }

        public async Task<BaseReturnDataModel<OutPut>> PostAsync<OutPut>(ZeroOneApi zeroOneApi, object input)
        {
            return await TryCatchProcedure(async (param) =>
            {
                var r = GetSign(param);

                var res = await RestRequestHelper.Request(ZeroOneSettings.Domain, zeroOneApi.GetApiUrl())
                    .Post(e => e
                            .AddParameter(param)
                            .AddHeader("sign", r.Sign)
                            .AddHeader("ts", r.Ts.ToString())
                        )
                    .ResponseAsync<ZenoOneResDataBase<OutPut>>();

                CheckSuccess(res);

                return new BaseReturnDataModel<OutPut>(ReturnCode.Success)
                {
                    DataModel = GetResponseData(res)
                };
            }, input);
        }

        public async Task<BaseReturnModel> PostAsync(ZeroOneApi zeroOneApi, object input)
        {
            return await TryCatchProcedure(async (param) =>
            {
                var r = GetSign(param);
                var res = await RestRequestHelper.Request(ZeroOneSettings.Domain, zeroOneApi.GetApiUrl())
                    .Post(e => e
                            .AddParameter(param)
                            .AddHeader("sign", r.Sign)
                            .AddHeader("ts", r.Ts.ToString())
                        )
                    .ResponseAsync<ZenoOneResDataBase>();

                CheckSuccess(res);

                var result = GetResponse(res);

                return new BaseReturnModel(ReturnCode.Success);
            }, input);
        }

        public async Task<BaseReturnDataModel<string>> MediaUploadAsync(string domain, ZeroOneApi zeroOneApi, ZOMediaUploadReq req)
        {
            return await TryCatchProcedure(async (param) =>
            {
                using (var md5 = MD5.Create())
                {
                    // 切割傳輸
                    var chunkCount = (param.FileBody.Length + ChunkSize - 1) / ChunkSize;
                    var mergeReq = new ZOMediaMergeReq()
                    {
                        suffix = param.FileNameExtension
                    };
                    for (var i = 0; i < chunkCount; i++)
                    {
                        if (param.FileBody.Length > i * ChunkSize)
                        {
                            var r = GetSign(new
                            {
                                path = "video"
                            }, ZeroOneSettings.MediaSalt);
                            var body = param.FileBody.Skip(i * ChunkSize).Take(ChunkSize).ToArray();
                            var name = ToHexString(md5.ComputeHash(body)).ToLower();
                            var upload = new Dictionary<string, byte[]>();
                            var res = await RestRequestHelper.Request(domain, zeroOneApi.GetApiUrl())
                            .Post(e => e
                                    .AddFileUpload(name, body, name)
                                    .AddHeader("sign", r.Sign)
                                    .AddHeader("ts", r.Ts.ToString())
                                )
                            .ResponseAsync<ZenoOneResDataBase<string[]>>();

                            CheckSuccess(res);

                            var result = GetResponse(res);
                            mergeReq.path_list.Add(result.Data.FirstOrDefault());
                        }
                    }

                    // 合併檔案
                    var mergeRes = await RestRequestHelper.Request(domain, ZeroOneApi.MediaMerge.GetApiUrl())
                        .Post(e => e
                                .AddParameter(mergeReq)
                            )
                        .ResponseAsync<ZenoOneResDataBase<string>>();

                    CheckSuccess(mergeRes);

                    var mergeResult = GetResponse(mergeRes);
                    return new BaseReturnDataModel<string>(ReturnCode.Success)
                    {
                        DataModel = mergeResult.Data
                    };
                }
            }, req);
        }

        public async Task<BaseReturnDataModel<string>> MediaSplitUpload(string domain, ZeroOneApi zeroOneApi, ZOMediaUploadReq param)
        {
            if (param.FileBody.Length > ChunkSize)
            {
                return new BaseReturnDataModel<string>(ReturnCode.ParameterIsInvalid);
            }

            using (var md5 = MD5.Create())
            {
                var r = GetSign(new
                {
                    path = "video"
                }, ZeroOneSettings.MediaSalt);
                var body = param.FileBody;
                var name = ToHexString(md5.ComputeHash(body)).ToLower();
                var upload = new Dictionary<string, byte[]>();
                var res = await RestRequestHelper.Request(domain, zeroOneApi.GetApiUrl())
                .Post(e => e
                        .AddFileUpload(name, body, name)
                        .AddHeader("sign", r.Sign)
                        .AddHeader("ts", r.Ts.ToString())
                    )
                .ResponseAsync<ZenoOneResDataBase<string[]>>();

                CheckSuccess(res);

                var result = GetResponse(res);
                if (result.Success)
                {
                    return new BaseReturnDataModel<string>(ReturnCode.Success)
                    {
                        DataModel = result.Data.FirstOrDefault()
                    };
                }
                else
                {
                    return new BaseReturnDataModel<string>(ReturnCode.ThirdPartyApiNotSuccess);
                }
            }
        }

        public async Task<BaseReturnDataModel<string>> MediaMergeUpload(string domain, string[] paths, string suffix)
        {
            if (string.IsNullOrWhiteSpace(suffix) || paths.Length < 1)
            {
                return new BaseReturnDataModel<string>(ReturnCode.ParameterIsInvalid);
            }

            // 合併檔案
            var mergeReq = new ZOMediaMergeReq()
            {
                suffix = suffix,
                path_list = paths.ToList(),
            };

            var mergeRes = await RestRequestHelper.Request(domain, ZeroOneApi.MediaMerge.GetApiUrl())
                .Post(e => e
                        .AddParameter(mergeReq)
                    )
                .ResponseAsync<ZenoOneResDataBase<string>>();

            CheckSuccess(mergeRes);

            var mergeResult = GetResponse(mergeRes);
            return new BaseReturnDataModel<string>(ReturnCode.Success)
            {
                DataModel = mergeResult.Data
            };
        }

        protected override async Task<ReturnModel> TryCatchProcedure<InputData, ReturnModel>(Func<InputData, Task<ReturnModel>> procedure, InputData param, string failLog = "fail", [CallerMemberName] string methodName = "")
        {
            var result = new ReturnModel();
            try
            {
                if (procedure != null)
                {
                    return await procedure(param);
                }
            }
            catch (ZeroOneException zeroEx)
            {
                if (param == null)
                {
                    _logger.LogError(zeroEx, $"RequestId:{Provider.GetRequestId()} {methodName} {failLog} {zeroEx.Message}");
                }
                else
                {
                    _logger.LogError(zeroEx, $"RequestId:{Provider.GetRequestId()} {methodName} {failLog} {zeroEx.Message}, param:{JsonUtil.ToJsonString(param)}");
                }
                result.SetCode(zeroEx.ReturnCode);
                result.Message = zeroEx.Message ?? string.Empty;
            }
            catch (Exception ex)
            {
                if (param == null)
                {
                    _logger.LogError(ex, $"RequestId:{Provider.GetRequestId()} {methodName} {failLog}");
                }
                else
                {
                    _logger.LogError(ex, $"RequestId:{Provider.GetRequestId()} {methodName} {failLog}, param:{JsonUtil.ToJsonString(param)}");
                }
                result.SetCode(ReturnCode.SystemError);
            }
            return result;
        }

        protected override async Task<T> TryCatchProcedure<T>(Func<Task<T>> procedure, string failLog = "fail", [CallerMemberName] string methodName = "")
        {
            var result = new T();
            try
            {
                if (procedure != null)
                {
                    return await procedure();
                }
            }
            catch (ZeroOneException zeroEx)
            {
                _logger.LogError(zeroEx, $"RequestId:{Provider.GetRequestId()} {methodName} {failLog} {zeroEx.Message}");
                result.SetCode(zeroEx.ReturnCode);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"RequestId:{Provider.GetRequestId()} {methodName} {failLog}");
            }
            result.SetCode(ReturnCode.SystemError);
            return result;
        }

        private void CheckSuccess<OutPut>(RestResponseModel<ZenoOneResDataBase<OutPut>> res)
        {
            RestResponseModel<ZenoOneResDataBase> restResponse = new()
            {
                ErrorMessage = res.ErrorMessage,
                IsSuccess = res.IsSuccess,
                Request = res.Request,
                Response = res.Response,
                Result = res.Result,
            };
            CheckSuccess(restResponse);
        }

        private void CheckSuccess(RestResponseModel<ZenoOneResDataBase> res)
        {
            var logData = JsonConvert.SerializeObject(new
            {
                res.Request,
                res.Result,
                res.ErrorMessage,
                res.Response.StatusCode,
                res.Response.Content,
                res.IsSuccess,
                ResponseErrorMessage = res.Response.ErrorMessage
            });
            if (res.IsSuccess && res.Response.IsSuccessful)
            {
                _logger.LogInformation($"ZO{res.Request.Method} LogData:{logData}");
            }
            else
            {
                _logger.LogError($"ZO{res.Request.Method} LogData:{logData}");
                throw new ZeroOneException(ReturnCode.ThirdPartyApi, $"resError:{res.Response.ErrorMessage} error:{res.ErrorMessage}");
            }
        }

        /// <summary>
        /// 取得簽章
        /// </summary>
        /// <returns>簽章</returns>
        public (string Sign, long Ts) GetSign(object model)
        {
            return GetSign(model, ZeroOneSettings.Salt);
        }

        /// <summary>
        /// 取得簽章
        /// </summary>
        /// <returns>簽章</returns>
        public (string Sign, long Ts) GetSign(object model, string salt)
        {
            var r = GetSignRaw(model, salt);
            return (ToHexString(MD5.Create()
                .ComputeHash(Encoding.UTF8.GetBytes(r.Sign)))
                .ToLower(), r.Ts);
        }
        public (string Sign, long Ts) GetSignRaw<T>(T model, string salt)
        {
            var saltParam = $"{nameof(ZeroOneSettings.Salt).ToCamelCase()}={salt}";

            long ts = new DateTimeOffset(DateTimeProvider.UtcNow).ToUnixTimeSeconds();

            if (model == null)
            {
                return ($"ts={ts}&{saltParam}", ts);
            }

            var elements = model.GetType().GetProperties()
                .Where(e => e.PropertyType.IsArray == false)
                .Select(e =>
                {
                    object? value = GetValue(model, e);
                    return new SaltModel
                    {
                        Name = e.Name,
                        Value = value
                    };
                }).ToList();

            elements.Add(new SaltModel
            {
                Name = "ts",
                Value = ts
            });

            var list = new List<string>();
            foreach (var element in elements.OrderBy(e => e.Name))
            {
                list.Add($"{element.Name.ToCamelCase()}={element.Value}");
            }
            list.Add(saltParam);
            return (string.Join("&", list), ts);
        }

        private static object? GetValue<T>(T model, PropertyInfo e)
        {
            object? value;

            if (e.PropertyType.IsEnum)
            {
                var v = e.GetValue(model);
                if (v == null)
                {
                    value = null;
                }
                else
                {
                    value = (int)v;
                }
            }
            else
            {
                value = e.GetValue(model);
            }

            return value;
        }

        protected string ToHexString(byte[] vs)
        {
            return string.Join(string.Empty, vs.Select(x => x.ToString("X2")));
        }

        private static T GetResponseData<T>(RestResponseModel<ZenoOneResDataBase<T>> apiRestResponse)
        {
            var result = GetResponse(apiRestResponse);

            return result.Data;
        }

        private static T GetResponse<T>(RestResponseModel<T> apiRestResponse) where T : ZenoOneResDataBase
        {
            var response = apiRestResponse.Response;

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new ZeroOneException(ReturnCode.ThirdPartyApi, response.ErrorMessage);
            }

            var result = apiRestResponse.Result;

            if (result == null)
            {
                throw new ZeroOneException(ReturnCode.ThirdPartyApiNull, response.ErrorMessage);
            }

            if (result.Success == false)
            {
                throw new ZeroOneException(ReturnCode.ThirdPartyApiNotSuccess, result.Error);
            }

            return result;
        }

        protected async Task<BaseReturnDataModel<VideoUrlModel>> GetUploadVideoUrl(string domain, ZeroOneApi zeroOneApi)
        {
            try
            {
                using (var md5 = MD5.Create())
                {
                    var r = GetSign(new
                    {
                        path = "video"
                    }, ZeroOneSettings.MediaSalt);
                    return await Task.FromResult(new BaseReturnDataModel<VideoUrlModel>(ReturnCode.Success)
                    {
                        DataModel = new VideoUrlModel()
                        {
                            Sign = r.Sign,
                            Ts = r.Ts,
                            Url = new Uri(new Uri(domain), zeroOneApi.GetApiUrl()).AbsoluteUri
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetUploadVideoUrl fails");
            }

            return await Task.FromResult(new BaseReturnDataModel<VideoUrlModel>(ReturnCode.OperationFailed));
        }
    }
    public class SaltModel
    {
        public string Name { get; set; }
        public object? Value { get; set; }
    }
}