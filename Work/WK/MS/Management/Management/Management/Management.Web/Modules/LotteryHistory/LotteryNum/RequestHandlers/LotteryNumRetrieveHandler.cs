using Serenity.Services;
using MyRequest = Serenity.Services.RetrieveRequest;
using MyResponse = Serenity.Services.RetrieveResponse<Management.LotteryHistory.LotteryNumRow>;
using MyRow = Management.LotteryHistory.LotteryNumRow;

namespace Management.LotteryHistory
{
    public interface ILotteryNumRetrieveHandler : IRetrieveHandler<MyRow, MyRequest, MyResponse> {}

    public class LotteryNumRetrieveHandler : RetrieveRequestHandler<MyRow, MyRequest, MyResponse>, ILotteryNumRetrieveHandler
    {
        public LotteryNumRetrieveHandler(IRequestContext context)
             : base(context)
        {
        }
    }
}