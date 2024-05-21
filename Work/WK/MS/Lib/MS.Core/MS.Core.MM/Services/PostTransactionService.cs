using Microsoft.Extensions.Logging;
using MS.Core.Extensions;
using MS.Core.Infrastructures.Providers;
using MS.Core.Infrastructures.ZeroOne.Models.Requests;
using MS.Core.Infrastructures.ZoneOne;
using MS.Core.MM.Extensions;
using MS.Core.MM.Infrastructures.Exceptions;
using MS.Core.MM.Models.Entities.Post;
using MS.Core.MM.Models.Entities.PostTransaction;
using MS.Core.MM.Models.Entities.User;
using MS.Core.MM.Models.IncomeExpense;
using MS.Core.MM.Models.Post;
using MS.Core.MM.Models.User;
using MS.Core.MM.Repos.interfaces;
using MS.Core.MM.Services.interfaces;
using MS.Core.MMModel.Extensions;
using MS.Core.MMModel.Models;
using MS.Core.MMModel.Models.IncomeExpense;
using MS.Core.MMModel.Models.Post.Enums;
using MS.Core.Models;

namespace MS.Core.MM.Services
{
    public class PostTransactionService : BaseTransactionService, IPostTransactionService
    {
        public PostTransactionService(
            IRequestIdentifierProvider provider,
            ILogger logger,
            IVipTransactionRepo vipTransactionRepo,
            IVipWelfareService vipWelfareService,
            IUserInfoRepo userInfoRepo,
            IZeroOneApiService zeroOneApiService,
            IPostRepo postRepo,
            IUserSummaryService userSummaryService,
            IVipService vipServic,
            IDateTimeProvider dateTimeProvider,
            IPostTransactionRepo postTransactionRepository) : base(provider, logger, vipTransactionRepo, vipWelfareService, userInfoRepo, zeroOneApiService, dateTimeProvider)
        {
            PostRepo = postRepo;
            PostTransactionRepository = postTransactionRepository;
            UserSummaryService = userSummaryService;
            VipServic = vipServic;
        }
        IVipService VipServic { get; }
        IUserSummaryService UserSummaryService { get; }
        IPostRepo PostRepo { get; }
        IPostTransactionRepo PostTransactionRepository { get; }

        public async Task<BaseReturnDataModel<ResPostTransaction>> PostTransaction(ReqPostTransaction res)
        {
            return await base.TryCatchProcedure(async (resPostTrans) =>
            {
                MMPost? post = await PostRepo.GetById(resPostTrans.PostId);

                if (post == null)
                {
                    throw new MMException(ReturnCode.DataIsNotExist, "该贴子不存在");
                }

                if (post.Status != ReviewStatus.Approval)
                {
                    throw new MMException(ReturnCode.DataNotUse, "该贴子不可解锁");
                }

                if (await PostTransactionRepository.IsUserBuyPost(resPostTrans.UserId, resPostTrans.PostId))
                {
                    throw new MMException(ReturnCode.DataIsExist, "已解锁贴子");
                }

                IEnumerable<MMUserInfo> users = await UserInfoRepo.GetUserInfos(new int[] { resPostTrans.UserId, post.UserId });

                MMUserInfo? expenseUser = users.FirstOrDefault(e => e.UserId == resPostTrans.UserId);
                MMUserInfo? incomeUser = users.FirstOrDefault(e => e.UserId == post.UserId);

                if (expenseUser == null || incomeUser == null)
                {
                    throw new MMException(ReturnCode.NotMember, "資料異常");
                }

                //是否申請調價
                post.UnlockAmount = post.ApplyAdjustPrice ? post.ApplyAmount : post.UnlockAmount;

                UserInfoData expenseUserInfoData = await VipServic.GetUserInfoData(expenseUser);

                if (await IsFreeUnlockPost(expenseUserInfoData, post.PostType))
                {
                    UseUserPointUnlockPostModel useUserPointUnlockPost =
                        await GetUseUserPointUnlockPostModel(expenseUserInfoData, incomeUser, post);

                    await PostTransactionRepository.UseUserPointUnlockPost(useUserPointUnlockPost);

                    return await GetResPostTransaction(post, true);
                }

                UnlockPostInfoModel unlockPost = await GetUnlockPostModel(expenseUserInfoData, incomeUser, post, post.UnlockAmount);

                await ZeroOneUnlockPost(unlockPost);

                await PostTransactionRepository.UnlockPost(unlockPost);
                return await GetResPostTransaction(post, false);
            }, res);

            async Task<BaseReturnDataModel<ResPostTransaction>> GetResPostTransaction(MMPost post,bool isFree)
            {
                //取得贴相關的聯絡方式
                var contacts = await PostRepo.GetPostContact(new string[] { post.PostId });

                return new BaseReturnDataModel<ResPostTransaction>(ReturnCode.Success)
                {
                    DataModel = new ResPostTransaction
                    {
                        IsFree = isFree,
                        UnlockInfo = new UserUnlockGetInfo
                        {
                            Address = post.Address,
                            ContactInfos =
                                EnumExtension.GetAll<ContactType>()
                                .Select(type => new ContactInfo
                                {
                                    ContactType = type,
                                    Contact = contacts?.FirstOrDefault(x => x.ContactType == (byte)type)?.Contact ?? string.Empty
                                }).ToArray() ?? Array.Empty<ContactInfo>(),
                        }
                    }
                };
            }
        }

        /// <summary>
        /// 呼叫 01 解鎖扣款
        /// </summary>
        /// <param name="unlockPost"></param>
        /// <returns></returns>
        /// <exception cref="MMException"></exception>
        private async Task ZeroOneUnlockPost(UnlockPostInfoModel unlockPost)
        {
            ZOPointIncomeExpenseReq req =
                                new(ZOIncomeExpenseCategory.UnlockPost, unlockPost.Expense.UserId, unlockPost.ExpenseData.DiscountAmount, unlockPost.Expense.Id);

            var zeroOneResult = await ZeroOneApiService.PointExpense(req);

            if (zeroOneResult.IsSuccess == false)
            {
                throw new MMException(ReturnCode.ThirdPartyApiNotSuccess, zeroOneResult.Message);
            }
        }

        /// <summary>
        /// 判斷是否免費解鎖
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private async Task<bool> IsFreeUnlockPost(UserInfoData user, PostType postType)
        {
            var vip = user.CurrentVip;
            if (vip == null || user.VipWelfares.IsEmpty())
            {
                return false;
            }

            //免費解鎖授權
            if (user.VipWelfares.Where(e => e.Type == VIPWelfareTypeEnum.FreeUnlockAuth 
                    && e.Category == postType.ConvertToVIPWelfareCategory() && e.Value == 1M).Any() == false)
            {
                return false;
            }

            UserSummaryCategoryEnum userSummary = vip.VipType.ConvertToUserSummaryCategory();

            decimal userUsePoint =
                await UserSummaryService
                .GetUserAmount(user.UserInfo.UserId, UserSummaryTypeEnum.FreeUnlock, userSummary)
                .GetReturnDataAsync();

            bool isFree = userUsePoint < user.FreeUnlock;

            return isFree;
        }

        /// <summary>
        /// 免費解鎖贴子
        /// </summary>
        /// <param name="expenseUser"></param>
        /// <param name="incomeUser"></param>
        /// <param name="post"></param>
        /// <returns></returns>
        private async Task<UseUserPointUnlockPostModel> GetUseUserPointUnlockPostModel(UserInfoData expenseUser, MMUserInfo incomeUser, MMPost post)
        {
            UnlockPostInfoModel model = await GetUnlockPostModel(expenseUser, incomeUser, post, 0, true);
            
            return new UseUserPointUnlockPostModel
            {
                Expense = model.Expense,
                Income = model.Income,
                PostTransaction = model.PostTransaction,
                UserPointExpense = new UserPointExpenseModel
                {
                    Point = 1,
                    PostType = post.PostType,
                    VipType = expenseUser.CurrentVip!.VipType,
                },
                ExpenseData = model.ExpenseData,
            };
        }
        /// <summary>
        /// 解鎖贴子相關 Model
        /// </summary>
        /// <param name="expenseUser"></param>
        /// <param name="incomeUser"></param>
        /// <param name="post"></param>
        /// <param name="expenseAmount"></param>
        /// <returns></returns>
        private async Task<UnlockPostInfoModel> GetUnlockPostModel(UserInfoData expenseUser, MMUserInfo incomeUser, MMPost post, decimal expenseAmount, bool isFree = false)
        {
            PostType postType = post.PostType;

            IncomeExpenseCategoryEnum incomeExpenseCategory = postType.ConvertToIncomeExpenseCategory();

            MMPostTransactionModel postTransaction = new()
            {
                CreateTime = DateTimeProvider.Now,
                UserId = expenseUser.UserInfo.UserId,
                PostId = post.PostId,
                Id = await PostTransactionRepository.GetSequenceIdentity<MMPostTransactionModel>(),
                PostType = postType,
                PostUserId = post.UserId,
            };

            decimal rebate = 0.1M * 0.6M;//0.1是鑽石兌換匯率，0.6 根據身分變動

            ExpenseData expenseData = GetExpenseData(expenseUser, post.PostType, expenseAmount, isFree);

            string incomeId = await PostTransactionRepository.GetSequenceIdentity<MMIncomeExpenseModel>();
            string expenseId = await PostTransactionRepository.GetSequenceIdentity<MMIncomeExpenseModel>();

            MMIncomeExpenseModel income = new()
            {
                Amount = expenseData.DiscountAmount,
                Rebate = rebate,
                Category = incomeExpenseCategory,
                CreateTime = DateTimeProvider.Now,
                Id = incomeId,
                TransactionType = IncomeExpenseTransactionTypeEnum.Income,
                SourceId = postTransaction.Id,
                Title = post.Title,
                UserId = incomeUser.UserId,
                Status = IncomeExpenseStatusEnum.UnDispatched,
                DistributeTime = null,
                Memo = string.Empty,
                PayType = IncomeExpensePayType.Amount,
                TargetId = expenseId,
            };

            MMIncomeExpenseModel expense = new()
            {
                Amount = expenseAmount,
                Category = incomeExpenseCategory,
                CreateTime = DateTimeProvider.Now,
                Id = expenseId,
                TransactionType = IncomeExpenseTransactionTypeEnum.Expense,
                SourceId = postTransaction.Id,
                Title = post.Title,
                UserId = expenseUser.UserInfo.UserId,
                Status = IncomeExpenseStatusEnum.Approved,
                DistributeTime = null,
                Memo = string.Empty,
                Rebate = expenseData.Discount,
                TargetId = incomeId,
                PayType = IncomeExpensePayType.Point
            };

            return new UnlockPostInfoModel
            {
                ExpenseData = expenseData,
                Expense = expense,
                Income = income,
                PostTransaction = postTransaction,
            };
        }

        /// <summary>
        /// 取得實際消費金額
        /// (會員會有折扣)
        /// </summary>
        /// <param name="user"></param>
        /// <param name="postType"></param>
        /// <param name="expenseAmount"></param>
        /// <returns></returns>
        private ExpenseData GetExpenseData(UserInfoData user, PostType postType, decimal expenseAmount, bool isFree = false)
        {
            decimal discount = 1;

            if (isFree)
            {
                discount = 0;
            }
            else if (user.VipWelfares.IsNotEmpty())
            {
                discount = user.Discount(postType);
                //MMVipWelfare? welfare = user.VipWelfares
                //    .Where(e => e.Category == postType.ConvertToVIPWelfareCategory() 
                //        && e.Type == VIPWelfareTypeEnum.Discount).SingleOrDefault();
                //if(welfare != null) 
                //{
                //    discount = welfare.Value;
                //}
            }

            decimal discountExpenseAmount = expenseAmount * discount;

            return new ExpenseData
            {
                Discount = discount,
                DiscountAmount = discountExpenseAmount,
            };
        }
    }
}
