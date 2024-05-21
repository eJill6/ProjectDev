using Serenity.Services;
using MyRequest = Serenity.Services.DeleteRequest;
using MyResponse = Serenity.Services.DeleteResponse;
using MyRow = Management.SystemSettings.LotteryInfoRow;

namespace Management.SystemSettings
{
    public interface ILotteryInfoDeleteHandler : IDeleteHandler<MyRow, MyRequest, MyResponse> {}

    public class LotteryInfoDeleteHandler : DeleteRequestHandler<MyRow, MyRequest, MyResponse>, ILotteryInfoDeleteHandler
    {
        public LotteryInfoDeleteHandler(IRequestContext context)
             : base(context)
        {
        }
    }
}