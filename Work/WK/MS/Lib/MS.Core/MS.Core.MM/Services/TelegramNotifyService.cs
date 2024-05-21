using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MS.Core.MM.Models.Entities.Post;
using MS.Core.MM.Models.Post;
using MS.Core.MM.Repos.interfaces;
using MS.Core.MM.Services.interfaces;
using MS.Core.MMModel.Models.Post.Enums;
using MS.Core.Models.Models;
using Newtonsoft.Json;
using Telegram.Bot;

namespace MS.Core.MM.Services
{
    /// <summary>
    /// Telegram通知服務
    /// </summary>
    public class TelegramNotifyService : INotifyService
    {
        /// <summary>
        /// 帖子相關repo
        /// </summary>
        private IPostRepo _postRepo;

        /// <summary>
        /// 預約相關repo
        /// </summary>
        private IBookingRepo _bookingRepo;

        /// <summary>
        /// 覓老闆相關repo
        /// </summary>
        private IBossShopRepo _bossRepo;

        /// <summary>
        /// 申請資料
        /// </summary>
        private IIdentityApplyRepo _identityApplyRepo;

        /// <summary>
        /// TG設定
        /// </summary>
        private IOptionsMonitor<TelegramSetting> _setting;

        /// <summary>
        /// 日誌相關
        /// </summary>
        private ILogger<TelegramNotifyService> _logger;

        private readonly static string[] _message = new string[3]
        {
            "下单【{0}】用户下单【{1}】，已支付【{2}】，请接单。",
            "退款【{0}】的【{1}】，用户申请退款【{2}】，请注意。",
            "投诉【{0}】的【{1}】，用户投诉，请保留接单对话截图。"
        };

        public TelegramNotifyService(IPostRepo postRepo,
            IBossShopRepo bossRepo,
            IIdentityApplyRepo identityApplyRepo,
            IBookingRepo bookingRepo,
            IOptionsMonitor<TelegramSetting> setting,
            ILogger<TelegramNotifyService> logger)
        {
            _postRepo = postRepo;
            _bossRepo = bossRepo;
            _identityApplyRepo = identityApplyRepo;
            _bookingRepo = bookingRepo;
            _setting = setting;
            _logger = logger;
        }

        public async Task NotifyBooking(string bookingId)
        {
            try
            {
                var booking = await _bookingRepo.GetById(bookingId, true);
                if (booking != null)
                {
                    var post = await _postRepo.GetOfficialPostById(booking.PostId);
                    if (post != null)
                    {
                        var message = string.Format(_message[(int)NotifyEventType.Booking],
                            DateTime.Now.ToString(GlobalSettings.DateTimeFormat),
                            post.Title,
                            booking.PaymentMoney.ToString("0"));
                        await Notify(post, message);
                    }
                    else
                    {
                        _logger.LogError($"處理預約通知失敗, 帖子不存在, PostId:{booking.PostId}");
                    }
                }
                else
                {
                    _logger.LogError($"處理預約通知失敗, 預約單不存在, bookingId:{bookingId}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"處理預約通知失敗, bookingId:{bookingId}");
            }
        }

        public async Task NotifyRefund(string bookingId)
        {
            try
            {
                var booking = await _bookingRepo.GetById(bookingId);
                if (booking != null)
                {
                    var post = await _postRepo.GetOfficialPostById(booking.PostId);
                    if (post != null)
                    {
                        var message = string.Format(_message[(int)NotifyEventType.Refund],
                            DateTime.Now.ToString(GlobalSettings.DateTimeFormat),
                            post.Title,
                            booking.PaymentMoney.ToString("0"));
                        await Notify(post, message);
                    }
                }
                else
                {
                    _logger.LogError($"處理退款通知失敗, 預約單不存在, bookingId:{bookingId}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"處理退款通知失敗, bookingId:{bookingId}");
            }
        }

        public async Task NotifyReport(string postId)
        {
            try
            {
                var post = await _postRepo.GetOfficialPostById(postId);
                if (post != null)
                {
                    var message = string.Format(_message[(int)NotifyEventType.Report], DateTime.Now.ToString(GlobalSettings.DateTimeFormat), post.Title);
                    await Notify(post, message);
                }
                else
                {
                    _logger.LogError($"處理舉報通知失敗, 帖子不存在, postId:{postId}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"處理舉報通知失敗, postId:{postId}");
            }
        }

        /// <inheritdoc/>
        private async Task Notify(MMOfficialPost post, string message)
        {
            var chatId = string.Empty;
            try
            {
                var identity = await _identityApplyRepo.DetailByUserId(post.UserId, (int)ReviewStatus.Approval);
                if (identity != null)
                {
                    var boss = await _bossRepo.GetBossDetailByApplyId(identity.ApplyId);
                    chatId = boss.TelegramGroupId;
                    if (boss != null && !string.IsNullOrEmpty(chatId))
                    {
                        var botClient = new TelegramBotClient(_setting.CurrentValue.Token);
                        await botClient.SendTextMessageAsync(chatId: chatId,
                            text: message);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"發送Tg通知發生異常, post:{JsonConvert.SerializeObject(post)}, message:{message}, chatId:{chatId}");
            }
        }
    }
}
