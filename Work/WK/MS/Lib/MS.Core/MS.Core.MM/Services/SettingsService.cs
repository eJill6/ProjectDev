using Microsoft.Extensions.Logging;
using MS.Core.MM.Models.Entities.Post;
using MS.Core.MM.Models.SystemSettings;
using MS.Core.MM.Repos.interfaces;
using MS.Core.MM.Services.interfaces;
using MS.Core.MMModel.Extensions;
using MS.Core.MMModel.Models;
using MS.Core.MMModel.Models.Post.Enums;
using MS.Core.Models;
using MS.Core.Services;

namespace MS.Core.MM.Services
{
    /// <summary>
    /// 設定相關
    /// </summary>
    public class SettingsService : BaseService, ISettingsService
    {
        /// <summary>
        /// 系統設定資源
        /// </summary>
        private readonly IOptionItemRepo _optionRepo;

        /// <summary>
        ///
        /// </summary>
        private readonly IAdvertisingContentRepo _advertisingRepo;

        /// <summary>
        /// 基本解鎖的
        /// </summary>
        public readonly decimal BaseUnlockAmount = 100M;

        /// <summary>
        /// 年齡篩選設定
        /// </summary>
        private static Dictionary<string, int[]> AgeFilter => new Dictionary<string, int[]>()
        {
            { "18-21", new int[]{(int)AgeDefined.Y18,
                (int)AgeDefined.Y19,
                (int)AgeDefined.Y20,
                (int)AgeDefined.Y21 } },
            { "21-24", new int[]{(int)AgeDefined.Y21,
                (int)AgeDefined.Y22,
                (int)AgeDefined.Y23,
                (int)AgeDefined.Y24 } },
            { "24-27", new int[]{(int)AgeDefined.Y24,
                (int)AgeDefined.Y25,
                (int)AgeDefined.Y26,
                (int)AgeDefined.Y27 } },
            { "28以上", new int[]{(int)AgeDefined.Y28,
                (int)AgeDefined.Y_Plus } }
        };

        /// <summary>
        /// 身高篩選設定
        /// </summary>
        private static Dictionary<string, int[]> BodyHeightFilter => new Dictionary<string, int[]>()
        {
            { "150-155", new int[]{(int)BodyHeightDefined.H150,
                (int)BodyHeightDefined.H151,
                (int)BodyHeightDefined.H152,
                (int)BodyHeightDefined.H153,
                (int)BodyHeightDefined.H154,
                (int)BodyHeightDefined.H155 } },
            { "155-160", new int[]{(int)BodyHeightDefined.H155,
                (int)BodyHeightDefined.H156,
                (int)BodyHeightDefined.H157,
                (int)BodyHeightDefined.H158,
                (int)BodyHeightDefined.H159,
                (int)BodyHeightDefined.H160 } },
            { "160-165", new int[]{(int)BodyHeightDefined.H160,
                (int)BodyHeightDefined.H161,
                (int)BodyHeightDefined.H162,
                (int)BodyHeightDefined.H163,
                (int)BodyHeightDefined.H164,
                (int)BodyHeightDefined.H165 } },
            { "165-170", new int[]{(int)BodyHeightDefined.H165,
                (int)BodyHeightDefined.H166,
                (int)BodyHeightDefined.H167,
                (int)BodyHeightDefined.H168,
                (int)BodyHeightDefined.H169,
                (int)BodyHeightDefined.H170 } },
            { "170-175", new int[]{(int)BodyHeightDefined.H170,
                (int)BodyHeightDefined.H171,
                (int)BodyHeightDefined.H172,
                (int)BodyHeightDefined.H173,
                (int)BodyHeightDefined.H174,
                (int)BodyHeightDefined.H175 } },
            { "175以上", new int[]{(int)BodyHeightDefined.H_Plus } }
        };

        /// <summary>
        /// 價格篩選設定
        /// </summary>
        private static Dictionary<string, PriceLowAndHigh> PriceFilter => new Dictionary<string, PriceLowAndHigh>()
        {
            { "500以下", new PriceLowAndHigh() { Low = 0, High = 500 } },
            { "500-1000", new PriceLowAndHigh() { Low = 500, High = 1000 } },
            { "1000-2000", new PriceLowAndHigh() { Low = 1000, High = 2000 } },
            { "2000以上", new PriceLowAndHigh() { Low = 2000, High = 0 } }
        };

        /// <summary>
        /// 系統設定資源
        /// </summary>
        /// <param name="logger">log</param>
        /// <param name="optionRepo">選項資源</param>
        /// <param name="advertisingRepo">宣傳設定</param>
        public SettingsService(ILogger logger,
            IOptionItemRepo optionRepo,
            IAdvertisingContentRepo advertisingRepo) : base(logger)
        {
            _optionRepo = optionRepo;
            _advertisingRepo = advertisingRepo;
        }

        private Func<MMOptions[], OptionType, OptionItem[]> FilterOptionItems =
            (data, optionType) =>
            {
                return data?.Where(x => x.OptionType == (byte)optionType)?
                    .Select(x => new OptionItem
                    {
                        Key = x.OptionId,
                        Value = x.OptionContent
                    })?.ToArray() ?? new OptionItem[0];
            };

        public async Task<BaseReturnDataModel<PostTypeOptions>> GetOptionItemByPostType(PostType postType)
        {
            return await base.TryCatchProcedure(async (param) =>
            {
                var optionData = await _optionRepo.GetOptionByPostType((int)postType);
                var options = new PostTypeOptions()
                {
                    Price = FilterOptionItems(optionData, OptionType.ApplyAdjustPrice),
                    MessageType = FilterOptionItems(optionData, OptionType.Message),
                    Service = FilterOptionItems(optionData, OptionType.Service),
                    Age = Enum.GetValues(typeof(AgeDefined)).Cast<AgeDefined>().Select(p => new OptionItem
                    {
                        Key = (int)p,
                        Value = p.GetDescription()
                    }).ToArray(),
                    BodyHeight = Enum.GetValues(typeof(BodyHeightDefined)).Cast<BodyHeightDefined>().Select(p => new OptionItem
                    {
                        Key = (int)p,
                        Value = p.GetDescription()
                    }).ToArray(),
                    Cup = Enum.GetValues(typeof(CupDefined)).Cast<CupDefined>().Select(p => new OptionItem
                    {
                        Key = (int)p,
                        Value = p.GetDescription()
                    }).ToArray()
                };

                options.Price = options.Price
                    .Where(p =>
                    {
                        return decimal.TryParse(p.Value, out decimal val);
                    })
                    .OrderBy(p =>
                    {
                        return Convert.ToDecimal(p.Value);
                    }).ToArray();

                var result = new BaseReturnDataModel<PostTypeOptions>();
                result.DataModel = options;
                result.SetCode(ReturnCode.Success);

                return await Task.FromResult(result);
            }, postType);
        }

        /// <summary>
        /// 插入基本的價格到第一筆
        /// </summary>
        /// <param name="options"></param>
        private OptionItem[] InsertBasePriceSetting(OptionItem[] options)
        {
            var priceOption = options.ToList();
            priceOption.Insert(0, new OptionItem { Key = 0, Value = BaseUnlockAmount.ToString() });

            return priceOption.ToArray();
        }

        /// <summary>
        /// 取得價格設定
        /// </summary>
        /// <returns></returns>
        public async Task<BaseReturnDataModel<OptionItem[]>> Price(PostType postType)
        {
            return await TryCatchProcedure(async (param) =>
            {
                var result = await GetOptionItem(param, OptionType.ApplyAdjustPrice);
                result.DataModel = result.DataModel
                    .Where(p =>
                    {
                        return decimal.TryParse(p.Value, out decimal val);
                    })
                    .OrderBy(p =>
                    {
                        return Convert.ToDecimal(p.Value);
                    }).ToArray();

                return await Task.FromResult(result);
            }, postType);
        }

        /// <summary>
        /// 取得訊息種類設定
        /// </summary>
        /// <returns></returns>
        public async Task<BaseReturnDataModel<OptionItem[]>> MessageType(PostType postType)
        {
            return await TryCatchProcedure(async (param) =>
            {
                return await Task.FromResult(await GetOptionItem(param, OptionType.Message));
            }, postType);
        }

        /// <summary>
        /// 取得服務項目設定
        /// </summary>
        /// <returns></returns>
        public async Task<BaseReturnDataModel<OptionItem[]>> Services(PostType postType)
        {
            return await TryCatchProcedure(async (param) =>
            {
                return await Task.FromResult(await GetOptionItem(param, OptionType.Service));
            }, postType);
        }

        /// <summary>
        /// 取得年齡設定項
        /// </summary>
        /// <returns></returns>
        public async Task<BaseReturnDataModel<OptionItem[]>> Age()
        {
            return await TryCatchProcedure(async (param) =>
            {
                var result = new BaseReturnDataModel<OptionItem[]>();
                result.SetCode(ReturnCode.Success);
                result.DataModel = Enum.GetValues(typeof(AgeDefined)).Cast<AgeDefined>().Select(p => new OptionItem
                {
                    Key = (int)p,
                    Value = p.GetDescription()
                }).ToArray();

                return await Task.FromResult(result);
            }, string.Empty);
        }

        /// <summary>
        /// 身高設定項
        /// </summary>
        /// <returns></returns>
        public async Task<BaseReturnDataModel<OptionItem[]>> BodyHeight()
        {
            return await TryCatchProcedure(async (param) =>
            {
                var result = new BaseReturnDataModel<OptionItem[]>();
                result.SetCode(ReturnCode.Success);
                result.DataModel = Enum.GetValues(typeof(BodyHeightDefined)).Cast<BodyHeightDefined>().Select(p => new OptionItem
                {
                    Key = (int)p,
                    Value = p.GetDescription()
                }).ToArray();

                return await Task.FromResult(result);
            }, string.Empty);
        }

        /// <summary>
        /// Cup 設定項
        /// </summary>
        /// <returns></returns>
        public async Task<BaseReturnDataModel<OptionItem[]>> Cup()
        {
            return await TryCatchProcedure(async (param) =>
            {
                var result = new BaseReturnDataModel<OptionItem[]>();
                result.SetCode(ReturnCode.Success);
                result.DataModel = Enum.GetValues(typeof(CupDefined)).Cast<CupDefined>().Select(p => new OptionItem
                {
                    Key = (int)p,
                    Value = p.GetDescription()
                }).ToArray();

                return await Task.FromResult(result);
            }, string.Empty);
        }

        /// <summary>
        /// 取得管理員帳號
        /// </summary>
        /// <returns></returns>
        public async Task<BaseReturnDataModel<AdminContact>> GetAdminContact()
        {
            return await TryCatchProcedure(async (param) =>
            {
                var result = new BaseReturnDataModel<AdminContact>();
                result.DataModel = new AdminContact
                {
                    Contact = await _advertisingRepo.GetAdminContact()
                };
                result.SetCode(ReturnCode.Success);

                return await Task.FromResult(result);
            }, string.Empty);
        }

        /// <summary>
        /// 取得Tip
        /// </summary>
        /// <returns></returns>
        public async Task<BaseReturnDataModel<string>> GetTip(PostType postType)
        {
            return await TryCatchProcedure(async (param) =>
            {
                var result = new BaseReturnDataModel<string>();
                result.DataModel = await _advertisingRepo.GetTip(postType);
                result.SetCode(ReturnCode.Success);

                return await Task.FromResult(result);
            }, string.Empty);
        }

        /// <summary>
        /// 取得篩選條件
        /// </summary>
        /// <param name="postType">發贴類型</param>
        /// <returns></returns>
        public async Task<BaseReturnDataModel<PostFilterOptions>> GetPostFilterOptions(PostType? postType)
        {
            return await TryCatchProcedure(async (postType) =>
            {
                var optionData = await _optionRepo.GetOptionByPostType((int)(postType ?? PostType.Square));

                var result = new BaseReturnDataModel<PostFilterOptions>();
                result.DataModel = new PostFilterOptions()
                {
                    Age = AgeFilter,
                    Height = BodyHeightFilter,
                    Price = PriceFilter,
                    Service = FilterOptionItems(optionData, OptionType.Service),
                    Cup = Enum.GetValues(typeof(CupDefined)).Cast<CupDefined>().Select(p => new OptionItem
                    {
                        Key = (int)p,
                        Value = $"{p.GetDescription()}罩杯"
                    }).ToArray()
                };
                result.SetCode(ReturnCode.Success);

                return await Task.FromResult(result);
            }, postType);
        }

        /// <summary>
        /// 取得選項項目
        /// </summary>
        /// <param name="postType">發贴類型</param>
        /// <param name="optionType">選項類型</param>
        /// <returns></returns>
        private async Task<BaseReturnDataModel<OptionItem[]>> GetOptionItem(PostType postType, OptionType optionType)
        {
            var result = new BaseReturnDataModel<OptionItem[]>();
            result.SetCode(ReturnCode.Success);
            try
            {
                var optionData = await _optionRepo.GetOptionByPostType((int)postType);
                result.DataModel = FilterOptionItems(optionData, optionType);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"取得選項出現異常。Ex：{ex.Message}");
            }

            return result;
        }
    }
}