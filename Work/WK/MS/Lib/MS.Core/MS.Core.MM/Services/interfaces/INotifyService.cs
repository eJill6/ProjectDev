namespace MS.Core.MM.Services.interfaces
{
    public interface INotifyService
    {
        Task NotifyBooking(string bookingId);
        Task NotifyRefund(string bookingId);
        Task NotifyReport(string postId);
    }
}
