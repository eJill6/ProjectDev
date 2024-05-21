using MS.Core.Infrastructures.ZeroOne.Models.Requests;
using MS.Core.Infrastructures.ZeroOne.Models.Responses;
using MS.Core.Models;
using MS.Core.Models.Models;

namespace MS.Core.Infrastructures.ZoneOne
{
    public interface IZeroOneApiService
    {
        /// <summary>
        /// 取得玩家資料
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<BaseReturnDataModel<ZOUserInfoRes>> GetUserInfo(ZOUserInfoReq req);
        /// <summary>
        /// 鑽石消費(支出)
        /// </summary>
        /// <param name="thirdExpense"></param>
        /// <returns></returns>
        Task<BaseReturnModel> PointExpense(ZOPointIncomeExpenseReq thirdExpense);
        /// <summary>
        /// 鑽石消費(收入)
        /// </summary>
        /// <param name="thirdExpense"></param>
        /// <returns></returns>
        Task<BaseReturnModel> PointIncome(ZOPointIncomeExpenseReq thirdExpense);
        /// <summary>
        /// 現金消費(支出)
        /// </summary>
        /// <param name="thirdExpense"></param>
        /// <returns></returns>
        Task<BaseReturnModel> CashExpense(ZOCashIncomeExpenseReq req);
        /// <summary>
        /// 現金收益(收入)
        /// </summary>
        /// <param name="thirdExpense"></param>
        /// <returns></returns>
        Task<BaseReturnModel> CashIncome(ZOCashIncomeExpenseReq thirdExpense);

        /// <summary>
        /// 上傳影片
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<BaseReturnDataModel<string>> MediaUpload(ZOMediaUploadReq req);

        /// <summary>
        /// 分批上傳影片
        /// </summary>
        /// <param name="req"></param>
        /// <returns>路徑</returns>
        Task<BaseReturnDataModel<string>> MediaSplitUpload(ZOMediaUploadReq req);

        /// <summary>
        /// 分批上傳影片
        /// </summary>
        /// <param name="paths">所有的路徑</param>
        /// <param name="suffix">副檔名</param>
        /// <returns>完整的路徑</returns>
        Task<BaseReturnDataModel<string>> MediaMergeUpload(string[] paths, string suffix);

        /// <summary>
        /// 取得完整路徑
        /// </summary>
        /// <param name="fileUrl">相對路徑</param>
        /// <returns>完整路徑</returns>
        Task<string> GetFullMediaUrl(string fileUrl);

        /// <summary>
        /// 通知ZeroOne做影像處理
        /// </summary>
        /// <param name="fileUrl">相對路徑</param>
        /// <param name="path">原始檔案路徑</param>
        /// <param name="id">會回通知是哪一筆資料在處理</param>
        /// <param name="orientation">1:横屏、2:竖屏</param>
        /// <returns>非同步Task</returns>
        Task NotifyVideoProcess(string path, string id, int orientation);

        /// <summary>
        /// 取得上傳影片的Url
        /// </summary>
        /// <returns>上傳影片的Url</returns>
        Task<BaseReturnDataModel<VideoUrlModel>> GetUploadVideoUrl();

        /// <summary>
        /// 取得權限
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<BaseReturnDataModel<bool>> GetPermission(ZOVipPermissionReq req);
    }

}