using ImageMagick;
using Microsoft.Extensions.Logging;
using MS.Core.Infrastructure.OSS;
using MS.Core.MM.Model.Entities.Media;
using MS.Core.MM.Model.Media;
using MS.Core.MM.Models;
using MS.Core.MM.Models.Media;
using MS.Core.MM.Repos.interfaces;
using MS.Core.MM.Services.interfaces;
using MS.Core.MMModel.Models;
using MS.Core.MMModel.Models.Media.Enums;
using MS.Core.MMModel.Models.Post.Enums;
using MS.Core.Models;
using MS.Core.Models.Models;
using MS.Core.Services;
using MS.Core.Utils;
using Newtonsoft.Json;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace MS.Core.MM.Service
{
    /// <summary>
    /// 媒體服務的基礎型別
    /// </summary>
    public abstract class BaseImageService : BaseService, IMediaService
    {
        /// <summary>
        /// 上傳位址
        /// </summary>
        protected abstract string UploadOssPath { get; }

        /// <summary>
        /// Aes Key
        /// </summary>
        private static readonly byte[] AesKey = Encoding.UTF8.GetBytes("94a4b778g01ca4ab");

        /// <summary>
        /// 圖片上傳來源
        /// </summary>
        protected abstract string ApplicationSoruce { get; }

        /// <inheritdoc cref="IMediaRepo"/>
        private readonly IMediaRepo _repo = null;

        /// <inheritdoc/>
        public abstract SourceType SourceType { get; }

        /// <inheritdoc/>
        public MediaType Type => MediaType.Image;

        private readonly IObjectStorageService _oos = null;

        /// <summary>
        /// 縮圖的前贅詞
        /// </summary>
        private static readonly Dictionary<PostType, string> _thumbnailPrefix = new Dictionary<PostType, string>()
        {
            { PostType.Official, "350-350" },
            { PostType.Agency, "225-225" },
            { PostType.Square, "225-225" },
        };

        /// <summary>
        /// 縮圖大小
        /// </summary>
        private static readonly Dictionary<PostType, MagickGeometry> _thumbnailSize = _thumbnailPrefix.ToDictionary(x =>
            x.Key,
            x => new MagickGeometry(
                int.Parse(x.Value.Split("-")[0]),
                int.Parse(x.Value.Split("-")[1]))
            {
                IgnoreAspectRatio = false
            });

        public BaseImageService(IObjectStorageService oos,
            IMediaRepo repo,
            ILogger<BaseImageService> logger) : base(logger)
        {
            _oos = oos;
            _repo = repo;
        }

        /// <inheritdoc/>
        public async Task<BaseReturnModel> Create(SaveMediaToOssParam param)
        {
            var uploadResult = await CreateToOss(param);
            if (!uploadResult.IsSuccess)
            {
                return uploadResult;
            }

            return await TryCatchProcedure<SaveMediaToOssParam, BaseReturnModel>(async (param) =>
            {
                await _repo.Create(param);
                return new BaseReturnModel(ReturnCode.Success);
            }, param);
        }

        /// <summary>
        /// 由CDN讀取
        /// </summary>
        /// <param name="fileUrl">檔案名稱</param>
        /// <returns>讀取結果</returns>
        private async Task<bool> LoadByCDN(string fileUrl)
        {
            using (var client = new HttpClient())
            {
                var resp = await client.GetAsync(fileUrl);
                if (resp.IsSuccessStatusCode)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 轉換成媒體的Url
        /// </summary>
        /// <param name="fullObjectName">完整的名稱</param>
        /// <returns>媒體的Url</returns>
        private string ConvertToMediaUrl(string fullObjectName) => $"/{fullObjectName}";

        /// <inheritdoc/>
        public async Task<BaseReturnModel> Delete(string seqId)
        {
            var deleteOssResult = await DeleteToOss(seqId);
            if (!deleteOssResult.IsSuccess)
            {
                return deleteOssResult;
            }

            return await TryCatchProcedure<string, BaseReturnModel>(async (param) =>
            {
                if (!(await _repo.Delete(param)))
                {
                    return new BaseReturnModel(ReturnCode.OperationFailed);
                }
                return new BaseReturnModel(ReturnCode.Success);
            }, seqId);
        }

        /// <inheritdoc/>
        public async Task<BaseReturnModel> DeleteToOss(string seqId)
        {
            return await TryCatchProcedure<string, BaseReturnModel>(async (param) =>
            {
                var media = await _repo.Get(param);
                if (media == null)
                {
                    return new BaseReturnModel(MMReturnCode.DateIncorrect);
                }

                await _oos.DeleteObject(ConvertToOosUrl(ConvertToThumbnailUrl(media.FileUrl, PostType.Official)));

                await _oos.DeleteObject(ConvertToOosUrl(ConvertToThumbnailUrl(media.FileUrl, PostType.Agency)));

                if (!(await _oos.DeleteObject(ConvertToOosUrl(media.FileUrl))))
                {
                    return new BaseReturnModel(MMReturnCode.OperationFailed);
                }
                return new BaseReturnModel(ReturnCode.Success);
            }, seqId);
        }

        /// <inheritdoc/>
        public async Task<BaseReturnDataModel<MediaInfo[]>> Get(SourceType type, string refId)
        {
            return await Get(type, new string[] { refId });
        }

        /// <inheritdoc/>
        public async Task<BaseReturnDataModel<MediaInfo[]>> Get(SourceType type, string[] refIds)
        {
            if (refIds.Length <= 0)
            {
                _logger.LogError($"{MethodInfo.GetCurrentMethod()} fail, refIds Length is 0");
                return await Task.FromResult(new BaseReturnDataModel<MediaInfo[]>(ReturnCode.MissingNecessaryParameter));
            }
            return await TryCatchProcedure<Tuple<SourceType, string[]>, BaseReturnDataModel<MediaInfo[]>>(async (param) =>
            {
                var result = new BaseReturnDataModel<MediaInfo[]>();
                var queryResult = await _repo.Get((int)Type, (int)param.Item1, param.Item2);
                var items = new List<MediaInfo>();
                foreach (var query in queryResult)
                {
                    var item = JsonUtil.CastByJson<MediaInfo>(query);
                    item.FullMediaUrl = await _oos.GetCdnPath(item.FileUrl);
                    items.Add(item);
                }
                result.DataModel = items.ToArray();
                result.SetCode(ReturnCode.Success);
                return result;
            }, new Tuple<SourceType, string[]>(type, refIds));
        }

        public async Task<BaseReturnDataModel<MediaInfo[]>> GetByIds(SourceType type, string[] ids)
        {
            if (ids.Length <= 0)
            {
                _logger.LogError($"{MethodInfo.GetCurrentMethod()} fail, ids Length is 0");
                return await Task.FromResult(new BaseReturnDataModel<MediaInfo[]>(ReturnCode.MissingNecessaryParameter));
            }
            return await TryCatchProcedure<Tuple<SourceType, string[]>, BaseReturnDataModel<MediaInfo[]>>(async (param) =>
            {
                var result = new BaseReturnDataModel<MediaInfo[]>();
                var queryResult = await _repo.GetByIds((int)Type, (int)param.Item1, param.Item2);
                var items = new List<MediaInfo>();
                foreach (var query in queryResult)
                {
                    var item = JsonUtil.CastByJson<MediaInfo>(query);
                    item.FullMediaUrl = await _oos.GetCdnPath(item.FileUrl);
                    items.Add(item);
                }
                result.DataModel = items.ToArray();
                result.SetCode(ReturnCode.Success);
                return result;
            }, new Tuple<SourceType, string[]>(type, ids));
        }

        /// <inheritdoc/>
        public async Task<BaseReturnModel> Update(SaveMediaToOssParam param)
        {
            var updateResult = await UpdateToOss(param);
            if (!updateResult.IsSuccess)
            {
                return updateResult;
            }
            return await TryCatchProcedure<SaveMediaToOssParam, BaseReturnModel>(async (param) =>
            {
                if (!await _repo.Update(param))
                {
                    return new BaseReturnModel(ReturnCode.OperationFailed);
                }
                return new BaseReturnModel(ReturnCode.Success);
            }, param);
        }

        /// <inheritdoc/>
        public async Task<BaseReturnModel> UpdateToOss(SaveMediaToOssParam param)
        {
            return await TryCatchProcedure<SaveMediaToOssParam, BaseReturnModel>(async (param) =>
            {
                var media = await _repo.Get(param.Id);
                if (media.FileUrl == param.FileUrl)
                {
                    // 圖片一樣就不做事
                    return new BaseReturnModel(ReturnCode.Success);
                }

                param.ModifyDate = DateTime.Now;

                // 先上傳新的圖檔
                var createResult = await CreateToOss(param);

                if (!createResult.IsSuccess)
                {
                    return createResult;
                }

                // 在刪除不一樣的圖檔
                if (!(await _oos.DeleteObject(ConvertToOosUrl(media.FileUrl))))
                {
                    return new BaseReturnModel(ReturnCode.OperationFailed);
                }

                return createResult;
            }, param);
        }

        private string ConvertToOosUrl(string fileUrl)
        {
            return fileUrl.TrimStart('/');
        }

        /// <inheritdoc/>
        public async Task<BaseReturnModel> CheckParam(SaveMediaToOssParam param)
        {
            if (param == null)
            {
                return new BaseReturnModel(ReturnCode.DataIsNotCompleted);
            }

            List<string> validRequiredValues = new List<string>
            {
                param.FileName,
            };

            if (!ParamUtil.IsValidRequired(validRequiredValues.ToArray()))
            {
                return new BaseReturnModel(ReturnCode.DataIsNotCompleted);
            }

            return await ChildCheckParam(param);
        }

        /// <summary>
        /// 由子類別來實作相對應的檢查
        /// </summary>
        /// <param name="param">輸入參數</param>
        /// <returns>驗證結果</returns>
        protected abstract Task<BaseReturnModel> ChildCheckParam(SaveMediaToOssParam param);

        /// <inheritdoc/>
        public async Task<BaseReturnModel> CreateToOss(SaveMediaToOssParam param)
        {
            return await base.TryCatchProcedure(async (param) =>
            {
                if (string.IsNullOrEmpty(param.Id))
                {
                    param.Id = await _repo.CreateNewSEQID();
                }

                if (!string.IsNullOrEmpty(param.FileName))
                {
                    param.FileName = param.FileName.Split('.')[0] + "." + param.FileName.Split('.')[1].ToLower();
                }

                if (string.IsNullOrEmpty(param.FileUrl))
                {
                    var oosName = await _repo.CreateNewObjectName(param.Id, param.FileName);
                    param.FileName = string.Concat(ApplicationSoruce, UploadOssPath, oosName);
                }
                else
                {
                    param.FileName = ConvertToOosUrl(param.FileUrl);
                }

                // 處理二進制檔AES加密的動作
                if (param.IsNeedEncrypt)
                {
                    // 加密才上傳縮圖 中介、官方都要上傳
                    var thumbnailResult = await UploadThumbnail(param, PostType.Agency);
                    if (!thumbnailResult.IsSuccess)
                    {
                        return thumbnailResult;
                    }

                    thumbnailResult = await UploadThumbnail(param, PostType.Official);
                    if (!thumbnailResult.IsSuccess)
                    {
                        return thumbnailResult;
                    }

                    await AesEncryption(param);
                }

                if (!(await _oos.UploadObject(param.Bytes, param.FileName)))
                {
                    return new BaseReturnModel(ReturnCode.OperationFailed);
                }

                param.FileUrl = ConvertToMediaUrl(param.FileName);
                param.FullMediaUrl = await GetFullMediaUrl(param);
                if (!(await LoadByCDN(param.FullMediaUrl)))
                {
                    return new BaseReturnModel(ReturnCode.OperationFailed);
                }

                if (param.CreateDate == default(DateTime))
                {
                    param.CreateDate = DateTime.Now;
                }

                return new BaseReturnModel(ReturnCode.Success);
            }, param);
        }

        /// <summary>
        /// 上傳縮圖
        /// </summary>
        /// <param name="param">原始圖檔</param>
        /// <returns>非同步任務</returns>
        public async Task<BaseReturnModel> UploadThumbnail(SaveMediaToOssParam param, PostType postType, bool isForceToOss = false)
        {
            var extRaw = Path.GetExtension(param.FileName);
            var ext = extRaw.ToLower();

            // 判斷是不是要轉檔
            var bytes = param.Bytes;
            if (ext != ".jpg" && ext != ".jpeg")
            {
                var converts = await ConvertImage(param.Bytes);
                if (converts.Item1)
                {
                    bytes = converts.Item2;
                }
                else if (!isForceToOss)
                {
                    return new BaseReturnModel(ReturnCode.ParameterIsInvalid);
                }
            }

            // 縮圖
            try
            {
                using (var image = new MagickImage(new MemoryStream(bytes)))
                {
                    using (var stream = new MemoryStream())
                    {
                        image.Thumbnail(_thumbnailSize[postType]);

                        // Save the result
                        image.Write(stream);
                        bytes = stream.ToArray();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Resize fail");
                return new BaseReturnModel(ReturnCode.ParameterIsInvalid);
            }

            // 更改附檔名
            var fileName = param.FileName.Replace(ext, ".aes");

            var aesBytes = await AesEncryptionBytes(bytes);

            if (!(await _oos.UploadObject(aesBytes, ConvertToThumbnailUrl(fileName, postType))))
            {
                return new BaseReturnModel(ReturnCode.OperationFailed);
            }

            return new BaseReturnModel(ReturnCode.Success);
        }

        private string ConvertToThumbnailUrl(string fileName, PostType type)
        {
            var splits = fileName.Split("/");
            if (_thumbnailPrefix.ContainsKey(type))
            {
                splits[splits.Length - 1] = string.Concat(_thumbnailPrefix[type], "-", splits[splits.Length - 1]);
            }
            else
            {
                splits[splits.Length - 1] = string.Concat(_thumbnailPrefix[PostType.Square], "-", splits[splits.Length - 1]);
            }
            return string.Join("/", splits);
        }

        /// <summary>
        /// 圖片轉換為jpg
        /// </summary>
        /// <param name="source">來源圖片</param>
        /// <returns>是否成功，轉檔圖片</returns>
        protected async Task<Tuple<bool, byte[]>> ConvertImage(byte[] source)
        {
            try
            {
                using (var image = new MagickImage(new MemoryStream(source)))
                {
                    using (var stream = new MemoryStream())
                    {
                        image.Write(stream, MagickFormat.Jpg);
                        var result = stream.ToArray();
                        return await Task.FromResult(new Tuple<bool, byte[]>(true, result));
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Convert to jpg fail");
                return await Task.FromResult(new Tuple<bool, byte[]>(false, new byte[0]));
            }
        }

        /// <summary>
        /// 將原來的圖檔做AES加密
        /// </summary>
        /// <param name="param">輸入的圖檔資料</param>
        /// <returns>非同步的任務</returns>
        private async Task AesEncryption(SaveMediaToOssParam param)
        {
            var extRaw = Path.GetExtension(param.FileName);
            var ext = extRaw.ToLower();

            // 更改附檔名
            param.FileName = param.FileName.Replace(ext, ".aes");

            param.Bytes = await AesEncryptionBytes(param.Bytes);
        }

        /// <summary>
        /// 針對內容去做AES加密
        /// </summary>
        /// <param name="bytes">原始內容</param>
        /// <returns>加密後的內容</returns>
        private async Task<byte[]> AesEncryptionBytes(byte[] bytes)
        {
            var resizeByte = bytes;
            if (resizeByte.Length % 16 != 0)
            {
                // 大小不符合 4*4 就調整到可以分成 4*4 的陣列大小
                var size = ((resizeByte.Length / 16) + 1) * 16;
                Array.Resize(ref resizeByte, size);
            }

            // AES 加密
            using (var aes = Aes.Create())
            {
                aes.Key = AesKey;
                aes.BlockSize = 128;
                aes.Mode = CipherMode.ECB;
                aes.Padding = PaddingMode.None;
                // 加密
                ICryptoTransform encryptor = aes.CreateEncryptor(AesKey, new byte[16]);

                // Create the streams used for decryption.
                using (MemoryStream rawStream = new MemoryStream(resizeByte))
                {
                    using (CryptoStream cryptStream = new CryptoStream(rawStream, encryptor, CryptoStreamMode.Read))
                    {
                        using (MemoryStream resultStream = new MemoryStream())
                        {
                            await cryptStream.CopyToAsync(resultStream);
                            return resultStream.ToArray();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 將原來的圖檔做AES加密
        /// </summary>
        /// <param name="param">輸入的圖檔資料</param>
        /// <returns>非同步的任務</returns>
        private async Task AesDecryption(SaveMediaToOssParam param)
        {
            var extRaw = Path.GetExtension(param.FileName);
            var ext = extRaw.ToLower();

            var resizeByte = param.Bytes;
            if (resizeByte.Length % 16 != 0)
            {
                // 大小不符合 4*4 就調整到可以分成 4*4 的陣列大小
                var size = ((resizeByte.Length / 16) + 1) * 16;
                Array.Resize(ref resizeByte, size);
                param.Bytes = resizeByte;
            }

            // 更改附檔名
            param.FileName = param.FileName.Replace(ext, ".jpg");
            if (!string.IsNullOrEmpty(param.FileUrl))
            {
                param.FileUrl = param.FileUrl.Replace(ext, ".jpg");
            }

            // AES 加密
            using (var aes = Aes.Create())
            {
                aes.Key = AesKey;
                aes.BlockSize = 128;
                aes.Mode = CipherMode.ECB;
                aes.Padding = PaddingMode.None;
                // 加密
                ICryptoTransform decryptor = aes.CreateDecryptor(AesKey, new byte[16]);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(param.Bytes))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (MemoryStream resultStream = new MemoryStream())
                        {
                            await csDecrypt.CopyToAsync(resultStream);
                            param.Bytes = resultStream.ToArray();
                        }
                    }
                }
            }
        }

        /// <inheritdoc/>
        public async Task<string> GetFullMediaUrl(MMMedia param, bool isThumbnail = false, PostType postType = PostType.Square)
        {
            if (isThumbnail)
            {
                var extRaw = Path.GetExtension(param.FileUrl);
                var ext = extRaw.ToLower();
                if (ext == ".aes")
                {
                    var url = ConvertToThumbnailUrl(param.FileUrl, postType);
                    if (await _oos.IsObjectExist(ConvertToOosUrl(url)))
                    {
                        // 目前加密的才有縮圖
                        return await _oos.GetCdnPath(ConvertToThumbnailUrl(param.FileUrl, postType));
                    }
                }
            }
            return await _oos.GetCdnPath(param.FileUrl);
        }

        /// <inheritdoc/>
        public async Task Encrypt(DateTime begin, DateTime end, DateTime finish)
        {
            try
            {
                var totalPage = 1;
                var index = 1;
                for (; index <= totalPage; index++)
                {
                    var pageResult = await _repo.GetUnencrypt(Type, SourceType, begin, end, index, 20);
                    var medias = pageResult.Data.Where(x => !string.Equals(Path.GetExtension(x.FileUrl).ToLower(), ".aes")).ToArray();
                    foreach (var media in medias)
                    {
                        await Encrypt(media.Id);
                        await Task.Delay(TimeSpan.FromSeconds(1));
                        _logger.LogInformation("Encrypt {begin}, {end}, {Id} success", begin, end, media.Id);
                    }
                    totalPage = pageResult.TotalPage;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Encrypt Fail");
            }
        }

        /// <summary>
        /// 將原有資訊加密
        /// </summary>
        /// <param name="id">媒體編號</param>
        /// <returns>非同步任務</returns>
        private async Task<BaseReturnModel> Encrypt(string id)
        {
            return await TryCatchProcedure(async (id) =>
            {
                var media = await _repo.Get(id);
                var extRaw = Path.GetExtension(media.FileUrl);
                var ext = extRaw.ToLower();
                if (string.IsNullOrEmpty(ext) || ext.Equals(".aes", StringComparison.OrdinalIgnoreCase))
                {
                    // 圖片已經加密就不做事
                    return new BaseReturnModel(ReturnCode.Success);
                }

                var param = new SaveMediaToOssParam()
                {
                    ModifyDate = DateTime.Now,
                    CreateDate = media.CreateDate,
                    FileUrl = media.FileUrl,
                    Id = id,
                    MediaType = media.MediaType,
                    SourceType = media.SourceType,
                    RefId = media.RefId,
                    FileName = media.FileUrl.Split('/').Last(),
                };

                param.FullMediaUrl = await GetFullMediaUrl(param);
                using (var client = new HttpClient())
                {
                    var resp = await client.GetAsync(param.FullMediaUrl);
                    if (!resp.IsSuccessStatusCode)
                    {
                        _logger.LogError($"Id:{param.Id}, FullMediaUrl:{param.FullMediaUrl} fetch fail");
                        return new BaseReturnModel(ReturnCode.OperationFailed);
                    }
                    param.Bytes = await resp.Content.ReadAsByteArrayAsync();
                }

                // 先上傳新的圖檔
                param.IsNeedEncrypt = true;
                var createResult = await CreateToOss(param);

                if (!createResult.IsSuccess)
                {
                    _logger.LogError($"Encrypt upload fail param:{JsonConvert.SerializeObject(param)}");
                    return createResult;
                }

                if (!await _repo.Update(param))
                {
                    _logger.LogError($"Encrypt update db fail param:{JsonConvert.SerializeObject(param)}");
                    return new BaseReturnModel(ReturnCode.OperationFailed);
                }

                _logger.LogInformation($"Encrypt success, id:{media.Id}");

                return createResult;
            }, id);
        }

        /// <inheritdoc/>
        public async Task Decrypt(DateTime begin, DateTime end, DateTime finish, bool isOnlyThumbnailResize)
        {
            try
            {
                var totalPage = 1;
                var index = 1;
                for (; index <= totalPage; index++)
                {
                    var pageResult = await _repo.GetUnencrypt(Type, SourceType, begin, end, index, 20);
                    var medias = pageResult.Data.Where(x => string.Equals(Path.GetExtension(x.FileUrl).ToLower(), ".aes")).ToArray();
                    foreach (var media in medias)
                    {
                        await Decrypt(media.Id, isOnlyThumbnailResize);
                        await Task.Delay(TimeSpan.FromSeconds(1));
                        _logger.LogInformation("Decrypt {begin}, {end}, {Id} success", begin, end, media.Id);
                    }
                    totalPage = pageResult.TotalPage;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Decrypt Fail");
            }
        }

        /// <summary>
        /// 將原有資訊解密
        /// </summary>
        /// <param name="id">媒體編號</param>
        /// <param name="isOnlyThumbnailResize">只上傳縮圖</param>
        /// <returns>非同步任務</returns>
        private async Task<BaseReturnModel> Decrypt(string id, bool isOnlyThumbnailResize)
        {
            return await TryCatchProcedure(async (id) =>
            {
                var media = await _repo.Get(id);
                var extRaw = Path.GetExtension(media.FileUrl);
                var ext = extRaw.ToLower();
                if (string.IsNullOrEmpty(ext) || !ext.Equals(".aes", StringComparison.OrdinalIgnoreCase))
                {
                    // 圖片已經解密就不做事
                    return new BaseReturnModel(ReturnCode.Success);
                }

                var param = new SaveMediaToOssParam()
                {
                    ModifyDate = DateTime.Now,
                    CreateDate = media.CreateDate,
                    FileUrl = media.FileUrl,
                    Id = id,
                    MediaType = media.MediaType,
                    SourceType = media.SourceType,
                    RefId = media.RefId,
                    FileName = string.Concat(ApplicationSoruce, UploadOssPath, media.FileUrl.Split('/').Last())
                };

                param.FullMediaUrl = await GetFullMediaUrl(param);
                using (var client = new HttpClient())
                {
                    var resp = await client.GetAsync(param.FullMediaUrl);
                    if (!resp.IsSuccessStatusCode)
                    {
                        _logger.LogError($"Id:{param.Id}, FullMediaUrl:{param.FullMediaUrl} fetch fail");
                        return new BaseReturnModel(ReturnCode.OperationFailed);
                    }
                    param.Bytes = await resp.Content.ReadAsByteArrayAsync();
                }
                await AesDecryption(param);

                if (isOnlyThumbnailResize)
                {
                    var thumbnail = await UploadThumbnail(param, PostType.Agency);

                    if (!thumbnail.IsSuccess)
                    {
                        _logger.LogError($"Agency UploadThumbnail fail param:{JsonConvert.SerializeObject(param)}");
                        return thumbnail;
                    }
                    else
                    {
                        thumbnail = await UploadThumbnail(param, PostType.Official);
                        if (!thumbnail.IsSuccess)
                        {
                            _logger.LogError($"Official UploadThumbnail fail param:{JsonConvert.SerializeObject(param)}");
                            return thumbnail;
                        }
                        else
                        {
                            return thumbnail;
                        }
                    }
                }

                // 先上傳新解密的圖檔
                param.IsNeedEncrypt = false;
                var createResult = await CreateToOss(param);

                if (!createResult.IsSuccess)
                {
                    _logger.LogError($"Decrypt upload fail param:{JsonConvert.SerializeObject(param)}");
                    return createResult;
                }

                if (!await _repo.Update(param))
                {
                    _logger.LogError($"Decrypt update db fail param:{JsonConvert.SerializeObject(param)}");
                    return new BaseReturnModel(ReturnCode.OperationFailed);
                }

                _logger.LogInformation($"Decrypt success, id:{media.Id}");

                return createResult;
            }, id);
        }

        /// <inheritdoc/>
        public async Task<BaseReturnModel> NotifyVideoProcess(string mediaId)
        {
            // 圖片不會這麼做
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<BaseReturnDataModel<string>> CreateSplit(SaveMediaToOssParam createParam)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<BaseReturnDataModel<MediaInfo>> CreateMerge(MergeUpload param)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        Task<BaseReturnDataModel<VideoUrlModel>> IMediaService.GetUploadVideoUrl()
        {
            throw new NotImplementedException();
        }
    }
}