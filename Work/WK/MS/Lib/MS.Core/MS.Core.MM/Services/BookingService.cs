using ImageMagick;
using Microsoft.Extensions.Logging;
using MS.Core.Extensions;
using MS.Core.Infrastructures.MQPublisher;
using MS.Core.Infrastructures.Providers;
using MS.Core.Infrastructures.ZeroOne.Models.Requests;
using MS.Core.Infrastructures.ZeroOne.Models.Responses;
using MS.Core.Infrastructures.ZoneOne;
using MS.Core.MM.Infrastructures.Exceptions;
using MS.Core.MM.Model.Entities.Media;
using MS.Core.MM.Models.Booking;
using MS.Core.MM.Models.Booking.Enums;
using MS.Core.MM.Models.Booking.Req;
using MS.Core.MM.Models.Booking.Res;
using MS.Core.MM.Models.Entities.Post;
using MS.Core.MM.Models.Entities.PostTransaction;
using MS.Core.MM.Models.Entities.User;
using MS.Core.MM.Models.Filters;
using MS.Core.MM.Models.User;
using MS.Core.MM.Repos;
using MS.Core.MM.Repos.interfaces;
using MS.Core.MM.Services.interfaces;
using MS.Core.MMModel.Attributes;
using MS.Core.MMModel.Extensions;
using MS.Core.MMModel.Models.AdminBooking;
using MS.Core.MMModel.Models.AdminUserManager;
using MS.Core.MMModel.Models.Booking.Enums;
using MS.Core.MMModel.Models.IncomeExpense;
using MS.Core.MMModel.Models.Media.Enums;
using MS.Core.MMModel.Models.Post.Enums;
using MS.Core.MMModel.Models.User.Enums;
using MS.Core.Models;
using MS.Core.Models.Models;
using Telegram.Bot.Types;

namespace MS.Core.MM.Services
{
    public class BookingService : BaseTransactionService, IBookingService
    {
        private IBookingRepo BookingRepo { get; }
        private IOfficialPostPriceRepo OfficialPostPriceRepo { get; }
        private IPostRepo PostRepo { get; }
        private IVipService VipService { get; }
        private IIncomeExpenseRepo IncomeExpenseRepo { get; }

        private IMSIMOneOnOneChatMessageRepo _messageRepo { get; }

        /// <summary>
        /// 身份认证相关
        /// </summary>
        private readonly IIdentityApplyRepo _identityApply;

        /// <summary>
        /// 店铺相关
        /// </summary>
        private readonly IBossShopRepo _bossShopRepo;

        /// <summary>
        /// 媒體服務
        /// </summary>
        private readonly IEnumerable<IMediaService> MediaServices;

        /// <summary>
        /// 設定取得贴子圖片
        /// </summary>
        private IMediaService PostMediaImageService => MediaServices.First(m => m.SourceType == SourceType.Post && m.Type == MediaType.Image);

        private IMediaService PrivateMessageImageService => MediaServices.First(m => m.SourceType == SourceType.PrivateMessage && m.Type == MediaType.Image);

        /// <summary>
        /// 媒體資源
        /// </summary>
        private readonly IMediaRepo MediaRepo;

        private readonly IMQPublishService mqService;

        private readonly BookingStatus[] DistributeBookingStatuses = new BookingStatus[]
        {
            BookingStatus.TransactionCompleted,
        };

        private IZeroOneApiService ZeroOneApi { get; }

        public BookingService(IRequestIdentifierProvider provider,
            ILogger logger,
            IDateTimeProvider dateTimeProvider,
            IVipTransactionRepo vipTransactionRepo,
            IVipWelfareService vipWelfareServic,
            IUserInfoRepo userInfoRepo,
            IZeroOneApiService zeroOneApiService,
            IBookingRepo bookingRepo,
            IOfficialPostPriceRepo officialPostPriceRepo,
            IPostRepo postRepo,
            IMediaRepo mediaRepo,
            IEnumerable<IMediaService> mediaServices,
            IIncomeExpenseRepo incomeExpenseRepo,
            IVipService vipService,
            IMSIMOneOnOneChatMessageRepo messageRepo,
            IMQPublishService MQService,
            IZeroOneApiService zeroOneApi,
            IIdentityApplyRepo identityApplyRepo,
            IBossShopRepo bossShopRepo) : base(provider, logger, vipTransactionRepo, vipWelfareServic, userInfoRepo, zeroOneApiService, dateTimeProvider)
        {
            BookingRepo = bookingRepo;
            OfficialPostPriceRepo = officialPostPriceRepo;
            PostRepo = postRepo;
            VipService = vipService;
            IncomeExpenseRepo = incomeExpenseRepo;
            MediaRepo = mediaRepo;
            MediaServices = mediaServices;
            IncomeExpenseRepo = incomeExpenseRepo;
            mqService = MQService;
            ZeroOneApi = zeroOneApi;
            _messageRepo = messageRepo;
            _identityApply = identityApplyRepo;
            _bossShopRepo = bossShopRepo;
        }

        /// <summary>
        /// 将服务中的预约单设为已经完成
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public async Task SetBookingCompleted(int size)
        {
            PageResultModel<MMBooking> bookings = await BookingRepo.GetBookingPageByFilter(new MyOrderPageBookingFilter
            {
                Status = BookingStatus.InService,
                Pagination = new PaginationModel
                {
                    Page = 1,
                    PageSize = size,
                },
                IsRunOutOfTime = true,
            });

            if (bookings == null || !bookings.Data.IsNotEmpty())
            {
                return;
            }

            var bookingDatas = bookings.Data;

            var postBookings = bookingDatas.GroupBy(c => c.PostId);
            var userBookings = bookingDatas.GroupBy(c => c.UserId);
            var userBookeds = bookingDatas.GroupBy(c => c.PostUserId);

            var updateBookings = new List<MMBooking>();
            var updatePosts = new List<MMPost>();
            var insertUsers = new List<MMUserSummary>();
            var updateUsers = new List<MMUserSummary>();

            #region 修改预约状态

            foreach (var booking in bookingDatas)
            {
                booking.Status = BookingStatus.TransactionCompleted;
                booking.FinishTime = DateTime.Now;
                updateBookings.Add(booking);
            }

            #endregion 修改预约状态

            #region 修改帖子预约次数

            //修改帖子预约次数
            foreach (var postGroup in postBookings)
            {
                var postAppointmentCount = postGroup.Count();
                var postInfo = await PostRepo.GetById(postGroup.Key);
                if (postInfo == null)
                {
                    continue;
                }
                updatePosts.Add(postInfo);
            }

            #endregion 修改帖子预约次数

            #region 修改或者新增用户的预约次数

            //修改或者新增用户的预约次数
            foreach (var userGroup in userBookings)
            {
                var userAppointmentCount = userGroup.Count();
                var usersDatas = await UserInfoRepo.GetUserSummary(new UserSummaryFilter()
                {
                    UserId = userGroup.Key,
                    Category = (int)UserSummaryCategoryEnum.Official,
                    Type = (int)UserSummaryTypeEnum.BookingCount
                });

                if (usersDatas.Any())
                {
                    var model = usersDatas.First();
                    model.Amount = userGroup.Count();
                    updateUsers.Add(usersDatas.First());
                }
                else
                {
                    var model = new MMUserSummary();
                    model.Amount = userGroup.Count();
                    model.UserId = userGroup.Key;
                    model.Category = UserSummaryCategoryEnum.Official;
                    model.Type = UserSummaryTypeEnum.BookingCount;
                    insertUsers.Add(model);
                }
            }

            #endregion 修改或者新增用户的预约次数

            #region 修改或者新增用户的被预约次数

            //修改或者新增用户的被预约次数
            foreach (var userGroup in userBookeds)
            {
                var userAppointmentCount = userGroup.Count();
                var usersDatas = await UserInfoRepo.GetUserSummary(new UserSummaryFilter()
                {
                    UserId = userGroup.Key,
                    Category = (int)UserSummaryCategoryEnum.Official,
                    Type = (int)UserSummaryTypeEnum.BookedCount
                });

                if (usersDatas.Any())
                {
                    var model = usersDatas.First();
                    model.Amount = userGroup.Count();
                    updateUsers.Add(usersDatas.First());
                }
                else
                {
                    var model = new MMUserSummary();
                    model.Amount = userGroup.Count();
                    model.UserId = userGroup.Key;
                    model.Category = UserSummaryCategoryEnum.Official;
                    model.Type = UserSummaryTypeEnum.BookedCount;
                    insertUsers.Add(model);
                }
            }

            #endregion 修改或者新增用户的被预约次数

            var dataList = new ResBookingCompleted()
            {
                Bookings = updateBookings,
                Posts = updatePosts,
                InsertUserSummary = insertUsers,
                UpdateUserSummary = updateUsers
            };

            await BookingRepo.SetBookingCompleted(dataList);

            dataList.Bookings.ForEach(async c =>
            {
                ///判断这组发帖人和预约人是否还有其他未完成的订单！！！！！
                int count = await BookingRepo.GetUserAndPostUserNotCompletedCount(c.UserId, c.PostUserId);
                //如果count数等于0的话,代表UserId 下的预约单号已经全部完成
                if (count == 0)
                {
                    count = await BookingRepo.GetUserAndPostUserNotCompletedCount(c.PostUserId, c.UserId);
                    if (count == 0)
                    {
                        await DeleteChatMessages(c.UserId, c.PostUserId);
                    }
                }
            });
        }

        /// <summary>
        /// 刪除對話訊息
        /// </summary>
        /// <param name="userId">預約者編號</param>
        /// <param name="postUserId">發帖者編號</param>
        /// <returns></returns>
        private async Task DeleteChatMessages(int userId, int postUserId)
        {
            var ids = new string[0];
            try
            {
                var messages = await _messageRepo.GetRoomMessages(userId, new MMModel.Models.Chat.QueryRoomMessageParam()
                {
                    RoomID = postUserId.ToString()
                }, int.MaxValue);

                ids = messages.Data.Where(x => x.MessageType == 2).Select(x => x.Message).ToArray();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"獲取刪除私訊訊息失敗");
            }

            await mqService.DeleteChatMessages(userId, postUserId);

            if (ids.Length > 0)
            {
                foreach (var id in ids)
                {
                    try
                    {
                        await PrivateMessageImageService.Delete(id);
                        await Task.Delay(TimeSpan.FromSeconds(1));
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"刪除私訊圖片失敗, Id:{id}");
                    }
                }
            }
        }

        /// <summary>
        /// 派發
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public async Task Distribute(int size)
        {
            PageResultModel<MMBooking> bookings = await BookingRepo.GetPageByFilter(new PageBookingFilter
            {
                Statuses = DistributeBookingStatuses,
                Pagination = new PaginationModel
                {
                    Page = 1,
                    PageSize = size,
                },
                IsDistribute = false,
            });

            var expenses = (await IncomeExpenseRepo.GetTransactionByFilter(new IncomeExpenseFilter
            {
                SourceIds = bookings.Data.Select(e => e.BookingId),
                PayType = IncomeExpensePayType.Point,
                TransactionTypes = new List<IncomeExpenseTransactionTypeEnum> { IncomeExpenseTransactionTypeEnum.Expense },
                Categories = new IncomeExpenseCategoryEnum[1] { IncomeExpenseCategoryEnum.Official },
            })).ToDictionary(e => e.SourceId, e => e);

            foreach (MMBooking booking in bookings.Data)
            {
                if (expenses.ContainsKey(booking.BookingId))
                {
                    await Distribute(booking, expenses[booking.BookingId]);
                    await Task.Delay(500);
                }
            }
        }

        /// <inheritdoc/>
        public async Task<BaseReturnDataModel<ResBookingDetail>> GetBookingDetail(ReqBookingDetail req)
        {
            return await base.TryCatchProcedure(async (param) =>
            {
                UserInfoData user = await VipService.GetUserInfoData(param.UserId);
                IEnumerable<MMOfficialPostPrice> prices = await OfficialPostPriceRepo.GetByPostId(req.PostId);

                //获取帖子信息
                var postdata = await PostRepo.GetOfficialPostById(req.PostId);
                //获取当前发帖人的身份信息
                var postUserinfo = await VipService.GetUserInfoData(postdata.UserId);

                return new BaseReturnDataModel<ResBookingDetail>(ReturnCode.Success)
                {
                    DataModel = new ResBookingDetail
                    {
                        Prices = prices.Select(price =>
                        {
                            BookingAmountData amountData = GetBookingAmountData(price, user, postUserinfo);
                            ResBookingPrice bookingPrice = new ResBookingPrice
                            {
                                BookingPrice = amountData.BookingAmount.ToString(GlobalSettings.AmountFormat),
                                ComboName = price.ComboName,
                                Service = price.Service,
                                ComboPrice = price.ComboPrice.ToString(GlobalSettings.AmountFormat),
                                FullPrice = amountData.FullAmount.ToString(GlobalSettings.AmountFormat),
                                PriceId = price.Id,
                            };
                            return bookingPrice;
                        }).ToArray()
                    }
                };
            }, req);
        }

        /// <summary>
        /// 超時未接單處理
        /// </summary>
        /// <returns></returns>
        /// <exception cref="MMException"></exception>
        public async Task TimeoutNoAcceptance()
        {
            await base.TryCatchProcedure(async () =>
            {
                var bookings = await BookingRepo.GetPageByFilter(new PageBookingFilter
                {
                    //Statuses = BookingStatus.TimeoutNoAcceptanceProgress.ToEnumerable(),
                    Pagination = new PaginationModel
                    {
                        PageNo = 1,
                        PageSize = 1000,
                    },
                });

                foreach (MMBooking booking in bookings.Data)
                {
                    try
                    {
                        //await Refund(booking, BookingStatus.TimeoutNoAcceptance);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"退款失敗:{booking.BookingId}");
                    }
                }

                return new BaseReturnModel(ReturnCode.Success);
            });
        }

        /// <summary>
        /// 退款
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        /// <exception cref="MMException"></exception>
        public async Task<BaseReturnDataModel<ResBookingCancel>> Refund(ReqBookingCancel req)
        {
            return await base.TryCatchProcedure(async (param) =>
            {
                MMBooking? booking = await BookingRepo.GetById(param.BookingId);

                if (booking == null)
                {
                    throw new MMException(ReturnCode.DataIsNotExist, "预约单不存在");
                }

                if (booking.UserId != param.UserId)
                {
                    throw new MMException(ReturnCode.InvalidUser, "無此權限");
                }

                switch (booking.Status)
                {
                    /*case BookingStatus.Pending:
                        await Refund(booking, BookingStatus.RefundSuccessful);
                        break;*/

                    default:
                        throw new MMException(ReturnCode.DataNotUse, "该贴子不可取消");
                }

                return new BaseReturnDataModel<ResBookingCancel>(ReturnCode.Success);
            }, req);
        }

        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        /// <exception cref="MMException"></exception>
        public async Task<BaseReturnDataModel<ResBookingCancel>> Cancel(ReqBookingCancel req)
        {
            return await base.TryCatchProcedure(async (param) =>
            {
                MMBooking? booking = await BookingRepo.GetById(param.BookingId);

                if (booking == null)
                {
                    throw new MMException(ReturnCode.DataIsNotExist, "预约单不存在");
                }

                if (booking.PostUserId != param.UserId)
                {
                    throw new MMException(ReturnCode.InvalidUser, "無此權限");
                }

                switch (booking.Status)
                {
                    /*case BookingStatus.Pending:
                        if (booking.PostUserId == param.UserId)
                        {
                            await Refund(booking, BookingStatus.OrderCancelled);
                        }
                        else
                        {
                            await Refund(booking, BookingStatus.RefundSuccessful);
                        }

                        break;*/

                    default:
                        throw new MMException(ReturnCode.DataNotUse, "该贴子不可取消");
                }

                return new BaseReturnDataModel<ResBookingCancel>(ReturnCode.Success);
            }, req);
        }

        /// <summary>
        /// 刪除
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        /// <exception cref="MMException"></exception>
        public async Task<BaseReturnDataModel<ResBookingDelete>> Delete(ReqBookingDelete req)
        {
            return await base.TryCatchProcedure(async (param) =>
            {
                MMBooking? booking = await BookingRepo.GetById(param.BookingId);

                if (booking == null)
                {
                    throw new MMException(ReturnCode.DataIsNotExist, "预约单不存在");
                }

                if (booking.UserId != param.UserId)
                {
                    throw new MMException(ReturnCode.InvalidUser, "無此權限");
                }

                booking.IsDelete = true;
                await BookingRepo.Update(booking);
                _logger.LogInformation($"Delete:{booking.BookingId}:{DateTimeProvider.Now}");

                return new BaseReturnDataModel<ResBookingDelete>(ReturnCode.Success);
            }, req);
        }

        /// <summary>
        /// 退款
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        /// <exception cref="MMException"></exception>
        public async Task<BaseReturnDataModel<ResBookingRefund>> Refund(ReqBookingRefund req)
        {
            return await base.TryCatchProcedure(async (param) =>
            {
                if (!Enum.IsDefined(typeof(RefundReasonType), param.ReasonType))
                {
                    throw new MMException(ReturnCode.ParameterIsInvalid);
                }

                MMBooking? booking = await BookingRepo.GetById(param.BookingId);

                if (booking == null)
                {
                    throw new MMException(ReturnCode.DataIsNotExist, "预约单不存在");
                }

                if (booking.UserId != param.UserId)
                {
                    throw new MMException(ReturnCode.DataNotUse, "非本人操作");
                }

                var applyRefundOrders = await BookingRepo.GetApplyRefundWithBookingId(new string[] { param.BookingId });
                if (applyRefundOrders.Any())
                {
                    throw new MMException(ReturnCode.RefundHasBeenSubmitted);
                }

                MMMedia[] media = await MediaRepo.Get(param.PhotoIds, true);
                media = media
                    .Where(p => p.SourceType == (int)SourceType.Refund &&
                        p.MediaType == (int)MediaType.Image &&
                        string.IsNullOrWhiteSpace(p.RefId))?
                    .ToArray() ?? new MMMedia[0];

                if (!media.Any())
                {
                    throw new MMException(ReturnCode.InvalidPhoto);
                }

                switch (booking.Status)
                {
                    case BookingStatus.InService:
                        await InServiceRefund(booking, param, media);
                        break;

                    default:
                        throw new MMException(ReturnCode.DataNotUse, "该贴子不可退款");
                }

                return new BaseReturnDataModel<ResBookingRefund>(ReturnCode.Success);
            }, req);
        }

        /// <summary>
        /// 確認完成
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        /// <exception cref="MMException"></exception>
        public async Task<BaseReturnDataModel<ResBookingDone>> Done(ReqBookingDone req)
        {
            return await base.TryCatchProcedure(async (param) =>
            {
                MMBooking? booking = await BookingRepo.GetById(param.BookingId);

                if (booking == null)
                {
                    throw new MMException(ReturnCode.DataIsNotExist, "预约单不存在");
                }

                if (booking.Status != BookingStatus.InService)
                {
                    throw new MMException(ReturnCode.DataNotUse, "该預約單不可完成");
                }

                ///已经有收益单，不允许完成
                if (!string.IsNullOrEmpty(booking.IncomeId))
                {
                    throw new MMException(ReturnCode.DataNotUse, "已经有收益单，不允许完成");
                }
                //非本人
                if (booking.UserId != param.UserId)
                {
                    throw new MMException(ReturnCode.IllegalUser, "非本人操作");
                }

                booking.Status = BookingStatus.TransactionCompleted;
                booking.FinishTime = DateTimeProvider.Now;

                await BookingRepo.Done(new BookingDone
                {
                    Booking = booking,
                });

                MMIncomeExpenseModel expense = await IncomeExpenseRepo.GetTransactionByFilter(new IncomeExpenseFilter
                {
                    SourceIds = booking.BookingId.ToEnumerable(),
                    PayType = IncomeExpensePayType.Point,
                    TransactionTypes = new List<IncomeExpenseTransactionTypeEnum> { IncomeExpenseTransactionTypeEnum.Expense }
                }).FirstAsync();

                MMIncomeExpenseModel? income = await Distribute(booking, expense);

                int count = await BookingRepo.GetUserAndPostUserNotCompletedCount(param.UserId, booking.PostUserId);
                //如果count数等于0的话,代表UserId 下的预约单号已经全部完成
                if (count == 0)
                {
                    //觅老板相互下单
                    count = await BookingRepo.GetUserAndPostUserNotCompletedCount(booking.PostUserId, param.UserId);
                    if (count == 0)
                    {
                        await DeleteChatMessages(param.UserId, booking.PostUserId);
                    }
                }

                return new BaseReturnDataModel<ResBookingDone>(ReturnCode.Success)
                {
                    DataModel = new ResBookingDone
                    {
                        IncomeId = income?.Id,
                    },
                };
            }, req);
        }

        /// <summary>
        /// 接受
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<BaseReturnDataModel<ResBookingAccept>> Accept(ReqBookingAccept req)
        {
            return await base.TryCatchProcedure(async (param) =>
            {
                MMBooking? booking = await BookingRepo.GetById(req.BookingId);

                if (booking == null)
                {
                    throw new MMException(ReturnCode.DataIsNotExist, "预约单不存在");
                }

                //if (booking.Status != BookingStatus.Pending)
                //{
                //    throw new MMException(ReturnCode.DataNotUse, "该預約單不可接單");
                //}

                if (booking.BookingTime.AddMinutes(180) <= DateTimeProvider.Now)
                {
                    throw new MMException(ReturnCode.OperationFailed, "该預約單已超過時限");
                }

                //非本人
                if (booking.PostUserId != param.UserId)
                {
                    throw new MMException(ReturnCode.IllegalUser, "非本人操作");
                }

                await BookingRepo.Accept(new BookingAccept
                {
                    AcceptTime = DateTimeProvider.Now,
                    BookingId = booking.BookingId,
                    OriginalStatus = booking.Status,
                    Status = BookingStatus.InService,
                    ScheduledTime = DateTimeProvider.Now.AddHours(48),
                });

                return new BaseReturnDataModel<ResBookingAccept>(ReturnCode.Success);
            }, req);
        }

        /// <summary>
        /// 管理预约状态转换
        /// </summary>
        /// <param name="myBookingStatus"></param>
        /// <returns></returns>
        private IEnumerable<BookingStatus> ManageBookingsStateMutual(MyBookingStatus myBookingStatus)
        {
            IEnumerable<BookingStatus> bookingStatuses = new List<BookingStatus>() { BookingStatus.InService };
            switch (myBookingStatus)
            {
                case MyBookingStatus.InService:
                    bookingStatuses = new List<BookingStatus>()
                    {
                        BookingStatus.InService,
                    };
                    break;

                case MyBookingStatus.Refunding:
                    bookingStatuses = new List<BookingStatus>()
                    {
                        BookingStatus.RefundInProgress,
                    };
                    break;

                case MyBookingStatus.Refunded:
                    bookingStatuses = new List<BookingStatus>()
                    {
                        BookingStatus.RefundSuccessful,
                    };
                    break;

                case MyBookingStatus.Completed:
                    bookingStatuses = new List<BookingStatus>(){
                        BookingStatus.TransactionCompleted,
                    };
                    break;

                default:
                    break;
            }

            return bookingStatuses;
        }

        /// <summary>
        /// 預約管理列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<BaseReturnDataModel<PageResultModel<ResManageBooking>>> GetManageBookings(ReqManageBooking req)
        {
            return await base.TryCatchProcedure(async (param) =>
            {
                var result = new BaseReturnDataModel<PageResultModel<ResManageBooking>>(ReturnCode.Success);
                result.DataModel = new PageResultModel<ResManageBooking>();

                var bookings = await BookingRepo.GetPageByFilter(new PageBookingFilter
                {
                    Pagination = new PaginationModel
                    {
                        PageNo = param.PageNo,
                    },
                    PostUserId = param.UserId,
                    Statuses = ManageBookingsStateMutual(req.Status)
                });

                var posts = (await PostRepo.GetByFilter(new OfficialPostFilter
                {
                    PostIds = bookings.Data.Select(e => e.PostId),
                })).ToDictionary(e => e.PostId, e => e);

                Dictionary<int, BriefUserInfo> briefUserInfos = (await UserInfoRepo.GetBriefUserInfoByFilter(new BriefUserInfoFilter
                {
                    UserIds = bookings.Data.Select(e => e.UserId)
                })).ToDictionary(e => e.UserId, e => e);

                var resbooking = new List<ResManageBooking>();

                foreach (var booking in bookings.Data)
                {
                    ZOUserInfoRes zOUserInfo = await ZeroOneApi.GetUserInfo(new ZOUserInfoReq(booking.UserId)).GetReturnDataAsync();

                    MMOfficialPost? post = posts.GetValueOrDefault(booking.PostId);

                    ResManageBooking resManageBooking = new()
                    {
                        AvatarUrl = zOUserInfo?.Avatar,
                        Nickname = zOUserInfo?.NickName,
                        UserId = booking.UserId,
                        Status = booking.Status,
                        StatusText = booking.Status.GetDescription<ManageDescriptionAttribute>(),
                        Title = post?.Title,
                        Contact = booking.Contact,
                        BookingTime = booking.BookingTime.ToString(GlobalSettings.DateTimeFormat),
                        AcceptTime = booking.AcceptTime?.ToString(GlobalSettings.DateTimeFormat) ?? string.Empty,
                        CancelTime = booking.CancelTime?.ToString(GlobalSettings.DateTimeFormat) ?? string.Empty,
                        FinishTime = booking.FinishTime?.ToString(GlobalSettings.DateTimeFormat) ?? string.Empty,
                        PaymentType = booking.PaymentType,
                        BookingId = booking.BookingId,
                        PaymentMoney = $"{booking.PaymentMoney:0}钻石"
                    };
                    resbooking.Add(resManageBooking);
                }
                result.DataModel.Page = bookings.Page;
                result.DataModel.PageNo = bookings.PageNo;
                result.DataModel.TotalCount = bookings.TotalCount;
                result.DataModel.TotalPage = bookings.TotalPage;
                result.DataModel.Data = resbooking.ToArray();
                return result;
            }, req);
        }

        /// <summary>
        /// 我的預約
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<BaseReturnDataModel<PageResultModel<ResMyBooking>>> GetMyBooking(ReqMyBooking req)
        {
            return await base.TryCatchProcedure(async (param) =>
            {
                var bookings = await BookingRepo.GetBookingPageByFilter(new MyOrderPageBookingFilter
                {
                    Pagination = new PaginationModel
                    {
                        PageNo = param.PageNo,
                    },
                    UserId = param.UserId,
                    Status = param.Status,
                    IsDelete = false,
                });

                MMOfficialPost[] officialPosts =
                   await PostRepo.GetByFilter(new OfficialPostFilter
                   {
                       PostIds = bookings.Data.Select(e => e.PostId).ToArray()
                   }).ToArrayAsync();

                Dictionary<string, MMOfficialPost> officialPostDic =
                    officialPosts
                    .ToDictionary(e => e.PostId, e => e);

                Dictionary<int, MMUserInfo> users =
                    (await UserInfoRepo.GetUserInfos(officialPosts.Select(e => e.UserId)))
                        .ToDictionary(e => e.UserId, e => e);

                IEnumerable<ResMyBooking> data = Enumerable.Empty<ResMyBooking>();

                foreach (var booking in bookings.Data)
                {
                    MMOfficialPost? post = officialPostDic.GetValueOrDefault(booking.PostId);
                    if (post == null)
                    {
                        continue;
                    }
                    var myBookingPost = await GetBookingPostData(post);
                    ///是否拒绝退款
                    bool RefusalOfRefund = false;
                    //退款是否成功
                    bool RefundSuccessful = false;
                    IEnumerable<MMApplyRefund> applyRefunds = await BookingRepo.GetApplyRefundWithBookingId(new string[] { booking.BookingId });
                    if (applyRefunds.Any())
                    {
                        if (applyRefunds.First() != null && applyRefunds.First().Status == 2)
                            RefusalOfRefund = true;
                        /*if (applyRefunds.First() != null && applyRefunds.First().Status == 1)
                            RefundSuccessful = true;*/
                    }

                    ResMyBooking resMyBooking = new()
                    {
                        Post = myBookingPost,
                        Status = booking.Status,
                        PaymentStatus = GetPaymentStatus(booking),
                        BookingId = booking.BookingId,
                        PaymentMoney = $"{booking.PaymentMoney:0}钻石",
                        RefusalOfRefund = RefusalOfRefund,
                    };
                    data = data.Append(resMyBooking);
                }

                return new BaseReturnDataModel<PageResultModel<ResMyBooking>>(ReturnCode.Success)
                {
                    DataModel = new PageResultModel<ResMyBooking>
                    {
                        PageNo = bookings.PageNo,
                        PageSize = bookings.PageSize,
                        TotalCount = bookings.TotalCount,
                        TotalPage = bookings.TotalPage,
                        Data = data.ToArray(),
                    },
                };
            }, req);
        }

        /// <summary>
        /// 取得預約資訊
        /// </summary>
        /// <param name="bookingId">預約編號</param>
        /// <returns>預約資訊</returns>
        public async Task<BaseReturnDataModel<MMBooking>> GetBookingInfo(string bookingId)
        {
            return await base.TryCatchProcedure(async (param) =>
            {
                var bookingInfo = await BookingRepo.GetById(bookingId);
                if (bookingInfo != null)
                {
                    return new BaseReturnDataModel<MMBooking>(ReturnCode.Success)
                    {
                        DataModel = bookingInfo,
                    };
                }
                return new BaseReturnDataModel<MMBooking>(ReturnCode.DataIsNotExist);
            }, bookingId);
        }

        /// <summary>
        /// 預約
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<BaseReturnDataModel<ResBooking>> Booking(ReqBooking req)
        {
            return await base.TryCatchProcedure(async (param) =>
            {
                MMOfficialPost post = await GetPost(param.PostId);
                //获取当前发帖人的身份信息
                var postUserinfo = await VipService.GetUserInfoData(post.UserId);
                if (!postUserinfo.UserInfo.IsOpen)
                {
                    return new BaseReturnDataModel<ResBooking>(new BaseReturnModel(ReturnCode.CustomizedMessage, "店铺已关闭，请退回首页"));
                }
                if (post.Status == ReviewStatus.UnderReview || !post.LockStatus)
                {
                    return new BaseReturnDataModel<ResBooking>(new BaseReturnModel(ReturnCode.CustomizedMessage, "帖子编辑中，请稍后再试"));
                }
                else if (post.Status == ReviewStatus.NotApproved || post.IsDelete)
                {
                    return new BaseReturnDataModel<ResBooking>(new BaseReturnModel(ReturnCode.CustomizedMessage, "帖子已下架，请退回首页"));
                }

                MMOfficialPostPrice price = await GetPostPrice(param.PostPriceId);

                if (price.PostId != param.PostId)
                {
                    return new BaseReturnDataModel<ResBooking>(ReturnCode.ParameterIsInvalid);
                }

                UserInfoData user = await VipService.GetUserInfoData(param.UserId);

                //根据用户id获取申请信息
                var identity = await _identityApply.QueryBossOrSuperBoss(post.UserId, (int)ReviewStatus.Approval);
                //根据用申请ID获取店铺信息
                var bossDetail = await _bossShopRepo.GetBossDetailByApplyId(identity.ApplyId);

                //预定时帖子身份和当前发帖人身份不一致
                if (postUserinfo.UserInfo.UserIdentity != param.PostUserIdentity)
                {
                    return new BaseReturnDataModel<ResBooking>(new BaseReturnModel(ReturnCode.CustomizedMessage, "商品数据更新，请退出后重新预约"));
                }

                BookingAmountData amountData = GetBookingAmountData(price, user, postUserinfo);

                //如果是超觅老板使用拆账比信息
                decimal? platformSharing = null;
                if ((IdentityType)postUserinfo.UserInfo.UserIdentity == IdentityType.SuperBoss && bossDetail.PlatformSharing.HasValue && bossDetail.PlatformSharing > 0)
                {
                    platformSharing = bossDetail.PlatformSharing / 100M;
                }

                string bookingId = await BookingRepo.GetSequenceIdentity();

                string expenseId = await BookingRepo.GetSequenceIdentity<MMIncomeExpenseModel>();

                decimal paymentMoney = req.PaymentType == BookingPaymentType.Full ? amountData.FullAmount : amountData.BookingAmount;

                //扣款
                await ZeroOneBookingExpense(param, paymentMoney, expenseId);

                PostBookingModel postBooking =
                    new()
                    {
                        MMBooking = new MMBooking
                        {
                            BookingId = bookingId,
                            BookingTime = DateTimeProvider.Now,
                            Contact = "",
                            ComboName = price.ComboName,
                            ComboPrice = price.ComboPrice,
                            Service = price.Service,
                            BookingAmount = amountData.BookingAmount,
                            PaymentMoney = paymentMoney,
                            PaymentType = param.PaymentType,
                            PostId = param.PostId,
                            Status = BookingStatus.InService,
                            UserId = param.UserId,
                            Discount = amountData.Discount,
                            PostUserId = post.UserId,
                            IsDelete = false,
                            AcceptTime = null,
                            CancelTime = null,
                            FinishTime = null,
                            ScheduledTime = DateTimeProvider.Now.AddHours(48),
                            CurrentIdentity = (IdentityType)postUserinfo.UserInfo.UserIdentity,
                            PlatformSharing = platformSharing
                        },
                        MMIncomeExpenseModel = new MMIncomeExpenseModel
                        {
                            Id = expenseId,
                            Category = IncomeExpenseCategoryEnum.Official,
                            CreateTime = DateTimeProvider.Now,
                            PayType = IncomeExpensePayType.Point,
                            Amount = paymentMoney,
                            SourceId = bookingId,
                            Status = IncomeExpenseStatusEnum.Approved,
                            TransactionType = IncomeExpenseTransactionTypeEnum.Expense,
                            UserId = param.UserId,
                            Title = post.Title,
                            DistributeTime = null,
                            TargetId = string.Empty,
                            Memo = param.PaymentType == BookingPaymentType.Booking ? "预约金" : "全额",
                            Rebate = 1M,
                            UnusualMemo = string.Empty,
                            UpdateTime = null,
                        },
                    };

                await BookingRepo.Booking(postBooking);

                //发送短信
                ZOUserInfoRes zoUser = await ZeroOneApiService.GetUserInfo(new ZOUserInfoReq(post.UserId)).GetReturnDataAsync();

                string prefixes = string.Empty;
                string phoneNo = string.Empty;
                if (!string.IsNullOrEmpty(zoUser.Phone))
                    (prefixes, phoneNo) = zoUser.Phone.SplitWholePhoneNo();

                return new BaseReturnDataModel<ResBooking>(ReturnCode.Success)
                {
                    DataModel = new ResBooking
                    {
                        BookingId = bookingId,
                        CountryCode = prefixes,
                        PhoneNo = phoneNo,
                        ContentParamInfo = "888888"
                    }
                };
            }, req);
        }

        /// <summary>
        /// 訂單詳情
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<BaseReturnDataModel<ResMyBookingDetail>> GetMyBookingDetail(ReqMyBookingDetail req)
        {
            return await base.TryCatchProcedure(async (param) =>
            {
                MMBooking? booking = await BookingRepo.GetById(req.BookingId);

                if (booking == null)
                {
                    throw new MMException(ReturnCode.DataIsNotExist, "预约单不存在");
                }

                if (booking.UserId != param.UserId)
                {
                    throw new MMException(ReturnCode.IllegalUser, "非本人操作");
                }

                MMOfficialPost post = await GetPost(booking.PostId);
                if (post == null)
                {
                    throw new MMException(ReturnCode.SearchResultIsEmpty, "查询不到该帖");
                }

                MMUserInfo? user = await UserInfoRepo.GetUserInfo(post.UserId);

                ///是否拒绝退款
                bool RefusalOfRefund = false;
                //退款是否成功
                bool RefundSuccessful = false;
                //官方拒绝退款备注
                string Memo = string.Empty;
                IEnumerable<MMApplyRefund> applyRefunds = await BookingRepo.GetApplyRefundWithBookingId(new string[] { booking.BookingId });
                if (applyRefunds.Any())
                {
                    if (applyRefunds.First() != null && applyRefunds.First().Status == 2)
                    {
                        RefusalOfRefund = true;
                        Memo = applyRefunds.First().Memo;
                    }
                    if (applyRefunds.First() != null && applyRefunds.First().Status == 1)
                        RefundSuccessful = true;
                }

                return new BaseReturnDataModel<ResMyBookingDetail>(ReturnCode.Success)
                {
                    DataModel = new ResMyBookingDetail
                    {
                        Post = await GetBookingPostData(post),
                        Status = RefundSuccessful ? BookingStatus.RefundSuccessful : booking.Status,
                        PaymentStatus = GetPaymentStatus(booking),
                        BookingId = booking.BookingId,
                        AcceptTime = booking.AcceptTime?.ToString(GlobalSettings.DateTimeFormat) ?? string.Empty,
                        BookingTime = booking.BookingTime.ToString(GlobalSettings.DateTimeFormat),
                        FinishTime = booking.FinishTime?.ToString(GlobalSettings.DateTimeFormat) ?? string.Empty,
                        PaymentMoney = $"{booking.PaymentMoney:0}钻石",
                        RefusalOfRefund = RefusalOfRefund,
                        Memo = Memo,
                    }
                };
            }, req);
        }

        /// <summary>
        /// 后台审核预约退款
        /// </summary>
        /// <param name="refundModel"></param>
        /// <returns></returns>
        public async Task Refund(MMApplyRefund refundModel)
        {
            var booking = await BookingRepo.GetById(refundModel.BookingId);
            var now = DateTimeProvider.Now;
            ///后台审核拒绝退款
            if (refundModel.Status == 2)
            {
                refundModel.ExamineTime = now;

                //计算订单距离完成时间:过期时间=订单过期时间+（退款审核时间-退款申请时间）

                var ScheduledTime = booking.ScheduledTime.HasValue ? (DateTime)booking.ScheduledTime + (refundModel.ExamineTime - refundModel.ApplyTime) : booking.BookingTime.AddHours(48) + (refundModel.ExamineTime - refundModel.ApplyTime);

                await BookingRepo.RefuseRefund(refundModel, new AdminRefuseRefundUpdateModel
                {
                    ///后台拒绝退款后,将状态改为服务中，等待48小时后自动改为已经完成
                    BookingId = refundModel.BookingId,
                    Status = (int)BookingStatus.InService,
                    ScheduledTime = (DateTime)ScheduledTime,
                });
            }//后台审核同意退款
            else if (refundModel.Status == 1)
            {
                BookingRefundModel bookingRefund = new()
                {
                    BookingId = booking.BookingId,
                    Status = BookingStatus.RefundSuccessful,
                    CancelTime = now,
                    OriginalStatus = booking.Status,
                };
                string incomeId = await BookingRepo.GetSequenceIdentity<MMIncomeExpenseModel>();
                MMIncomeExpenseModel income = new()
                {
                    Amount = booking.PaymentMoney,
                    Category = IncomeExpenseCategoryEnum.Official,
                    CreateTime = now,
                    DistributeTime = now,
                    Memo = "预约退款",
                    Status = IncomeExpenseStatusEnum.Refund,
                    Id = incomeId,
                    PayType = IncomeExpensePayType.Point,
                    Rebate = 1,
                    SourceId = booking.BookingId,
                    TargetId = string.Empty,
                    Title = "",
                    TransactionType = IncomeExpenseTransactionTypeEnum.Refund,
                    UnusualMemo = string.Empty,
                    UserId = booking.UserId,
                };
                await BookingRepo.Refund(new RefundModel(bookingRefund, income, refundModel));
                await ZeroOneApiService.PointIncome(new ZOPointIncomeExpenseReq(ZOIncomeExpenseCategory.UnBooking, booking.UserId, booking.PaymentMoney, income.Id));

                int count = await BookingRepo.GetUserAndPostUserNotCompletedCount(booking.UserId, booking.PostUserId);
                //如果count数等于0的话,代表UserId 下的预约单号已经全部完成
                if (count == 0)
                {
                    //觅老板相互下单
                    count = await BookingRepo.GetUserAndPostUserNotCompletedCount(booking.PostUserId, booking.UserId);
                    if (count == 0)
                    {
                        await DeleteChatMessages(booking.UserId, booking.PostUserId);
                    }
                }
            }
        }

        private async Task<MMIncomeExpenseModel?> Distribute(MMBooking booking, MMIncomeExpenseModel expense)
        {
            string incomeId = await BookingRepo.GetSequenceIdentity<MMIncomeExpenseModel>();

            decimal incomeAmount = 0M;

            // 如果是超觅老板使用支付金额
            if (booking.CurrentIdentity.HasValue && booking.CurrentIdentity == IdentityType.SuperBoss)
            {
                incomeAmount = booking.PaymentMoney;
            }
            else
            {
                if (booking.PaymentMoney > booking.BookingAmount)
                {
                    incomeAmount = booking.PaymentMoney - booking.BookingAmount;
                }
            }

            var post = await PostRepo.GetOfficialPostById(booking.PostId);

            string memo = booking.PaymentType == BookingPaymentType.Booking ? $"支付预约金" : $"支付全额";

            expense.TargetId = incomeId;

            decimal rebate = 0.1M;
            //如果超觅老板则使用拆账比
            if (booking.CurrentIdentity == IdentityType.SuperBoss && booking.PlatformSharing != null)
            {
                rebate = (1 - booking.PlatformSharing.Value) / 10;
            }

            MMIncomeExpenseModel income = new()
            {
                Amount = incomeAmount,
                Category = IncomeExpenseCategoryEnum.Official,
                DistributeTime = DateTimeProvider.Now,
                CreateTime = DateTimeProvider.Now,
                PayType = IncomeExpensePayType.Amount,
                Rebate = rebate,
                SourceId = booking.BookingId,
                Status = IncomeExpenseStatusEnum.Dispatched,
                Title = post?.Title ?? string.Empty,
                TransactionType = IncomeExpenseTransactionTypeEnum.Income,
                Memo = memo,
                TargetId = expense.Id,
                UnusualMemo = string.Empty,
                UpdateTime = null,
                UserId = booking.PostUserId,
                Id = incomeId,
            };

            var distribute = new BookingDistribute()
            {
                BookingId = booking.BookingId,
                Status = BookingStatus.TransactionCompleted,
                Statuses = DistributeBookingStatuses,
                IncomeId = incomeId,
                CurrentIdentity = booking.CurrentIdentity,
                PlatformSharing = booking.PlatformSharing
            };

            var bookingDistribute = new BookingDistributeModel()
            {
                Booking = distribute,
                Income = income,
                Expense = expense,
            };

            try
            {
                //if (post != null){
                //    await PostRepo.EditMMOfficialPost(post);
                //}

                await BookingRepo.Distribute(bookingDistribute);

                if (income.Amount > 0)
                {
                    decimal amount = Math.Floor(income.Amount * income.Rebate);
                    decimal newamount = Convert.ToDecimal(amount.ToString("F2"));
                    ZOCashIncomeExpenseReq zoCashReq = new(ZOIncomeExpenseCategory.BookingEarnings, income.UserId, newamount, income.Id);
                    await ZeroOneApiService.CashIncome(zoCashReq);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "派發失敗，BookingId:{BookingId}、IncomeId:{Id}", booking.BookingId, income.Id);
                return null;
            }

            return income;
        }

        /// <summary>
        /// 取得訂單狀態
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="booking"></param>
        /// <returns></returns>
        private static string GetPaymentStatus<T>(T booking) where T : IBookingPaymentStatus
        {
            string paymentType = booking.PaymentType == BookingPaymentType.Full ? "全额" : "预约金";

            string status = GetStatus(booking.Status);

            return $"{status}{paymentType}";

            static string GetStatus(BookingStatus status)
            {
                switch (status)
                {
                    case BookingStatus.RefundSuccessful:
                        return "已退还";

                    default:
                        return "已支付";
                }
            }
        }

        private async Task<ResMyBookingPost> GetBookingPostData(MMOfficialPost? post)
        {
            ZOUserInfoRes zOUserInfo = await ZeroOneApi.GetUserInfo(new ZOUserInfoReq(post.UserId)).GetReturnDataAsync();

            return new ResMyBookingPost
            {
                UserId = zOUserInfo?.UserId,
                AvatarUrl = zOUserInfo?.Avatar,
                Nickname = zOUserInfo?.NickName,
                CoverUrl = post == null ? string.Empty : await PostMediaImageService.GetFullMediaUrl(new MMMedia() { FileUrl = post.CoverUrl }),
                Title = post?.Title,
                AreaCode = post?.AreaCode,
                PostId = post?.PostId,
            };
        }

        /// <summary>
        /// 服務中退款
        /// </summary>
        /// <param name="booking">預約單</param>
        /// <param name="refundData">申請退費資料</param>
        /// <returns></returns>
        private async Task InServiceRefund(MMBooking booking, ReqBookingRefund refundData, MMMedia[] media)
        {
            booking.Status = BookingStatus.RefundInProgress;
            booking.CancelTime = DateTimeProvider.Now;

            var applyRefund = new MMApplyRefund()
            {
                UserId = refundData.UserId,
                BookingId = refundData.BookingId,
                ReasonType = (byte)refundData.ReasonType,
                Reason = refundData.Describe,
                ApplyTime = DateTimeProvider.Now,
                Status = (byte)ReviewStatus.UnderReview
            };

            await BookingRepo.ApplyRefund(booking, applyRefund, media);
        }

        /// <summary>
        /// 退款
        /// </summary>
        /// <param name="booking">原始預約單</param>
        /// <param name="status">退款後狀態</param>
        /// <returns></returns>
        private async Task Refund(MMBooking booking, BookingStatus status)
        {
            BookingRefundModel bookingRefund = new()
            {
                BookingId = booking.BookingId,
                Status = status,
                CancelTime = DateTimeProvider.Now,
                OriginalStatus = booking.Status,
            };

            string incomeId = await BookingRepo.GetSequenceIdentity<MMIncomeExpenseModel>();

            MMOfficialPost? post = await PostRepo.GetOfficialPostById(booking.PostId);

            MMIncomeExpenseModel income = new()
            {
                Amount = booking.PaymentMoney,
                Category = IncomeExpenseCategoryEnum.Official,
                CreateTime = DateTimeProvider.Now,
                DistributeTime = DateTimeProvider.Now,
                Memo = "预约退款",
                Status = IncomeExpenseStatusEnum.Refund,
                Id = incomeId,
                PayType = IncomeExpensePayType.Point,
                Rebate = 1,
                SourceId = booking.BookingId,
                TargetId = string.Empty,
                Title = post?.Title ?? string.Empty,
                TransactionType = IncomeExpenseTransactionTypeEnum.Refund,
                UnusualMemo = string.Empty,
                UserId = booking.UserId,
            };
            await BookingRepo.Refund(new RefundModel(bookingRefund, income));

            await ZeroOneApiService.PointIncome(new ZOPointIncomeExpenseReq(ZOIncomeExpenseCategory.UnBooking, booking.UserId, booking.PaymentMoney, incomeId));
        }

        /// <summary>
        /// 支付金及預約金計算
        /// </summary>
        /// <param name="param"></param>
        /// <param name="price"></param>
        /// <param name="user"></param>
        /// <param name="postUser">发帖人身份</param>
        /// <returns></returns>
        private static BookingAmountData GetBookingAmountData(MMOfficialPostPrice price, UserInfoData user, UserInfoData postUser)
        {
            //原始預約金(鑽石)
            decimal originalBookingAmount = GetOriginalBookingAmount(price.ComboPrice);

            decimal discountRete = user.Discount(PostType.Official);
            //超觅老板发布的帖子用户预定时不享受任何折扣
            if (postUser.UserInfo.UserIdentity == (int)IdentityType.SuperBoss)
            {
                discountRete = 1;
            }

            //實際預約金(鑽石)
            decimal bookingAmount = discountRete * originalBookingAmount;
            //折扣(鑽石)
            decimal discount = originalBookingAmount - bookingAmount;
            //全額支付金額
            decimal fullAmount = price.ComboPrice * 10 - discount;

            return new BookingAmountData(originalBookingAmount, bookingAmount, fullAmount, discount);
        }

        /// <summary>
        /// 取得贴子價格
        /// </summary>
        /// <param name="postPriceId"></param>
        /// <returns></returns>
        /// <exception cref="MMException"></exception>
        private async Task<MMOfficialPostPrice> GetPostPrice(int postPriceId)
        {
            MMOfficialPostPrice price = await OfficialPostPriceRepo.GetById(postPriceId);

            if (price == null)
            {
                throw new MMException(ReturnCode.DataIsNotExist, "该贴子价格不存在");
            }

            return price;
        }

        /// <summary>
        /// 取得官方贴
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        /// <exception cref="MMException"></exception>
        private async Task<MMOfficialPost> GetPost(string postId)
        {
            MMOfficialPost? post = await PostRepo.GetOfficialPostById(postId);

            if (post == null)
            {
                throw new MMException(ReturnCode.DataIsNotExist, "该贴子不存在");
            }

            return post;
        }

        /// <summary>
        /// 預約扣款
        /// </summary>
        /// <param name="req"></param>
        /// <param name="paymentMoney"></param>
        /// <param name="expenseId"></param>
        /// <returns></returns>
        /// <exception cref="MMException"></exception>
        private async Task ZeroOneBookingExpense(ReqBooking req, decimal paymentMoney, string expenseId)
        {
            var zeroOneResult = await ZeroOneApiService.PointExpense(new ZOPointIncomeExpenseReq(ZOIncomeExpenseCategory.Booking, req.UserId, paymentMoney, expenseId));
            if (zeroOneResult.IsSuccess == false)
            {
                throw new MMException(ReturnCode.ThirdPartyApiNotSuccess, zeroOneResult.Message);
            }
        }

        /// <summary>
        /// 預約鑽石計算
        /// </summary>
        /// <param name="comboPrice">餘額</param>
        /// <returns>鑽石</returns>
        private static decimal GetOriginalBookingAmount(decimal comboPrice)
        {
            switch (comboPrice)
            {
                //小於100會有問題，直接例外
                case < 100:
                    throw new NotImplementedException();

                case >= 100 and <= 1500:
                    return 1000.0000M;

                case >= 1501 and <= 3000:
                    return 2000.0000M;

                case >= 3001 and <= 5000:
                    return 4000.0000M;

                case >= 5001 and <= 8000:
                    return 6000.0000M;

                default:
                    return 8000.0000M;
            }
        }

        /// <summary>
        /// 获取预约次数及被预约次数
        /// <param name="userId">用户id</param>
        /// </summary>
        /// <returns></returns>
        public async Task<int> GetInProgressBookingCount(int userId)
        {
            return await BookingRepo.GetInProgressBookingCount(userId);
        }
        //public async Task<MMBoss> GetBossInfoByUserId(int userId)
        //{
        //    return (await UserInfoRepo.GetIdentityApplyData(userId)).Where(c=>c.Status==1 && c.ApplyIdentity==);
        //}
    }
}