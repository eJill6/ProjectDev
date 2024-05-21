using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MS.Core.Infrastructures.DBTools.Models;
using MS.Core.Infrastructures.Providers;
using MS.Core.MM.Models.Entities.Post;
using MS.Core.MM.Models.Post;
using MS.Core.MM.Repos.interfaces;
using MS.Core.MMModel.Models;
using MS.Core.MMModel.Models.Post.Enums;
using MS.Core.MMModel.Models.SystemSettings;
using MS.Core.Models;
using MS.Core.Repos;
using MS.Core.MM.Extensions;

namespace MS.Core.MM.Repos
{
    public class OptionItemRepo : BaseInlodbRepository, IOptionItemRepo
    {
        public OptionItemRepo(IOptionsMonitor<MsSqlConnections> setting, IRequestIdentifierProvider provider, ILogger logger) : base(setting, provider, logger)
        {
        }

        /// <summary>
        /// 從頁面獲取選項
        /// </summary>
        /// <param name="postType"></param>
        /// <returns></returns>
        public async Task<MMOptions[]> GetOptionByPostType(int postType)
        {
            return (await ReadDb.QueryTable<MMOptions>()
                .Where(x => x.PostType == postType && x.IsActive == true)
                .QueryAsync())?.ToArray() ?? Array.Empty<MMOptions>();
        }

        /// <summary>
        /// 從頁面獲取選項(後台使用)
        /// </summary>
        public async Task<MMOptions[]> GetOptionsByPostTypeAndOptionType(PostType postType, OptionType optionType, int? OptionId)
        {
            var query = ReadDb.QueryTable<MMOptions>();
            if (postType != 0)
            {
                query = query.Where(x => x.PostType == (int)postType);
            }
            if (optionType != 0)
            {
                query = query.Where(x => x.OptionType == (int)optionType);
            }
            if (OptionId != null)
            {
                query = query.Where(x => x.OptionId == OptionId);
            }
            return (await query.QueryAsync())
                .OrderBy(x => x.PostType)
                .ThenBy(x => x.OptionType)
                .ToArray();
        }

        public async Task<DBResult> Create(CreateOptionsParam param)
        {
            var optionType = param.OptionType;
            var postType = param.PostType;

            var validationError = await ValidateAndProcessOption((OptionType)optionType, (PostType)postType, param.OptionContent);
            if (validationError != null)
                return validationError;

            await WriteDb.Insert(param).SaveChangesAsync();
            return new DBResult(ReturnCode.Success);
        }

        public async Task Delete(int optionId)
        {
            await WriteDb.Delete(new MMOptions()
            {
                OptionId = optionId,
            }).SaveChangesAsync();
        }

        public async Task<DBResult> Update(UpdateOptionsParam param)
        {
            var optionType = param.OptionType;
            var postType = param.PostType;

            var validationError = await ValidateAndProcessOptionForUpdate((OptionType)optionType, (PostType)postType, param.OptionContent, param.OptionId);
            if (validationError != null)
                return validationError;

            string sql = @"UPDATE [Inlodb].[dbo].[MMOptions]
                   SET [ModifyDate] = GETDATE(),
                   [IsActive] = @IsActive,
                   [OptionContent]=@OptionContent
                   WHERE [OptionId] = @OptionId;";

            await WriteDb.AddExecuteSQL(sql, param).SaveChangesAsync();
            return new DBResult(ReturnCode.Success);
        }

        /// <summary>
        /// 取得選項內容
        /// </summary>
        /// <param name="postType">發贴類型</param>
        /// <returns></returns>
        public async Task<Dictionary<int, string>> GetPostTypeOptions(PostType? postType)
        {
            string sql = @"
                SELECT OptionId,
                    OptionContent
                FROM [Inlodb].[dbo].[MMOptions] WITH(NOLOCK)";

            if (postType != null)
            {
                sql += " WHERE [PostType] = @PostType; ";
            }

            return (await ReadDb.QueryAsync<MMOptions>(sql, paras: (postType == null ? null : new { PostType = postType })))
                .ToDictionary(p => p.OptionId, p => p.OptionContent);
        }

        /// <summary>
        /// 取得選項內容
        /// </summary>
        /// <param name="postType">發贴類型</param>
        /// <returns></returns>
        public async Task<MMOptions[]> GetOptionsByPostTypes(List<int> postTypes)
        {
            string sql = $@"
                SELECT *
                FROM [Inlodb].[dbo].[MMOptions] WITH(NOLOCK) WHERE [PostType] IN ({string.Join(",", postTypes)});";
            return (await ReadDb.QueryAsync<MMOptions>(sql)).ToArray();
        }

        public bool IsAmountThanMin(PostType optionType, decimal amount, out decimal minAmount)
        {
            PostTypeUnlockAmount? result = MMGlobalSettings.BaseUnlockAmountSetting
              .Where(p => p.PostType == optionType).FirstOrDefault() ?? new PostTypeUnlockAmount();
            minAmount = result.UnlockAmount;
            if (amount >= minAmount)
            {
                return true;
            }
            return false;
        }

        public bool IsAmountDuplicated(PostType postType, string amount)
        {
            var data = GetOptionsByPostTypeAndOptionType(postType, OptionType.ApplyAdjustPrice, null);
            var amountList = data.Result.Select(p => p.OptionContent);
            if (amountList.Contains(amount.ToString()))
            {
                return true;
            }
            return false;
        }

        public bool IsAmountDuplicatedForUpdate(PostType postType, string amount, int optionId)
        {
            var data = GetOptionsByPostTypeAndOptionType(postType, OptionType.ApplyAdjustPrice, null);
            var amountList = data.Result.Where(d => d.OptionId != optionId && d.OptionContent == amount).Count();
            if (amountList > 0)
            {
                return true;
            }
            return false;
        }

        private async Task<DBResult> ValidateAndProcessOption(OptionType optionType, PostType postType, string optionContent)
        {
            var result = new DBResult();
            decimal minAmount;

            if (optionType == OptionType.ApplyAdjustPrice)
            {
                if (!IsAmountThanMin(postType, Convert.ToDecimal(optionContent), out minAmount))
                {
                    result.Code = "OP0001";
                    result.Msg = String.Format("{0}贴申请调价不可小于{1}", postType.GetEnumDescription(), minAmount);
                    return result;
                }

                if (IsAmountDuplicated(postType, optionContent))
                {
                    result.Code = "OP0002";
                    result.Msg = String.Format("{0}贴申请调价重复", postType.GetEnumDescription());
                    return result;
                }
            }

            return null;
        }

        private async Task<DBResult> ValidateAndProcessOptionForUpdate(OptionType optionType, PostType postType, string optionContent, int optionId)
        {
            var result = new DBResult();
            decimal minAmount;

            if (optionType == OptionType.ApplyAdjustPrice)
            {
                if (!IsAmountThanMin(postType, Convert.ToDecimal(optionContent), out minAmount))
                {
                    result.Code = "OP0001";
                    result.Msg = String.Format("{0}贴申请调价不可小于{1}", postType.GetEnumDescription(), minAmount);
                    return result;
                }

                if (IsAmountDuplicatedForUpdate(postType, optionContent, optionId))
                {
                    result.Code = "OP0002";
                    result.Msg = String.Format("{0}贴申请调价重复", postType.GetEnumDescription());
                    return result;
                }
            }

            return null;
        }
    }
}