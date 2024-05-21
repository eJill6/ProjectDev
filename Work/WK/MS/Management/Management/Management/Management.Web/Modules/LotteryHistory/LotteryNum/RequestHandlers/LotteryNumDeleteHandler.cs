using Serenity.Services;
using MyRequest = Serenity.Services.DeleteRequest;
using MyResponse = Serenity.Services.DeleteResponse;
using MyRow = Management.LotteryHistory.LotteryNumRow;

namespace Management.LotteryHistory
{
    public interface ILotteryNumDeleteHandler : IDeleteHandler<MyRow, MyRequest, MyResponse> {}

    public class LotteryNumDeleteHandler : DeleteRequestHandler<MyRow, MyRequest, MyResponse>, ILotteryNumDeleteHandler
    {
        public LotteryNumDeleteHandler(IRequestContext context)
             : base(context)
        {
        }
    }
}