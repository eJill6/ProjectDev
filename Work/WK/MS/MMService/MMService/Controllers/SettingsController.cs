using Microsoft.AspNetCore.Mvc;
using MS.Core.MM.Models.SystemSettings;
using MS.Core.MM.Services.interfaces;
using MS.Core.MMModel.Models;
using MS.Core.MMModel.Models.Post.Enums;
using MS.Core.MMModel.Models.SystemSettings;

namespace MMService.Controllers
{
    /// <summary>
    /// 設定相關
    /// </summary>
    public class SettingsController : ApiControllerBase
    {
        /// <summary>
        /// 設定相關服務
        /// </summary>
        private readonly ISettingsService _settings;

        /// <summary>
        /// 設定相關
        /// </summary>
        /// <param name="logger">log</param>
        /// <param name="settings">設定相關服務</param>
        public SettingsController(ILogger<SettingsController> logger,
            ISettingsService settings) : base(logger)
        {
            _settings = settings;
        }

        /// <summary>
        /// 取得發贴頁面的所有選項設定
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<PostTypeOptions>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ByPostType(PostType postType)
        {
            return ApiResult(await TryCatchProcedure(async (param) =>
            {
                return await _settings.GetOptionItemByPostType(param);
            }, postType));
        }

        /// <summary>
        /// 取得價格設定
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<OptionItem[]>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Price([FromQuery] PostType postType)
        {
            return ApiResult(await TryCatchProcedure(async (param) =>
            {
                return await _settings.Price(postType);
            }, postType));
        }

        /// <summary>
        /// 取得訊息種類設定
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<OptionItem[]>), StatusCodes.Status200OK)]
        public async Task<IActionResult> MessageType(PostType postType)
        {
            return ApiResult(await TryCatchProcedure(async (param) =>
            {
                return await _settings.MessageType(param);
            }, postType));
        }

        /// <summary>
        /// 取得服務項目設定
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<OptionItem[]>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Services(PostType postType)
        {
            return ApiResult(await TryCatchProcedure(async (param) =>
            {
                return await _settings.Services(param);
            }, postType));
        }

        /// <summary>
        /// 取得年齡項目設定
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<OptionItem[]>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Age()
        {
            return ApiResult(await TryCatchProcedure(async (param) =>
            {
                return await _settings.Age();
            }, string.Empty));
        }

        /// <summary>
        /// 取得身高項目設定
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<OptionItem[]>), StatusCodes.Status200OK)]
        public async Task<IActionResult> BodyHeight()
        {
            return ApiResult(await TryCatchProcedure(async (param) =>
            {
                return await _settings.BodyHeight();
            }, string.Empty));
        }

        /// <summary>
        /// 取得cup項目設定
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<OptionItem[]>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Cup()
        {
            return ApiResult(await TryCatchProcedure(async (param) =>
            {
                return await _settings.Cup();
            }, string.Empty));
        }

        /// <summary>
        /// 取得管理者聯繫方式
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<AdminContact>), StatusCodes.Status200OK)]
        public async Task<IActionResult> AdminContact()
        {
            return ApiResult(await TryCatchProcedure(async () =>
            {
                return await _settings.GetAdminContact();
            }));
        }

        /// <summary>
        /// 取得篩選頁選項設定
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<PostFilterOptions>), StatusCodes.Status200OK)]
        public async Task<IActionResult> PostFilterOptions(PostType? postType)
        {
            return ApiResult(await TryCatchProcedure(async (postType) =>
            {
                return await _settings.GetPostFilterOptions(postType);
            }, postType));
        }

        /// <summary>
        /// 取得预设解锁价格，从 PostType
        /// </summary>
        /// <param name="postType">贴的类型</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<PostTypeUnlockAmount>), StatusCodes.Status200OK)]
        public async Task<IActionResult> BaseUnlockAmountByType(PostType postType)
        {
            PostTypeUnlockAmount? result = MMGlobalSettings.BaseUnlockAmountSetting
                .Where(p => p.PostType == postType)
                .FirstOrDefault();

            if (result == null)
            {
                return ApiFailureResult(string.Empty);
            }
            else
            {
                return ApiSuccessResult(await Task.FromResult(result));
            }
        }

        /// <summary>
        /// 取得所有的预设解锁价格
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<PostTypeUnlockAmount[]>), StatusCodes.Status200OK)]
        public async Task<IActionResult> AllBaseUnlockAmount()
        {
            var result = await Task.FromResult(MMGlobalSettings.BaseUnlockAmountSetting);
            return ApiSuccessResult(result);
        }
    }
}