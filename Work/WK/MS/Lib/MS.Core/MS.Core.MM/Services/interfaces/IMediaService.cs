using MS.Core.MM.Model.Entities.Media;
using MS.Core.MM.Model.Media;
using MS.Core.MM.Models.Media;
using MS.Core.MMModel.Models;
using MS.Core.MMModel.Models.Media.Enums;
using MS.Core.MMModel.Models.Post.Enums;
using MS.Core.Models;
using MS.Core.Models.Models;

namespace MS.Core.MM.Services.interfaces
{
    /// <summary>
    /// 媒體的資訊
    /// </summary>
    public interface IMediaService
    {
        /// <summary>
        /// 創建媒體資料
        /// </summary>
        /// <param name="param">新增媒體資料的參數</param>
        /// <returns>新增結果</returns>
        Task<BaseReturnModel> Create(SaveMediaToOssParam param);

        /// <summary>
        /// 上傳媒體資料到雲倉儲
        /// </summary>
        /// <param name="param">新增媒體資料的參數</param>
        /// <returns>新增結果</returns>
        Task<BaseReturnModel> CreateToOss(SaveMediaToOssParam param);

        /// <summary>
        /// 刪除媒體資料
        /// </summary>
        /// <param name="seqId">媒體流水號</param>
        /// <returns>刪除的結果</returns>
        Task<BaseReturnModel> Delete(string seqId);

        /// <summary>
        /// 刪除媒體資料
        /// </summary>
        /// <param name="seqId">媒體流水號</param>
        /// <returns>刪除的結果</returns>
        Task<BaseReturnModel> DeleteToOss(string seqId);

        /// <summary>
        /// 更新媒體資料
        /// </summary>
        /// <param name="param">更新媒體資料的參數</param>
        /// <returns>更新結果</returns>
        Task<BaseReturnModel> Update(SaveMediaToOssParam param);

        /// <summary>
        /// 更新媒體資料到Oss
        /// </summary>
        /// <param name="media">更新媒體資料的參數</param>
        /// <returns>更新結果</returns>
        Task<BaseReturnModel> UpdateToOss(SaveMediaToOssParam media);

        /// <summary>
        /// 取得媒體資料
        /// </summary>
        /// <param name="refId">參考的編號</param>
        /// <param name="type">參考的對象</param>
        /// <returns>媒體資料</returns>
        Task<BaseReturnDataModel<MediaInfo[]>> Get(SourceType type, string refId);

        /// <summary>
        /// 取得媒體資料
        /// </summary>
        /// <param name="type">參考的對象</param>
        /// <param name="refIds">參考的編號</param>
        /// <returns>媒體資料</returns>
        Task<BaseReturnDataModel<MediaInfo[]>> Get(SourceType type, string[] refIds);
        /// <summary>
        /// 取得媒體資料
        /// </summary>
        /// <param name="type">參考的對象</param>
        /// <param name="refIds">參考的編號</param>
        /// <returns>媒體資料</returns>
        Task<BaseReturnDataModel<MediaInfo[]>> GetByIds(SourceType type, string[] ids);
        /// <summary>
        /// 多媒體資料是否齊全
        /// </summary>
        /// <param name="param">多媒體資料</param>
        /// <returns>檢查結果</returns>
        Task<BaseReturnModel> CheckParam(SaveMediaToOssParam param);

        /// <summary>
        /// 取得完整的媒體網址
        /// </summary>
        /// <param name="param">媒體資料</param>
        /// <param name="isThumbnail">是否為縮圖</param>
        /// <param name="postType">帖子區域</param>
        /// <returns>完整的媒體網址</returns>
        Task<string> GetFullMediaUrl(MMMedia param, bool isThumbnail = false, PostType postType = PostType.Square);

        /// <summary>
        /// 依照日期來針對位加密的檔案做加密
        /// </summary>
        /// <param name="begin">開始時間</param>
        /// <param name="end">結束時間</param>
        /// <param name="finish">最終時間</param>
        /// <returns>非同步任務</returns>
        Task Encrypt(DateTime begin, DateTime end, DateTime finish);

        /// <summary>
        /// 依照日期來針對位加密的檔案做解密
        /// </summary>
        /// <param name="begin">開始時間</param>
        /// <param name="end">結束時間</param>
        /// <param name="finish">最終時間</param>
        /// <param name="isOnlyThumbnailResize">是否只上傳縮圖</param>
        /// <returns>非同步任務</returns>
        Task Decrypt(DateTime begin, DateTime end, DateTime finish, bool isOnlyThumbnailResize);

        /// <summary>
        /// 上傳縮圖
        /// </summary>
        /// <param name="param">圖片資料</param>
        /// <param name="postType">圖片的位置</param>
        /// <param name="isForceToOss">轉檔失敗就不上傳</param>
        /// <returns>結果</returns>
        Task<BaseReturnModel> UploadThumbnail(SaveMediaToOssParam param, PostType postType, bool isForceToOss = false);


        /// <summary>
        /// 通知ZeroOne做影像處理
        /// </summary>
        /// <param name="req">傳輸參數</param>
        /// <returns>非同步Task</returns>
        Task<BaseReturnModel> NotifyVideoProcess(string mediaId);

        /// <summary>
        /// 分批上傳
        /// </summary>
        /// <param name="createParam">上傳的內容</param>
        /// <returns>回傳的結果及路徑</returns>
        Task<BaseReturnDataModel<string>> CreateSplit(SaveMediaToOssParam createParam);

        /// <summary>
        /// 合併資料
        /// </summary>
        /// <param name="param">合併的參數</param>
        /// <returns>回傳的媒體資料</returns>
        Task<BaseReturnDataModel<MediaInfo>> CreateMerge(MergeUpload param);

        /// <summary>
        /// 取得上傳影片的Url
        /// </summary>
        /// <returns>上傳影片的Url</returns>
        Task<BaseReturnDataModel<VideoUrlModel>> GetUploadVideoUrl();


        /// <inheritdoc cref="Model.Media.Enums.SourceType"/>
        SourceType SourceType { get; }

        /// <inheritdoc cref="MediaType"/>
        MediaType Type { get; }
    }
}